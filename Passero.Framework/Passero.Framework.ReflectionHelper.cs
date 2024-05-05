using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{
    public class ReflectionHelper
    {
        public static object? CallByName(object? ObjectRef,string ProcName, CallType UseCallType, params object?[] Args  )
        {
            return Microsoft.VisualBasic.Interaction.CallByName(ObjectRef, ProcName, UseCallType, Args);
        }

        public static Type GetListType(object List)
        {
            if (List == null)
                return null;    

            var type = List.GetType();

            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(List<>))
                throw new ArgumentException("Type must be List<>, but was " + type.FullName, "List");

            return type.GetGenericArguments()[0];
        }


        public static IBindingList GetBindingListOfType(Type t)
        {
            Type listType = typeof(BindingList<>);
            Type constructedListType = listType.MakeGenericType(t);
            object instance = Activator.CreateInstance(constructedListType);
            return (IBindingList)instance;
        }

        public static IList GetListOfType(Type t)
        {
            Type listType = typeof(List<>);
            Type constructedListType = listType.MakeGenericType(t);
            object instance = Activator.CreateInstance(constructedListType);
            return (IList)instance;
        }

      

        //public static Type GetBindingListType(object List)
        //{
        //    if (List == null)
        //        return null;

        //    var type = List.GetType();

        //    if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(BindingList<>))
        //        throw new ArgumentException("Type must be BindingList<>, but was " + type.FullName, "List");

        //    return type.GetGenericArguments()(0);
        //}

        public static bool IsBindingList(object List)
        {
            if (List == null)
                return default(Boolean);

            var type = List.GetType();

            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(BindingList<>))
                return false;

            return true;
        }
        public static bool IsList(object List)
        {
            if (List == null)
                return default(Boolean);

            var type = List.GetType();

            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(List<>))
                return false;

            return true;
        }
        public static object ConvertBindingListToList(object objContainingBindingList, Type T)
        {
            // Create a new List<object> and add elements from BindingList
            var newList = GetListOfType(T);
            foreach (var item in (IEnumerable)objContainingBindingList)
            {
                newList.Add(item);
            }
            // Return the List<object> as an object
            return newList;
        }



        public static object CreateClassInstanceFromAssembly(string AssemblyName, string ClassName)
        {
            Assembly assembly = Assembly.LoadFrom(AssemblyName);

            // Walk through each type in the assembly looking for our class
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass == true)
                {
                    if (type.FullName.EndsWith("." + ClassName))
                    {
                        // create an instance of the object
                        object ClassObj = Activator.CreateInstance(type);
                        return ClassObj;
                    }
                }
            }
            return null;
        }

        public static T Clone<T>(T source)
        {
            if (ReferenceEquals(source, null))
                return default;
            T newObject = Conversions.ToGenericParameter<T>(Activator.CreateInstance(source.GetType()));
            var deserializeSettings = new JsonSerializerSettings()
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
                //, ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                //, PreserveReferencesHandling = PreserveReferencesHandling.All 
            };


            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }

        public static string GetPropertyName<T>(string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            return propertyInfo.Name;
        }

        public static object GetPropertyValue(object Object, string PropertyName)
        {
            //because of "?." will return null if property not found
            return Object.GetType().GetProperty(PropertyName)?.GetValue(Object, null);
        }

        public static object GetPropertyValue<T>(T Object, string PropertyName)
        {
            //because of "?." will return null if property not found
            return Object.GetType().GetProperty(PropertyName)?.GetValue(Object, null);
        }

        public static object GetPropertyType(object Object, string PropertyName)
        {
            //because of "?." will return null if property not found
            return Object.GetType().GetProperty(PropertyName)?.PropertyType;
        }

        public static void SetPropertyValue(ref object Object, string PropertyName, object PropertyValue)
        {
            //because of "?." will return null if property not found
            Object.GetType().GetProperty(PropertyName)?.SetValue(Object, PropertyValue);
        }

        public static void SetPropertyValue<T>(ref T Object, string PropertyName, object PropertyValue)
        {
            if (Object is null)
                return;

            //because of "?." will return null if property not found
            Object.GetType().GetProperty(PropertyName)?.SetValue(Object, PropertyValue);
        }

        public static object InvokeMethod2(ref object Object, string MethodName, params object[] MethodParameters)
        {
            object result = null;

            if (Object is null)
                return result ;

            Type type = Object.GetType();

            MethodInfo methodInfo = type.GetMethod(MethodName);
            if (methodInfo != null)
            {
             
                ParameterInfo[] parameters = methodInfo.GetParameters();
                object classInstance = Object;// Activator.CreateInstance(type, null);

                if (parameters.Length == 0)
                {
                    // This works fine
                    result = methodInfo.Invoke(classInstance, null);
                }
                else
                {
                    object[] parametersArray = MethodParameters;

                    // The invoke does NOT work;
                    // it throws "Object does not match target type"             
                    result = methodInfo.Invoke(methodInfo, parametersArray);
                }
            }
            return result;
        }


        public static object InvokeMethod(ref object Object, string MethodName, params object[] MethodParameters)
        {
            return Interaction.CallByName(Object, MethodName, CallType.Method, MethodParameters);
        }

        public static object InvokeMethod<T>(ref T Object, string MethodName, params object[] MethodParameters)
        {
            return Interaction.CallByName(Object, MethodName, CallType.Method, MethodParameters);
        }

    }

}

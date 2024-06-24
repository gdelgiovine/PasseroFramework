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




        public static bool Compare<T>(T Object1, T object2)
        {
            //Get the type of the object
            Type type = typeof(T);

            //return false if any of the object is false
            if (object.Equals(Object1, default(T)) || object.Equals(object2, default(T)))
            {
                return false;
            }

            //Loop through each properties inside class and get values for the property from both the objects and compare
            foreach (System.Reflection.PropertyInfo property in type.GetProperties())
            {
                if (property.Name != "ExtensionData")
                {
                    string Object1Value = string.Empty;
                    string Object2Value = string.Empty;
                    if (type.GetProperty(property.Name).GetValue(Object1, null) != null)
                    {
                        Object1Value = type.GetProperty(property.Name).GetValue(Object1, null).ToString();
                    }
                    if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                    {
                        Object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                    }
                    if (Object1Value.Trim() != Object2Value.Trim())
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        public static object CallByName(object ObjectRef, string ProcName, CallType UseCallType, params object[] Args)
        {
            
          return Microsoft.VisualBasic.Interaction.CallByName(ObjectRef, ProcName, UseCallType, Args);
            
        }

        public static object CallByName(object ObjectRef, string ProcName, CallType UseCallType)
        {

            return Microsoft.VisualBasic.Interaction.CallByName(ObjectRef, ProcName, UseCallType);

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

        /// <summary>
        /// Allows late bound invocation of
        /// properties and methods.
        /// </summary>
        /// <param name="target">Object implementing the property or method.</param>
        /// <param name="methodName">Name of the property or method.</param>
        /// <param name="callType">Specifies how to invoke the property or method.</param>
        /// <param name="args">List of arguments to pass to the method.</param>
        /// <returns>The result of the property or method invocation.</returns>
        public static object _CallByName(object target, string methodName, CallType callType, params object[] args)
        {
            switch (callType)
            {
                case CallType.Get:
                    {
                        PropertyInfo p = target.GetType().GetProperty(methodName);
                        return p.GetValue(target, args);
                    }
                case CallType.Let:
                case CallType.Set:
                    {
                        PropertyInfo p = target.GetType().GetProperty(methodName);
                        p.SetValue(target, args[0], null);
                        return null;
                    }
                case CallType.Method:
                    {
                        MethodInfo m = target.GetType().GetMethod(methodName);
                        return m.Invoke(target, args);
                    }
            }
            return null;
        }


        public static object InvokeMethod2(ref object RefObject, string MethodName, params object[] MethodParameters)
        {

            if (RefObject is null)
                return null ;
            object result= new object();
            Type type = RefObject.GetType();
            MethodInfo methodInfo = type.GetMethod(MethodName);
            if (methodInfo != null)
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                //object classInstance = RefObject;// Activator.CreateInstance(type, null);

                if (parameters.Length ==0)
                {
                    result = methodInfo.Invoke(RefObject , null);
                }
                else
                {
                    object[] parametersArray = MethodParameters;

                    // The invoke does NOT work;
                    // it throws "Object does not match target type"
                    try
                    {
                        result = (object)methodInfo.Invoke(RefObject, parametersArray);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                    
                }
            }
            return result;
        }


        public static object InvokeMethodByName(ref object Object, string MethodName, params object[] MethodParameters)
        {
            return Interaction.CallByName(Object, MethodName, CallType.Method, MethodParameters);
        }

        public static object InvokeMethodByName(ref object Object, string MethodName)
        {
            return Interaction.CallByName(Object, MethodName, CallType.Method);
        }

        public static object InvokeMethodByName<T>(ref T Object, string MethodName, params object[] MethodParameters)
        {
            return Interaction.CallByName(Object, MethodName, CallType.Method, MethodParameters);
        }

        public static object InvokeMethodByName<T>(ref T Object, string MethodName)
        {
            return Interaction.CallByName(Object, MethodName, CallType.Method);
        }

    }

}

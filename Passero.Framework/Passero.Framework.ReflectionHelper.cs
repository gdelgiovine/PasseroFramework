
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Passero.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public class ReflectionHelper
    {
        public static Type GetDTOClassType(object viewModelInstance)
        {
            return viewModelInstance.GetType().GetGenericArguments()[0];
        }

        /// <summary>
        /// Compares the specified object1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object1">The object1.</param>
        /// <param name="object2">The object2.</param>
        /// <returns></returns>
        public static bool Compare<T>(T Object1, T object2)
        {


            //return false if any of the object is false
            if (object.Equals(Object1, default(T)) || object.Equals(object2, default(T)))
            {
                return false;
            }
            //Get the type of the object
            Type type = typeof(T);

            //Loop through each properties inside class and get values for the property from both the objects and compare
            foreach (System.Reflection.PropertyInfo property in type.GetProperties())
            {
               

                if (property.Name != "ExtensionData")
                {
                    string Object1Value = Convert.ToString(property.GetValue(Object1));
                    string Object2Value = Convert.ToString(property.GetValue(object2));
                    if (Object1Value.Trim() != Object2Value.Trim())
                    {
                        return false;
                    }
                }

            }
            return true;
        }


      


        /// <summary>
        /// Calls the name of the by.
        /// </summary>
        /// <param name="ObjectRef">The object reference.</param>
        /// <param name="ProcName">Name of the proc.</param>
        /// <param name="UseCallType">Type of the use call.</param>
        /// <param name="Args">The arguments.</param>
        /// <returns></returns>
        public static object CallByNameVB(object ObjectRef, string ProcName, CallType UseCallType, params object[] Args)
        {

            return Microsoft.VisualBasic.Interaction.CallByName(ObjectRef, ProcName, UseCallType, Args);

        }

        /// <summary>
        /// Calls the name of the by.
        /// </summary>
        /// <param name="ObjectRef">The object reference.</param>
        /// <param name="ProcName">Name of the proc.</param>
        /// <param name="UseCallType">Type of the use call.</param>
        /// <returns></returns>
        public static object CallByNameVB(object ObjectRef, string ProcName, CallType UseCallType)
        {

            return Microsoft.VisualBasic.Interaction.CallByName(ObjectRef, ProcName, UseCallType);

        }


        // Cache per migliorare le prestazioni della riflessione
        private static readonly ConcurrentDictionary<string, MemberInfo> _memberCache =
            new ConcurrentDictionary<string, MemberInfo>();
        private static readonly ConcurrentDictionary<string, MethodInfo[]> _methodCache =
            new ConcurrentDictionary<string, MethodInfo[]>();

        /// <summary>
        /// Calls the name of the by - Optimized version.
        /// </summary>
        /// <param name="ObjectRef">The object reference.</param>
        /// <param name="ProcName">Name of the proc.</param>
        /// <param name="UseCallType">Type of the use call.</param>
        /// <param name="Args">The arguments.</param>
        /// <returns></returns>
        public static object CallByName(object ObjectRef, string ProcName, CallType UseCallType, params object[] Args)
        {
            if (ObjectRef == null)
                throw new ArgumentNullException(nameof(ObjectRef));
            if (string.IsNullOrEmpty(ProcName))
                throw new ArgumentException("Procedure name cannot be null or empty", nameof(ProcName));
            //return Microsoft.VisualBasic.Interaction.CallByName(ObjectRef, ProcName, UseCallType, Args);
            return CallByNameOptimized(ObjectRef, ProcName, UseCallType, Args);
        }

        /// <summary>
        /// Calls the name of the by - Optimized version.
        /// </summary>
        /// <param name="ObjectRef">The object reference.</param>
        /// <param name="ProcName">Name of the proc.</param>
        /// <param name="UseCallType">Type of the use call.</param>
        /// <returns></returns>
        public static object CallByName(object ObjectRef, string ProcName, CallType UseCallType)
        {
            return CallByName(ObjectRef, ProcName, UseCallType, null);
        }

        /// <summary>
        /// Optimized implementation of CallByName using caching for better performance.
        /// </summary>
        private static object CallByNameOptimized(object target, string memberName, CallType callType, params object[] args)
        {
            if (target == null) return null;

            var targetType = target.GetType();
            var cacheKey = $"{targetType.FullName}.{memberName}";

            try
            {
                switch (callType)
                {
                    case CallType.Get:
                        return GetPropertyValueCached(target, memberName, cacheKey, args);

                    case CallType.Let:
                    case CallType.Set:
                        SetPropertyValueCached(target, memberName, cacheKey, args);
                        return null;

                    case CallType.Method:
                        return InvokeMethodCached(target, memberName, cacheKey, args);

                    default:
                        throw new ArgumentException($"Unsupported CallType: {callType}");
                }
            }
            catch (Exception ex)
            {
                // Fallback to VB.NET implementation for compatibility
                return Microsoft.VisualBasic.Interaction.CallByName(target, memberName, callType, args);
            }
        }

        private static object GetPropertyValueCached(object target, string propertyName, string cacheKey, object[] args)
        {
            var propertyInfo = _memberCache.GetOrAdd(cacheKey, key =>
                target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)) as PropertyInfo;

            if (propertyInfo == null)
                throw new ArgumentException($"Property '{propertyName}' not found on type '{target.GetType().Name}'");

            return propertyInfo.GetValue(target, args);
        }

        private static void SetPropertyValueCached(object target, string propertyName, string cacheKey, object[] args)
        {
            var propertyInfo = _memberCache.GetOrAdd(cacheKey, key =>
                target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)) as PropertyInfo;

            if (propertyInfo == null)
                throw new ArgumentException($"Property '{propertyName}' not found on type '{target.GetType().Name}'");

            if (!propertyInfo.CanWrite)
                throw new ArgumentException($"Property '{propertyName}' is read-only");

            propertyInfo.SetValue(target, args?[0], null);
        }

        private static object InvokeMethodCached(object target, string methodName, string cacheKey, object[] args)
        {
            var methods = _methodCache.GetOrAdd(cacheKey, key =>
                target.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.Name == methodName)
                    .ToArray());

            if (methods.Length == 0)
                throw new ArgumentException($"Method '{methodName}' not found on type '{target.GetType().Name}'");

            // Trova il metodo con il numero di parametri corretto
            var method = FindBestMatchingMethod(methods, args);
            if (method == null)
                throw new ArgumentException($"No suitable overload found for method '{methodName}' with {args?.Length ?? 0} parameters");

            return method.Invoke(target, args);
        }

        //private static MethodInfo FindBestMatchingMethod(MethodInfo[] methods, object[] args)
        //{
        //    var argCount = args?.Length ?? 0;

        //    // Prima cerca corrispondenza esatta per numero di parametri
        //    var exactMatch = methods.FirstOrDefault(m => m.GetParameters().Length == argCount);
        //    if (exactMatch != null) return exactMatch;

        //    // Poi cerca metodi con parametri params
        //    return methods.FirstOrDefault(m =>
        //    {
        //        var parameters = m.GetParameters();
        //        return parameters.Length > 0 &&
        //               parameters[1].IsDefined(typeof(ParamArrayAttribute), false) &&
        //               parameters.Length - 1 <= argCount;
        //    });
        //}

        private static MethodInfo FindBestMatchingMethod(MethodInfo[] methods, object[] args)
        {
            var argCount = args?.Length ?? 0;

            // Prima cerca corrispondenza esatta per numero di parametri
            var exactMatch = methods.FirstOrDefault(m => m.GetParameters().Length == argCount);
            if (exactMatch != null) return exactMatch;

            // Poi cerca metodi con parametri params
            return methods.FirstOrDefault(m =>
            {
                var parameters = m.GetParameters();
                return parameters.Length > 0 &&
                       parameters[parameters.Length - 1].IsDefined(typeof(ParamArrayAttribute), false) && // Controlla l'ultimo parametro
                       parameters.Length - 1 <= argCount;
            });
        }
        /// <summary>
        /// Gets the type of the list.
        /// </summary>
        /// <param name="List">The list.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Type must be List<>, but was " + type.FullName - List</exception>
        public static Type GetListType(object List)
        {
            if (List == null)
                return null;

            var type = List.GetType();

            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(List<>))
                throw new ArgumentException("Type must be List<>, but was " + type.FullName, "List");

            return type.GetGenericArguments()[0];
        }


        /// <summary>
        /// Gets the type of the binding list of.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public static IBindingList GetBindingListOfType(Type t)
        {
            Type listType = typeof(BindingList<>);
            Type constructedListType = listType.MakeGenericType(t);
            object instance = Activator.CreateInstance(constructedListType);
            return (IBindingList)instance;
        }

        /// <summary>
        /// Gets the type of the list of.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public static IList GetListOfType(Type t)
        {
            Type listType = typeof(List<>);
            Type constructedListType = listType.MakeGenericType(t);
            object instance = Activator.CreateInstance(constructedListType);
            return (IList)instance;
        }

                
        /// <summary>
        /// Determines whether [is binding list] [the specified list].
        /// </summary>
        /// <param name="List">The list.</param>
        /// <returns>
        ///   <c>true</c> if [is binding list] [the specified list]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBindingList(object List)
        {
            if (List == null)
                return default(Boolean);

            var type = List.GetType();

            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(BindingList<>))
                return false;

            return true;
        }
        /// <summary>
        /// Determines whether the specified list is list.
        /// </summary>
        /// <param name="List">The list.</param>
        /// <returns>
        ///   <c>true</c> if the specified list is list; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsList(object List)
        {
            if (List == null)
                return default(Boolean);

            var type = List.GetType();

            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(List<>))
                return false;

            return true;
        }
        /// <summary>
        /// Converts the binding list to list.
        /// </summary>
        /// <param name="objContainingBindingList">The object containing binding list.</param>
        /// <param name="T">The t.</param>
        /// <returns></returns>
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



        /// <summary>
        /// Creates the class instance from assembly.
        /// </summary>
        /// <param name="AssemblyName">Name of the assembly.</param>
        /// <param name="ClassName">Name of the class.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Clones the specified source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static string GetPropertyName<T>(string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            return propertyInfo.Name;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="Object">The object.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <returns></returns>
        public static object GetPropertyValue(object Object, string PropertyName)
        {
            //because of "?." will return null if property not found
            return Object.GetType().GetProperty(PropertyName)?.GetValue(Object, null);
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object">The object.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <returns></returns>
        public static object GetPropertyValue<T>(T Object, string PropertyName)
        {
            //because of "?." will return null if property not found
            return Object.GetType().GetProperty(PropertyName)?.GetValue(Object, null);
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <param name="Object">The object.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <returns></returns>
        public static object GetPropertyType(object Object, string PropertyName)
        {
            //because of "?." will return null if property not found
            return Object.GetType().GetProperty(PropertyName)?.PropertyType;
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="Object">The object.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <param name="PropertyValue">The property value.</param>
        public static void SetPropertyValue(ref object Object, string PropertyName, object PropertyValue)
        {
            //because of "?." will return null if property not found
            Object.GetType().GetProperty(PropertyName)?.SetValue(Object, PropertyValue);
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object">The object.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <param name="PropertyValue">The property value.</param>
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
        /// <returns>
        /// The result of the property or method invocation.
        /// </returns>
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


        /// <summary>
        /// Invokes the method2.
        /// </summary>
        /// <param name="RefObject">The reference object.</param>
        /// <param name="MethodName">Name of the method.</param>
        /// <param name="MethodParameters">The method parameters.</param>
        /// <returns></returns>
        public static object InvokeMethod2(ref object RefObject, string MethodName, params object[] MethodParameters)
        {

            if (RefObject is null)
                return null;
            object result = new object();
            Type type = RefObject.GetType();
            MethodInfo methodInfo = type.GetMethod(MethodName);
            if (methodInfo != null)
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                //object classInstance = RefObject;// Activator.CreateInstance(type, null);

                if (parameters.Length == 0)
                {
                    result = methodInfo.Invoke(RefObject, null);
                }
                else
                {
                    object[] parametersArray = MethodParameters;

                    // The invoke does NOT work;
                    // it throws "Object does not match target type"
                    try
                    {
                        result = methodInfo.Invoke(RefObject, parametersArray);
                    }
                    catch (Exception)
                    {

                        throw;
                    }


                }
            }
            return result;
        }


        /// <summary>
        /// Invokes the name of the method by.
        /// </summary>
        /// <param name="Object">The object.</param>
        /// <param name="MethodName">Name of the method.</param>
        /// <param name="MethodParameters">The method parameters.</param>
        /// <returns></returns>
        public static object InvokeMethodByName(ref object Object, string MethodName, params object[] MethodParameters)
        {
            return Interaction.CallByName(Object, MethodName, CallType.Method, MethodParameters);
        }

        /// <summary>
        /// Invokes the name of the method by.
        /// </summary>
        /// <param name="Object">The object.</param>
        /// <param name="MethodName">Name of the method.</param>
        /// <returns></returns>
        public static object InvokeMethodByName(ref object Object, string MethodName)
        {
            return Interaction.CallByName(Object, MethodName, CallType.Method);
        }

        /// <summary>
        /// Invokes the name of the method by.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object">The object.</param>
        /// <param name="MethodName">Name of the method.</param>
        /// <param name="MethodParameters">The method parameters.</param>
        /// <returns></returns>
        public static object InvokeMethodByName<T>(ref T Object, string MethodName, params object[] MethodParameters)
        {
            return Interaction.CallByName(Object, MethodName, CallType.Method, MethodParameters);
        }

        /// <summary>
        /// Invokes the name of the method by.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object">The object.</param>
        /// <param name="MethodName">Name of the method.</param>
        /// <returns></returns>
        public static object InvokeMethodByName<T>(ref T Object, string MethodName)
        {
            return Interaction.CallByName(Object, MethodName, CallType.Method);
        }

    }

}

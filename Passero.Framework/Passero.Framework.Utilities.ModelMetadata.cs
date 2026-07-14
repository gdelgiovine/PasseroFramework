using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Passero.Framework
{
    public static partial class Utilities
    {

        private static readonly ConcurrentDictionary<string, PropertyInfo[]> _modelPropertyCache =
         new ConcurrentDictionary<string, PropertyInfo[]>();

        private static readonly ConcurrentDictionary<string, List<PropertyInfo>> _modelPrimaryKeysPropertiesInfoCache =
            new ConcurrentDictionary<string, List<PropertyInfo>>();

        private static readonly ConcurrentDictionary<string, string> _modelPrimaryKeyNamesCache =
            new ConcurrentDictionary<string, string>();

        public static void InvalidateModelMetadataCache(Type ModelClass)
        {
            if (ModelClass == null)
                return;

            var key = GetModelMetadataCacheKey(ModelClass);
            _modelPrimaryKeysPropertiesInfoCache.TryRemove(key, out _);
            _modelPrimaryKeyNamesCache.TryRemove(key, out _);
        }

        private static string GetModelMetadataCacheKey(Type ModelClass)
        {
            if (ModelClass == null)
                return string.Empty;

            return ModelClass.AssemblyQualifiedName ?? ModelClass.FullName ?? ModelClass.Name;
        }

        private static bool ShouldCacheModelMetadata(Type ModelClass)
        {
            if (ModelClass == null)
                return false;

            return !string.IsNullOrWhiteSpace(ModelClass.AssemblyQualifiedName);
        }


        public static List<PropertyInfo> GetModelPropertiesInfo(Type ModelClass, bool ExcludeComputed = false)
        {
            if (ModelClass == null)
                throw new ArgumentNullException(nameof(ModelClass));

            var cacheKey = $"{ModelClass.AssemblyQualifiedName}|{ExcludeComputed}";

            var properties = _modelPropertyCache.GetOrAdd(cacheKey, _ =>
            {
                return ModelClass
                    .GetProperties()
                    .Where(p =>
                    {
                        var isComputed = p.GetCustomAttribute<ComputedAttribute>() != null;
                        var isWriteAttribute = p.GetCustomAttribute<WriteAttribute>() != null;

                        if (isComputed || isWriteAttribute)
                            return !ExcludeComputed;

                        return true;
                    })
                    .ToArray();
            });

            return properties.ToList();
        }
        public static List<PropertyInfo> GetModelPropertiesInfoNC(Type ModelClass, bool ExcludeComputed = false)
        {
            var properties = ModelClass.GetProperties();

            List<PropertyInfo> x = new List<PropertyInfo>();
            foreach (PropertyInfo p in properties)
            {
                if (p.GetCustomAttribute<Dapper.Contrib.Extensions.ComputedAttribute>() != null || p.GetCustomAttribute<Dapper.Contrib.Extensions.WriteAttribute>() != null)
                {
                    if (ExcludeComputed == false)
                    {
                        x.Add(p);
                    }
                }
                else
                {
                    x.Add(p);
                }
            }
            return x;
        }


        public static List<PropertyInfo> GetModelPrimaryKeysPropertiesInfo(Type ModelClass)
        {
            if (ModelClass == null)
                throw new ArgumentNullException(nameof(ModelClass));

            if (!ShouldCacheModelMetadata(ModelClass))
                return GetModelPrimaryKeysPropertiesInfoNC(ModelClass);

            var cacheKey = GetModelMetadataCacheKey(ModelClass);
            return _modelPrimaryKeysPropertiesInfoCache.GetOrAdd(cacheKey, _ =>
            {
                var properties = ModelClass.GetProperties();
                List<PropertyInfo> x = new List<PropertyInfo>();
                foreach (PropertyInfo p in properties)
                {
                    if (p.GetCustomAttribute<Dapper.Contrib.Extensions.ExplicitKeyAttribute>() != null || p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null)
                    {
                        x.Add(p);
                    }
                }
                return x;
            }).ToList();
        }

        public static List<PropertyInfo> GetModelPrimaryKeysPropertiesInfoNC(Type ModelClass)
        {
            var properties = ModelClass.GetProperties();
            List<PropertyInfo> x = new List<PropertyInfo>();
            foreach (PropertyInfo p in properties)
            {
                if (p.GetCustomAttribute<Dapper.Contrib.Extensions.ExplicitKeyAttribute>() != null || p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null)
                {
                    x.Add(p);
                }
            }
            return x;
        }


        public static string GetModelPrimaryKeyNames(Type ModelClass)
        {
            if (ModelClass == null)
                throw new ArgumentNullException(nameof(ModelClass));

            if (!ShouldCacheModelMetadata(ModelClass))
                return GetModelPrimaryKeyNamesNC(ModelClass);

            var cacheKey = GetModelMetadataCacheKey(ModelClass);
            return _modelPrimaryKeyNamesCache.GetOrAdd(cacheKey, _ =>
            {
                var properties = ModelClass.GetProperties().Where((p) => p.GetCustomAttribute<Dapper.Contrib.Extensions.ExplicitKeyAttribute>() != null || p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null);
                return string.Join(",", properties.Select((p) => $"{p.Name}"));
            });
        }

        public static string GetModelPrimaryKeyNamesNC(Type ModelClass)
        {
            var properties = ModelClass.GetProperties().Where((p) => p.GetCustomAttribute<Dapper.Contrib.Extensions.ExplicitKeyAttribute>() != null || p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null);

            var values = string.Join(",", properties.Select((p) => $"{p.Name}"));
            return values;
        }


        public static List<string> GetModelPrimaryKeyNamesList(Type ModelClass)
        {
            List<string> x = new List<string>();
            var properties = ModelClass.GetProperties().Where((p) => p.GetCustomAttribute<Dapper.Contrib.Extensions.ExplicitKeyAttribute>() != null || p.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>() != null);

            var values = string.Join(",", properties.Select((p) => $"{p.Name}"));
            x = values.Split(',').ToList();
            return x;
        }


        public static bool PropertyIsWriteable(PropertyInfo pi)
        {

            var attributes = pi.GetCustomAttributes(typeof(WriteAttribute), false).AsList();
            if (attributes.Count != 1)
            {
                return true;
            }
            WriteAttribute writeAttribute = (WriteAttribute)attributes[0];
            return writeAttribute.Write;
        }


        public static bool PropertyIsExplicitKey(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(typeof(ExplicitKeyAttribute), false).AsList();
            if (attributes.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static bool PropertyIsIdentityKey(PropertyInfo pi)
        {
            var attributes = pi.GetCustomAttributes(typeof(KeyAttribute), false).AsList();
            if (attributes.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static bool PropertyIsExplicitKey(Type ModelClass, string PropertyName)
        {
            if (string.IsNullOrEmpty(PropertyName.Trim()))
            {
                return false;
            }

            var pi = ModelClass.GetProperty(PropertyName);
            if (pi == null)
            {
                return false;
            }
            var attributes = pi.GetCustomAttributes(typeof(ExplicitKeyAttribute), false).AsList();

            if (attributes.Count == 0)
            {
                return false;
            }
            return true;
        }


        public static bool PropertyIsIdentityKey(Type ModelClass, string PropertyName)
        {
            if (string.IsNullOrEmpty(PropertyName.Trim()))
            {
                return false;
            }

            var pi = ModelClass.GetProperty(PropertyName);
            if (pi == null)
            {
                return false;
            }

            var attributes = pi.GetCustomAttributes(typeof(KeyAttribute), false).AsList();
            if (attributes.Count == 0)
            {
                return false;
            }
            return true;
        }


        public static string GetMappedColumnName(PropertyInfo pi)
        {
            var attr = pi.GetCustomAttribute<Dapper.ColumnMapper.ColumnMappingAttribute>();
            return attr != null ? attr.ColumnName : pi.Name;
        }


        public static string GetModelPropertyNames(Type ModelClass, bool excludeKey = false)
        {

            var properties = ModelClass.GetProperties().Where(p => !excludeKey || p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() is null);
            string values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            return values;
        }


        public static IEnumerable<PropertyInfo> GetModelPropertiesInfo2(Type ModelClass, bool excludeKey = false)
        {
            var properties = ModelClass.GetProperties().Where(p => !excludeKey || p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() is null);
            return properties;
        }


        public static string GetModelColumnName<T>(string ColumnName)
        {
            var pInfo = typeof(T).GetProperty(ColumnName).GetCustomAttribute<Dapper.ColumnMapper.ColumnMappingAttribute>();
            string _ColumnName = pInfo.ColumnName;
            return _ColumnName;
        }


        public static string GetModelColumnName<T>(object PropertyName)
        {
            string ColumnName = nameof(PropertyName);
            var pInfo = typeof(T).GetProperty(ColumnName).GetCustomAttribute<Dapper.ColumnMapper.ColumnMappingAttribute>();
            string _ColumnName = pInfo.ColumnName;
            return _ColumnName;
        }


        public static string GetModelTableName<T>()
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(typeof(T));

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
            MethodInfo? getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.

            if (getTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

#pragma warning disable CS8603 // Possibile restituzione di riferimento Null.
            return getTableNameMethod.Invoke(null, new object[] { typeof(T) }) as string;
#pragma warning restore CS8603 // Possibile restituzione di riferimento Null.


        }


        public static string GetModelTableName(Type ModelClass)
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(ModelClass);

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
            MethodInfo? getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.

            if (getTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

#pragma warning disable CS8603 // Possibile restituzione di riferimento Null.
            return getTableNameMethod.Invoke(null, new object[] { ModelClass }) as string;
#pragma warning restore CS8603 // Possibile restituzione di riferimento Null.


        }


        public static string GetModelTableName(object Model)
        {
            // Check if we've already set our custom table mapper to TableNameMapper.
            if (SqlMapperExtensions.TableNameMapper != null)
                return SqlMapperExtensions.TableNameMapper(Model.GetType());

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string getTableName = "GetTableName";
#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
            MethodInfo? getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.

            if (getTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

#pragma warning disable CS8603 // Possibile restituzione di riferimento Null.
            return getTableNameMethod.Invoke(null, new object[] { Model.GetType() }) as string;
#pragma warning restore CS8603 // Possibile restituzione di riferimento Null.


        }


        public static string SetModelTableName<T>(T Model, string TableName)
        {

            // If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
            string setTableName = "SetTableName";
#pragma warning disable CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.
            MethodInfo? setTableNameMethod = typeof(SqlMapperExtensions).GetMethod(setTableName, BindingFlags.NonPublic | BindingFlags.Static);
#pragma warning restore CS8632 // L'annotazione per i tipi riferimento nullable deve essere usata solo nel codice in un contesto di annotations '#nullable'.

            if (setTableNameMethod == null)
                throw new ArgumentOutOfRangeException($"Method '{setTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

            Interaction.CallByName(Model, setTableName, CallType.Set, TableName);
            return setTableName;


        }

    }
}
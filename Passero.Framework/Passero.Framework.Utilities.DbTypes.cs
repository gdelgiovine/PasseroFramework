using Dapper.Contrib.Extensions;
using FastDeepCloner;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using MiniExcelLibs;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wisej.Core;
using Wisej.Web;

namespace Passero.Framework
{
    public static partial class Utilities
    {

        // Aggiungi questo campo statico nella classe Utilities
        private static readonly Dictionary<Type, DbType> _typeToDbTypeMap = new Dictionary<Type, DbType>
{
    { typeof(bool), DbType.Boolean },
    { typeof(byte), DbType.Byte },
    { typeof(sbyte), DbType.SByte },
    { typeof(short), DbType.Int16 },
    { typeof(ushort), DbType.UInt16 },
    { typeof(int), DbType.Int32 },
    { typeof(uint), DbType.UInt32 },
    { typeof(long), DbType.Int64 },
    { typeof(ulong), DbType.UInt64 },
    { typeof(float), DbType.Single },
    { typeof(double), DbType.Double },
    { typeof(decimal), DbType.Decimal },
    { typeof(DateTime), DbType.DateTime },
    { typeof(string), DbType.String },
    { typeof(char), DbType.StringFixedLength },
    { typeof(Guid), DbType.Guid },
    { typeof(byte[]), DbType.Binary }
};

        public static Type DbTypeToType(DbType dbType)
        {
            Type toReturn = typeof(DBNull);

            switch (dbType)
            {
                case DbType.String:
                    toReturn = typeof(string);
                    break;

                case DbType.UInt64:
                    toReturn = typeof(UInt64);
                    break;

                case DbType.Int64:
                    toReturn = typeof(Int64);
                    break;

                case DbType.Int32:
                    toReturn = typeof(Int32);
                    break;

                case DbType.UInt32:
                    toReturn = typeof(UInt32);
                    break;

                case DbType.Single:
                    toReturn = typeof(float);
                    break;

                case DbType.Date:
                    toReturn = typeof(DateTime);
                    break;

                case DbType.DateTime:
                    toReturn = typeof(DateTime);
                    break;

                case DbType.Time:
                    toReturn = typeof(DateTime);
                    break;

                case DbType.StringFixedLength:
                    toReturn = typeof(string);
                    break;

                case DbType.UInt16:
                    toReturn = typeof(UInt16);
                    break;

                case DbType.Int16:
                    toReturn = typeof(Int16);
                    break;

                case DbType.SByte:
                    toReturn = typeof(byte);
                    break;

                case DbType.Object:
                    toReturn = typeof(object);
                    break;

                case DbType.AnsiString:
                    toReturn = typeof(string);
                    break;

                case DbType.AnsiStringFixedLength:
                    toReturn = typeof(string);
                    break;

                case DbType.VarNumeric:
                    toReturn = typeof(decimal);
                    break;

                case DbType.Currency:
                    toReturn = typeof(double);
                    break;

                case DbType.Binary:
                    toReturn = typeof(byte[]);
                    break;

                case DbType.Decimal:
                    toReturn = typeof(decimal);
                    break;

                case DbType.Double:
                    toReturn = typeof(Double);
                    break;

                case DbType.Guid:
                    toReturn = typeof(Guid);
                    break;

                case DbType.Boolean:
                    toReturn = typeof(bool);
                    break;
            }

            return toReturn;
        }


        public static object ConvertStringToType(string value, Type type)
        {
            object result;

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            try
            {
                var converter = TypeDescriptor.GetConverter(type);
                result = converter.ConvertFromString(value);
                return result;
            }
            catch (Exception exception)
            {
                // Log this exception if required.
                throw new InvalidCastException(string.Format("Unable to cast the {0} to type {1}", value, type, exception));
            }
        }


        public static DbType GetDbType(Type runtimeType)
        {
            var nonNullableType = Nullable.GetUnderlyingType(runtimeType);
            if (nonNullableType != null)
            {
                runtimeType = nonNullableType;
            }

            if (_typeToDbTypeMap.TryGetValue(runtimeType, out DbType dbType))
            {
                return dbType;
            }

            return DbType.Object;
        }


        public static DbType GetDbTypeA(Type runtimeType)
        {
            var nonNullableType = Nullable.GetUnderlyingType(runtimeType);
            if (nonNullableType != null)
            {
                runtimeType = nonNullableType;
            }

            // Mappatura diretta tra Type e DbType
            if (runtimeType == typeof(string))
                return DbType.String;

            if (runtimeType == typeof(byte[]))
                return DbType.Binary;

            if (runtimeType == typeof(Guid))
                return DbType.Guid;

            switch (Type.GetTypeCode(runtimeType))
            {
                case TypeCode.Boolean:
                    return DbType.Boolean;

                case TypeCode.Byte:
                    return DbType.Byte;

                case TypeCode.SByte:
                    return DbType.SByte;

                case TypeCode.Int16:
                    return DbType.Int16;

                case TypeCode.UInt16:
                    return DbType.UInt16;

                case TypeCode.Int32:
                    return DbType.Int32;

                case TypeCode.UInt32:
                    return DbType.UInt32;

                case TypeCode.Int64:
                    return DbType.Int64;

                case TypeCode.UInt64:
                    return DbType.UInt64;

                case TypeCode.Single:
                    return DbType.Single;

                case TypeCode.Double:
                    return DbType.Double;

                case TypeCode.Decimal:
                    return DbType.Decimal;

                case TypeCode.DateTime:
                    return DbType.DateTime;

                case TypeCode.Char:
                    return DbType.StringFixedLength;

                case TypeCode.String:
                    return DbType.String;

                default:
                    return DbType.Object;
            }
        }


        public static DbType GetDbTypeDbParameter(Type runtimeType)
        {
            var nonNullableType = Nullable.GetUnderlyingType(runtimeType);
            if (nonNullableType != null)
            {
                runtimeType = nonNullableType;
            }

            var templateValue = (Object)null;
            if (runtimeType.IsClass == false)
            {
                templateValue = Activator.CreateInstance(runtimeType);
            }
           
            var sqlParamter = new SqlParameter (parameterName: String.Empty, value: templateValue);

            return sqlParamter.DbType;
        }

    }
}
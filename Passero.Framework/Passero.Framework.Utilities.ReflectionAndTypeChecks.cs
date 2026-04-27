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

        public static void Reflection_GetFields(Type t)
        {
            Console.WriteLine("***** Fields *****");
            FieldInfo[] fields = t.GetFields();
            for (int i = 0, loopTo = fields.Length - 1; i <= loopTo; i++)
                Console.WriteLine("->{0}", fields[i].Name);
            Console.WriteLine("");
        }


        public static void Reflection_GetProperties(Type t)
        {
            Console.WriteLine("***** Properties *****");
            PropertyInfo[] properties = t.GetProperties();
            for (int i = 0, loopTo = properties.Length - 1; i <= loopTo; i++)
                Console.WriteLine("->{0}", properties[i].Name);
            Console.WriteLine("");
        }


        public static string Reflection_GetValidFilename(string Filename)
        {
            string regex = string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars())));
            var removeInvalidChars = new Regex(regex, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
            return regex;
        }


        public static bool IsNumeric(object expression)
        {
            if (expression == null)
                return false;

            double number;
            return Double.TryParse(Convert.ToString(expression
                                                    , CultureInfo.InvariantCulture)
                                  , System.Globalization.NumberStyles.Any
                                  , NumberFormatInfo.InvariantInfo
                                  , out number);
        }


        public static bool IsNumericType(Type Type)
        {
            switch (Type.GetTypeCode(Type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }


        public static bool IsListType(Type type)
        {
            if (null == type)
                return false;

            if (typeof(System.Collections.IList).IsAssignableFrom(type))
                return true;
            foreach (var it in type.GetInterfaces())
                if (it.IsGenericType && typeof(IList<>) == it.GetGenericTypeDefinition())
                    return true;
            return false;
        }


        public static bool IsStringType(Type Type)
        {
            switch (Type.GetTypeCode(Type))
            {
                case TypeCode.String:
                case TypeCode.Char:
                    return true;
                default:
                    return false;
            }
        }


        public static bool IsDateTimeType(Type Type)
        {
            switch (Type.GetTypeCode(Type))
            {
                case TypeCode.DateTime:
                    return true;
                default:
                    return false;
            }
        }


        public static bool IsBooleanType(Type Type)
        {
            switch (Type.GetTypeCode(Type))
            {
                case TypeCode.Boolean:
                    return true;
                default:
                    return false;
            }
        }


        public static bool IsObjectType(Type Type)
        {
            switch (Type.GetTypeCode(Type))
            {
                case TypeCode.Object:
                    return true;
                default:
                    return false;
            }
        }


        public static bool IsEmptyType(Type Type)
        {
            switch (Type.GetTypeCode(Type))
            {
                case TypeCode.Empty:
                    return true;
                default:
                    return false;
            }
        }


        public static EnumSystemTypeIs GetSystemTypeIs(Type Type)
        {
            if (IsStringType(Type))
                return EnumSystemTypeIs.String;
            if (IsNumericType(Type))
                return EnumSystemTypeIs.Numeric;
            if (IsDateTimeType(Type))
                return EnumSystemTypeIs.DateTime;
            if (IsBooleanType(Type))
                return EnumSystemTypeIs.Boolean;
            if (IsObjectType(Type))
                return EnumSystemTypeIs.Object;

            return EnumSystemTypeIs.Empty;
        }

    }
}
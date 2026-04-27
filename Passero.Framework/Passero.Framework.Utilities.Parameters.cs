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

    /// <summary>
    /// 
    /// </summary>
    public static partial class Utilities
    {

        public static Dapper.DynamicParameters CreateDynamicParameters(object obj)
        {
            var parameters = new Dapper.DynamicParameters();

            if (obj == null)
                return parameters;

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                parameters.Add(property.Name, value);
            }

            return parameters;
        }


        public static Dapper.DynamicParameters CreateDynamicParameters(IDictionary<string, object> dictionary)
        {
            var parameters = new Dapper.DynamicParameters();

            if (dictionary == null)
                return parameters;

            foreach (var kvp in dictionary)
            {
                parameters.Add(kvp.Key, kvp.Value);
            }

            return parameters;
        }


        public static void AddParameters(Dapper.DynamicParameters parameters, object obj)
        {
            if (obj == null || parameters == null)
                return;

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                parameters.Add(property.Name, value);
            }
        }


        public static Dapper.DynamicParameters GetDynamicParameters(object Params)
        {
            if (Params == null)
                return null;

            switch (Params.GetType().Name)
            {
                case "VB$AnonymousType_0`1":
                    return null;

                case "DynamicParameters":
                    return (Dapper.DynamicParameters)Params;
            }
            return null;
        }

    }
}
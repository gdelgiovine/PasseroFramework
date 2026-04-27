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

        public static T WisejClone<T>(T source)
        {
            if (ReferenceEquals(source, null))
                return default;

            T newObject = Conversions.ToGenericParameter<T>(Activator.CreateInstance(source.GetType()));
            newObject = Wisej.Core.WisejSerializer .Parse(Wisej.Core.WisejSerializer.Serialize(source));
            return newObject;
        }


        public static T Clone<T>(T source)
        {
            if (ReferenceEquals(source, null))
                return default;

            // Usa FastDeepCloner per creare una copia profonda
            return (T)FastDeepCloner.DeepCloner.Clone(source);
        }


        public static T JsonClone<T>(T source)
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


            newObject = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
            return newObject;
        }


        public static bool ObjectsEquals(object obj, object another)
        {
            if (ReferenceEquals(obj, another))
                return true;
            if (obj is null || another is null)
                return false;
            if (obj.GetType() != another.GetType())
                return false;
            string objJson = JsonConvert.SerializeObject(obj);
            string anotherJson = JsonConvert.SerializeObject(another);
            return (objJson ?? "") == (anotherJson ?? "");
        }

    }
}
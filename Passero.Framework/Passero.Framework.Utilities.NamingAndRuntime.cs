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

        public static string GetObjectName(object obj,
                  [CallerArgumentExpression("obj")] string objectExpression = null)
        {
            if (obj == null)
                return string.Empty;

            string name = string.Empty;

            // 1. Prova a ottenere dalla proprietà Name dell'oggetto
            var nameProperty = obj.GetType().GetProperty("Name");
            if (nameProperty != null && nameProperty.PropertyType == typeof(string))
            {
                string propertyName = (string)nameProperty.GetValue(obj);
                if (!string.IsNullOrWhiteSpace(propertyName))
                {
                    name = propertyName;
                }
            }

            // 2. Se Name è vuoto, usa il nome della variabile catturato
            if (string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(objectExpression))
            {
                // Rimuove "this." solo se è all'inizio (es: "this.vmTitles" -> "vmTitles")
                name = objectExpression.StartsWith("this.")
                    ? objectExpression.Substring(5)
                    : objectExpression;
            }

            // 3. Fallback al nome del tipo
            if (string.IsNullOrWhiteSpace(name))
            {
                name = obj.GetType().Name;
            }

            return name;
        }


        public static string GetViewModelName(object ViewModel,
             [CallerArgumentExpression("ViewModel")] string viewModelExpression = null)
        {

            //string Name = string.Empty;

            //// 1. Prova a ottenere dalla proprietà Name del ViewModel
            //var nameProperty = ViewModel.GetType().GetProperty("Name");
            //if (nameProperty != null && nameProperty.PropertyType == typeof(string))
            //{
            //    string _Name = (string)nameProperty.GetValue(ViewModel) ?? string.Empty;
            //    if (!string.IsNullOrWhiteSpace(_Name))
            //    {
            //        Name = _Name;
            //    }
            //}

            //// 2. Se Name è vuoto, usa il nome della variabile catturato
            //if (string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(viewModelExpression))
            //{
            //    // Rimuove "this." se presente (es: "this.vmTitles" -> "vmTitles")
            //    Name = viewModelExpression.Replace("this.", "");
            //}

            //// 3. Fallback al nome del tipo
            //if (string.IsNullOrWhiteSpace(Name))
            //{
            //    Name = ViewModel.GetType().Name;
            //}
            //return Name;

            return GetModelName(ViewModel, viewModelExpression);    
        }


        public static string GetModelName(object Model,
           [CallerArgumentExpression("Model")] string ModelExpression = null)
        {

            //string Name = string.Empty;

            //// 1. Prova a ottenere dalla proprietà Name del ViewModel
            //var nameProperty = Model.GetType().GetProperty("Name");
            //if (nameProperty != null && nameProperty.PropertyType == typeof(string))
            //{
            //    string _Name = (string)nameProperty.GetValue(Model) ?? string.Empty;
            //    if (!string.IsNullOrWhiteSpace(_Name))
            //    {
            //        Name = _Name;
            //    }
            //}

            //// 2. Se Name è vuoto, usa il nome della variabile catturato
            //if (string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(ModelExpression))
            //{
            //    // Rimuove "this." se presente (es: "this.vmTitles" -> "vmTitles")
            //    Name = ModelExpression.Replace("this.", "");
            //}

            //// 3. Fallback al nome del tipo
            //if (string.IsNullOrWhiteSpace(Name))
            //{
            //    Name = Model.GetType().Name;
            //}
            //return Name;

            return GetObjectName(Model, ModelExpression);   
        }


        public static string GetRepositoryName(object Repository,
          [CallerArgumentExpression("Repository")] string RepositoryExpression = null)
        {
            return GetObjectName(Repository ,RepositoryExpression );
        }


        public static bool IsRepositoryType(Type type)
        {
            if (type == null)
                return false;

            // Controlla se il tipo è generico e corrisponde a Repository<T>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Repository<>))
                return true;

            // Controlla ricorsivamente la classe base
            if (type.BaseType != null)
                return IsRepositoryType(type.BaseType);

            return false;
        }


        public static bool IsViewModelType(Type type)
        {
            if (type == null)
                return false;

            // Controlla se il tipo è generico e corrisponde a ViewModel<T>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ViewModel<>))
                return true;

            // Controlla ricorsivamente la classe base
            if (type.BaseType != null)
                return IsViewModelType(type.BaseType);

            return false;
        }


        public static bool IsViewModelType(object obj)
        {
            if (obj == null)
                return false;

            return IsViewModelType(obj.GetType());
        }


        public static bool IsModelBaseType(object obj)
        {
            if (obj == null)
                return false;

            return IsModelBaseType(obj.GetType());
        }


        public static bool IsModelBaseType(Type type)
        {
            if (type == null)
                return false;

            // Controlla se il tipo corrisponde a ModelBase
            if (type == typeof(ModelBase))
                return true;

            // Controlla ricorsivamente la classe base
            if (type.BaseType != null)
                return IsModelBaseType(type.BaseType);

            return false;
        }


        public static void DisposeWisejSession()
        {
#pragma warning disable CS0168 // La variabile è dichiarata, ma non viene mai usata
            try
            {
            }

            catch (Exception ex)
            {
                throw;
            }
#pragma warning restore CS0168 // La variabile è dichiarata, ma non viene mai usata

        }


        public static bool ObjectPropertyExist(object Object, string Property)
        {
            if (Object is not null)
            {
                var type = Object.GetType();

                if (type.GetProperty(Property) is null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

        }


        public static string GetApplicationRuntimeInfo()
        {


            var wisejVersion = typeof(Wisej.Web.Application).Assembly
                .GetName()
                .Version
                ?.ToString() ?? "N/A";

            var sb = new StringBuilder();
            sb.Append($"Wisej.NET Version: {wisejVersion}\n");
            sb.Append($"OSArchitecture: {System.Runtime.InteropServices.RuntimeInformation.OSArchitecture}\n");
            sb.Append($"OSDescription: {System.Runtime.InteropServices.RuntimeInformation.OSDescription}\n");
            sb.Append($"ProcessArchitecture: {System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture}\n");
#if NET
            sb.Append($"RuntimeIdentifier: {System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier.ToString()}\n");
#endif
            sb.Append($"FrameworkDescription: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}\n");
            sb.Append($"Browser Device: {Application.Browser.Device}\n");
            sb.Append($"Browser Version: {Application.Browser.Version}\n");
            sb.Append($"Browser ScreenSize: {Application.Browser.ScreenSize.ToString()}\n");
            return sb.ToString();
        }


        public static void SaveByteArrayToFile(byte[] data, string filePath)
        {
            using (var writer = new BinaryWriter(File.OpenWrite(filePath)))
            {
                writer.Write(data);
            }
        }


        public static bool IsValidDateTime(DateTime DateTime)
        {

            if (DateTime.Date != new DateTime(1, 1, 1))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public static T GetParentOfType<T>(this Control control) where T : class
        {
            if (control?.Parent == null)
                return null;

            if (control.Parent is T parent)
                return parent;

            return GetParentOfType<T>(control.Parent);
        }


        public static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput))
            {
                return false;
            }

            strInput = strInput.Trim();

            if (strInput.StartsWith("{") && strInput.EndsWith("}") || strInput.StartsWith("[") && strInput.EndsWith("]"))
            {

                try
                {
                    var obj = Newtonsoft.Json.Linq.JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public static T Assign<T>(ref T target, T value)
        {
            target = value;
            return value;
        }




        /// <summary>
        /// Delays the process.
        /// </summary>
        /// <param name="Milliseconds">The milliseconds.</param>
        public static void DelayProcess(int Milliseconds)
        {
            Task.Run(async () => await _DelayProcess(Milliseconds)).Wait();
        }
        /// <summary>
        /// Delays the process.
        /// </summary>
        /// <param name="Milliseconds">The milliseconds.</param>
        public async static Task _DelayProcess(int Milliseconds)
        {
            // Wait for 5 seconds
            await Task.Delay(Milliseconds);
        }
        public static bool DesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }


        /// <summary>
        /// The get random static random generator
        /// </summary>
        private static Random GetRandom_staticRandomGenerator;

        /// <summary>
        /// Initializes the <see cref="Utilities"/> class.
        /// </summary>
        static Utilities()
        {
            GetRandom_staticRandomGenerator = new Random();
        }

        public static int GetRandom(int min, int max)
        {
            max += 1;
            return GetRandom_staticRandomGenerator.Next(min > max ? max : min, min > max ? min : max);
        }


        public static System.Drawing.Image SafeImageFromFile(string path)
        {

            byte[] bytesArr = null;
            bytesArr = File.ReadAllBytes(path);

            var memstr = new MemoryStream(bytesArr);
            return System.Drawing.Image.FromStream(memstr);


        }


        public static string NewGUID()
        {
            var guid = Guid.NewGuid();
            string upper = guid.ToString().Replace("-", "").ToUpper();
            return upper;
        }

    }
}
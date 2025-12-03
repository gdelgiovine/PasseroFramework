using FastDeepCloner;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
    public static class Utilities
    {


        public static string GetObjectName(object Object,
           [CallerArgumentExpression("Object")] string ObjectExpression = null)
        {

            string Name = string.Empty;

            // 1. Prova a ottenere dalla proprietà Name del ViewModel
            var nameProperty = Object.GetType().GetProperty("Name");
            if (nameProperty != null && nameProperty.PropertyType == typeof(string))
            {
                string _Name = (string)nameProperty.GetValue(Object) ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(_Name))
                {
                    Name = _Name;
                }
            }

            // 2. Se Name è vuoto, usa il nome della variabile catturato
            if (string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(ObjectExpression))
            {
                // Rimuove "this." se presente (es: "this.vmTitles" -> "vmTitles")
                Name = ObjectExpression.Replace("this.", "");
            }

            // 3. Fallback al nome del tipo
            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = Object.GetType().Name;
            }
            return Name;
          

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

        /// <summary>
        /// Verifica se il tipo è o deriva da Repository&lt;T&gt;.
        /// </summary>
        /// <param name="type">Il tipo da verificare.</param>
        /// <returns>True se il tipo è valido, altrimenti false.</returns>
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


        /// <summary>
        /// Verifica se il tipo è o deriva da ViewModel&lt;T&gt;.
        /// </summary>
        /// <param name="type">Il tipo da verificare.</param>
        /// <returns>True se il tipo è valido, altrimenti false.</returns>
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

        /// <summary>
        /// Verifica se l'oggetto è o deriva da ModelBase.
        /// </summary>
        /// <param name="obj">L'oggetto da verificare.</param>
        /// <returns>True se l'oggetto è valido, altrimenti false.</returns>
        public static bool IsViewModelType(object obj)
        {
            if (obj == null)
                return false;

            return IsViewModelType(obj.GetType());
        }

        /// <summary>
        /// Verifica se l'oggetto è o deriva da ModelBase.
        /// </summary>
        /// <param name="obj">L'oggetto da verificare.</param>
        /// <returns>True se l'oggetto è valido, altrimenti false.</returns>
        public static bool IsModelBaseType(object obj)
        {
            if (obj == null)
                return false;

            return IsModelBaseType(obj.GetType());
        }

        /// <summary>
        /// Verifica se il tipo è o deriva da ModelBase.
        /// </summary>
        /// <param name="type">Il tipo da verificare.</param>
        /// <returns>True se il tipo è valido, altrimenti false.</returns>
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


        /// <summary>
        /// Disposes the wisej session.
        /// </summary>
        [WebMethod]
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
        /// <summary>
        /// Objects the property exist.
        /// </summary>
        /// <param name="Object">The object.</param>
        /// <param name="Property">The property.</param>
        /// <returns></returns>
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



        /// <summary>
        /// Gets the application runtime information.
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationRuntimeInfo()
        {
            var sb = new StringBuilder();
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


        /// <summary>
        /// Saves the byte array to file.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="filePath">The file path.</param>
        public static void SaveByteArrayToFile(byte[] data, string filePath)
        {
            using (var writer = new BinaryWriter(File.OpenWrite(filePath)))
            {
                writer.Write(data);
            }
        }


        /// <summary>
        /// Determines whether [is valid date time] [the specified date time].
        /// </summary>
        /// <param name="DateTime">The date time.</param>
        /// <returns>
        ///   <c>true</c> if [is valid date time] [the specified date time]; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Gets the type of the parent of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        public static T GetParentOfType<T>(this Control control) where T : class
        {
            if (control?.Parent == null)
                return null;

            if (control.Parent is T parent)
                return parent;

            return GetParentOfType<T>(control.Parent);
        }

        /// <summary>
        /// Determines whether [is valid json] [the specified string input].
        /// </summary>
        /// <param name="strInput">The string input.</param>
        /// <returns>
        ///   <c>true</c> if [is valid json] [the specified string input]; otherwise, <c>false</c>.
        /// </returns>
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
        /// <summary>
        /// Assigns the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
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
        

   


        /// <summary>
        /// Clones the specified source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static T WisejClone<T>(T source)
        {
            if (ReferenceEquals(source, null))
                return default;

            T newObject = Conversions.ToGenericParameter<T>(Activator.CreateInstance(source.GetType()));
            newObject = Wisej.Core.WisejSerializer .Parse(Wisej.Core.WisejSerializer.Serialize(source));
            return newObject;
        }



        /// <summary>
        /// Clones the specified source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// 

        public static T Clone<T>(T source)
        {
            if (ReferenceEquals(source, null))
                return default;

            // Usa FastDeepCloner per creare una copia profonda
            return (T)FastDeepCloner.DeepCloner.Clone(source);
        }
        /// <summary>
        /// Clones the specified source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// 
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

        /// <summary>
        /// Objectses the equals.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="another">Another.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Designs the mode.
        /// </summary>
        /// <returns></returns>
        public static bool DesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }




        /// <summary>
        /// Gets the random.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public static int GetRandom(int min, int max)
        {
            max += 1;
            return GetRandom_staticRandomGenerator.Next(min > max ? max : min, min > max ? min : max);
        }


        /// <summary>
        /// Gets the true false data table for ComboBox.
        /// </summary>
        /// <param name="TrueDescription">The true description.</param>
        /// <param name="FalseDescription">The false description.</param>
        /// <returns></returns>
        public static DataTable GetTrueFalseDataTableForComboBox(string TrueDescription = "True", string FalseDescription = "False")
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(bool));
            table.Columns.Add("Desc", typeof(string));
            table.Rows.Add(new object[] { false, FalseDescription });
            table.Rows.Add(new object[] { true, TrueDescription });
            return table;
        }

        /// <summary>
        /// Gets the yes no data table for ComboBox.
        /// </summary>
        /// <param name="YesDescription">The yes description.</param>
        /// <param name="NoDescription">The no description.</param>
        /// <returns></returns>
        public static DataTable GetYesNoDataTableForComboBox(string YesDescription = "Yes", string NoDescription = "No")
        {
            return GetTrueFalseDataTableForComboBox(YesDescription, NoDescription);
        }


        /// <summary>
        /// Safes the image from file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static System.Drawing.Image SafeImageFromFile(string path)
        {

            byte[] bytesArr = null;
            bytesArr = File.ReadAllBytes(path);

            var memstr = new MemoryStream(bytesArr);
            return System.Drawing.Image.FromStream(memstr);


        }

        /// <summary>
        /// Creates new guid.
        /// </summary>
        /// <returns></returns>
        public static string NewGUID()
        {
            var guid = Guid.NewGuid();
            string upper = guid.ToString().Replace("-", "").ToUpper();
            return upper;
        }

        /// <summary>
        /// Reflections the get fields.
        /// </summary>
        /// <param name="t">The t.</param>
        public static void Reflection_GetFields(Type t)
        {
            Console.WriteLine("***** Fields *****");
            FieldInfo[] fields = t.GetFields();
            for (int i = 0, loopTo = fields.Length - 1; i <= loopTo; i++)
                Console.WriteLine("->{0}", fields[i].Name);
            Console.WriteLine("");
        }

        /// <summary>
        /// Reflections the get properties.
        /// </summary>
        /// <param name="t">The t.</param>
        public static void Reflection_GetProperties(Type t)
        {
            Console.WriteLine("***** Properties *****");
            PropertyInfo[] properties = t.GetProperties();
            for (int i = 0, loopTo = properties.Length - 1; i <= loopTo; i++)
                Console.WriteLine("->{0}", properties[i].Name);
            Console.WriteLine("");
        }

        /// <summary>
        /// Reflections the get valid filename.
        /// </summary>
        /// <param name="Filename">The filename.</param>
        /// <returns></returns>
        public static string Reflection_GetValidFilename(string Filename)
        {
            string regex = string.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars())));
            var removeInvalidChars = new Regex(regex, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
            return regex;
        }


        /// <summary>
        /// Determines whether the specified expression is numeric.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        ///   <c>true</c> if the specified expression is numeric; otherwise, <c>false</c>.
        /// </returns>
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


        /// <summary>
        /// Determines whether [is numeric type] [the specified type].
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is numeric type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Determines whether [is list type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is list type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
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
        /// <summary>
        /// Determines whether [is string type] [the specified type].
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is string type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Determines whether [is date time type] [the specified type].
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is date time type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
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
        /// <summary>
        /// Determines whether [is boolean type] [the specified type].
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is boolean type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
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
        /// <summary>
        /// Determines whether [is object type] [the specified type].
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is object type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Determines whether [is empty type] [the specified type].
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [is empty type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
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

        /// <summary>
        /// Gets the system type is.
        /// </summary>
        /// <param name="Type">The type.</param>
        /// <returns></returns>
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

        //public enum EnumToDataTableDisplayItem
        //{
        //    Name,
        //    Value,
        //    Description,
        //    ValueName,
        //    ValueDescription,
        //    NameDescription

        //}
        //public class EnumItem
        //{
        //    public string Name = "";
        //    public string Value = "";
        //    public string Description = "";
        //    public EnumItem()
        //    {
        //    }
        //}



        //public enum SystemTypeIs
        //{
        //    Empty=0,
        //    String=1,
        //    Numeric=2,
        //    DateTime=3,
        //    Boolean=4,
        //    Object=5
        //}
        //public enum EnumToDataTableValueItem
        //{
        //    Name,
        //    Value


        //}


        /// <summary>
        /// Databases the type of the type to.
        /// </summary>
        /// <param name="dbType">Type of the database.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts the type of the string to.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidCastException"></exception>
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

        /// <summary>
        /// Gets the type of the database.
        /// </summary>
        /// <param name="runtimeType">Type of the runtime.</param>
        /// <returns></returns>
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



        /// <summary>
        /// Gets the type of the database.
        /// </summary>
        /// <param name="runtimeType">Type of the runtime.</param>
        /// <returns></returns>
        /// 
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

        // Public Function __Assign(Of T)(ByRef target As T, value As T) As T
        // target = value
        // Return value
        // End Function



    }
}
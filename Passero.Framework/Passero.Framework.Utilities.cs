using FastDeepCloner;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
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

            var templateValue = (Object)null;
            if (runtimeType.IsClass == false)
            {
                templateValue = Activator.CreateInstance(runtimeType);
            }

            var sqlParamter = new SqlParameter(parameterName: String.Empty, value: templateValue);

            return sqlParamter.DbType;
        }

        // Public Function __Assign(Of T)(ByRef target As T, value As T) As T
        // target = value
        // Return value
        // End Function



    }
}
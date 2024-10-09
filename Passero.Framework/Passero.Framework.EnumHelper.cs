using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Passero.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the localized description.
        /// </summary>
        /// <param name="enum">The enum.</param>
        /// <returns></returns>
        public static string GetLocalizedDescription(this Enum @enum)
        {
            if (@enum == null)
                return null;

            string description = @enum.ToString();

            FieldInfo fieldInfo = @enum.GetType().GetField(description);
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Any())
                return attributes[0].Description;

            return description;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ComponentModel.DescriptionAttribute" />
    public class LocalizedEnumAttribute : DescriptionAttribute
    {
        /// <summary>
        /// The name property
        /// </summary>
        private PropertyInfo _nameProperty;
        /// <summary>
        /// The resource type
        /// </summary>
        private Type _resourceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedEnumAttribute"/> class.
        /// </summary>
        /// <param name="displayNameKey">The display name key.</param>
        /// <param name="Resource">The resource.</param>
        public LocalizedEnumAttribute(string displayNameKey, Type Resource)
            : base(displayNameKey)
        {
            NameResourceType = Resource;
        }

        /// <summary>
        /// Gets or sets the type of the name resource.
        /// </summary>
        /// <value>
        /// The type of the name resource.
        /// </value>
        public Type NameResourceType
        {
            get
            {
                return _resourceType;
            }
            set
            {
                _resourceType = value;

                _nameProperty = _resourceType.GetProperty(Description, BindingFlags.Static | BindingFlags.Public);
            }
        }

        /// <summary>
        /// Ottiene la descrizione memorizzata in questo attributo.
        /// </summary>
        public override string Description
        {
            get
            {
                //check if nameProperty is null and return original display name value
                if (_nameProperty == null)
                {
                    return base.Description;
                }

                return (string)_nameProperty.GetValue(_nameProperty.DeclaringType, null);
                //return this.GetLocalizedDescription();
            }
        }

        //public string xGetLocalizedDescription()
        //{
        //    //FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());
        //    FieldInfo fi = this.GetType().GetField(this.ToString());

        //    DescriptionAttribute[] attributes =
        //        (DescriptionAttribute[])fi.GetCustomAttributes(
        //        typeof(DescriptionAttribute),
        //        false);

        //    if (attributes != null &&
        //        attributes.Length > 0)
        //        return attributes[0].Description;
        //    else
        //        return this.ToString();
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    public static class EnumHelper
    {

        /// <summary>
        /// Enums the description.
        /// </summary>
        /// <param name="EnumConstant">The enum constant.</param>
        /// <returns></returns>
        public static string EnumDescription(Enum EnumConstant)
        {
            string str;
            var fi = EnumConstant.GetType().GetField(EnumConstant.ToString());
            DescriptionAttribute[] aattr = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            str = aattr.Length == 0 ? EnumConstant.ToString() : aattr[0].Description;
            return str;
        }



        /// <summary>
        /// Enums to data table.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="valueitem">The valueitem.</param>
        /// <param name="displayitem">The displayitem.</param>
        /// <returns></returns>
        public static DataTable EnumToDataTable(Type enumType, Passero.Framework.EnumToDataTableValueItem valueitem, Passero.Framework.EnumToDataTableDisplayItem displayitem)
        {
            string ID;
            var table = new DataTable();
            table.Columns.Add("Id", typeof(string));
            table.Columns.Add("Desc", typeof(string));
            var enumItem = new Passero.Framework.EnumItem();
            var enumItems = new Dictionary<int, Passero.Framework.EnumItem>();
            var values = new Dictionary<int, int>();
            int index = 0;
            string[] names = Enum.GetNames(enumType);
            for (int num = 0, loopTo = names.Length - 1; num <= loopTo; num++)
            {
                enumItem = new Passero.Framework.EnumItem() { Name = names[num] };
                enumItems.Add(index, enumItem);
                index += 1;
            }
            index = 0;
            foreach (int i in Enum.GetValues(enumType))
            {
                enumItems[index].Value = i.ToString();
                values.Add(index, i);
                index += 1;
            }
            FieldInfo[] info = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            for (int i = 0, loopTo1 = info.Length - 1; i <= loopTo1; i++)
            {
                enumItem = enumItems[i];
                foreach (var item in info[i].CustomAttributes)
                {
                    if (Equals(item.AttributeType.Name, "DescriptionAttribute"))
                    {
                        var customAttributeTypedArgument = item.ConstructorArguments[0];
                        enumItem.Description = customAttributeTypedArgument.ToString().Replace("\"", "");
                    }

                    if (Equals(item.AttributeType.Name, "DefaultValueAttribute"))
                    {
                        var customAttributeTypedArgument = item.ConstructorArguments[0];
                        enumItem.Value = customAttributeTypedArgument.ToString().Replace("\"", "");
                    }
                }
            }
            for (int i = 0, loopTo2 = enumItems.Count - 1; i <= loopTo2; i++)
            {
                enumItem = enumItems[i];
                string DESCRIPTION = "";
                ID = valueitem != Passero.Framework.EnumToDataTableValueItem.Name ? enumItem.Value : enumItem.Name;
                switch (displayitem)
                {
                    case Passero.Framework.EnumToDataTableDisplayItem.Name:
                        {
                            DESCRIPTION = enumItem.Name;
                            break;
                        }
                    case Passero.Framework.EnumToDataTableDisplayItem.Value:
                        {

                            break;
                        }
                    case Passero.Framework.EnumToDataTableDisplayItem.Description:
                        {
                            DESCRIPTION = enumItem.Description;
                            break;
                        }
                    case Passero.Framework.EnumToDataTableDisplayItem.ValueName:
                        {
                            DESCRIPTION = string.Concat(ID, "- ", enumItem.Name);
                            break;
                        }
                    case Passero.Framework.EnumToDataTableDisplayItem.ValueDescription:
                        {
                            DESCRIPTION = string.Concat(ID, "- ", enumItem.Description);
                            break;
                        }

                    default:
                        {

                            break;
                        }
                }
                table.Rows.Add(new object[] { ID, DESCRIPTION });
            }
            return table;
        }


        /// <summary>
        /// Enums to data table.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="valueitem">The valueitem.</param>
        /// <param name="displayitem">The displayitem.</param>
        /// <returns></returns>
        public static DataTable EnumToDataTable(Type enumType, Passero.Framework.EnumToDataTableDisplayItem valueitem, Passero.Framework.EnumToDataTableDisplayItem displayitem)
        {
            string ID;
            var table = new DataTable();
            table.Columns.Add("Id", typeof(string));
            table.Columns.Add("Desc", typeof(string));
            var enumItem = new Passero.Framework.EnumItem();
            var enumItems = new Dictionary<int, Passero.Framework.EnumItem>();
            var values = new Dictionary<int, int>();
            int index = 0;
            string[] names = Enum.GetNames(enumType);
            for (int num = 0, loopTo = names.Length - 1; num <= loopTo; num++)
            {
                enumItem = new Passero.Framework.EnumItem() { Name = names[num] };
                enumItems.Add(index, enumItem);
                index += 1;
            }
            index = 0;
            foreach (int i in Enum.GetValues(enumType))
            {
                enumItems[index].Value = i.ToString();
                values.Add(index, i);
                index += 1;
            }
            FieldInfo[] info = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            for (int i = 0, loopTo1 = info.Length - 1; i <= loopTo1; i++)
            {
                enumItem = enumItems[i];
                foreach (var item in info[i].CustomAttributes)
                {
                    if (Equals(item.AttributeType.Name, "DescriptionAttribute"))
                    {
                        var customAttributeTypedArgument = item.ConstructorArguments[0];
                        enumItem.Description = customAttributeTypedArgument.ToString().Replace("\"", "");
                    }
                    if (Equals(item.AttributeType.Name, "DefaultValueAttribute"))
                    {
                        var customAttributeTypedArgument = item.ConstructorArguments[0];
                        enumItem.Value = customAttributeTypedArgument.ToString().Replace("\"", "");
                    }

                }
            }
            for (int i = 0, loopTo2 = enumItems.Count - 1; i <= loopTo2; i++)
            {
                enumItem = enumItems[i];
                string DESCRIPTION = "";
                ID = valueitem != Passero.Framework.EnumToDataTableDisplayItem.Name ? enumItem.Name : enumItem.Name;
                switch (displayitem)
                {
                    case Passero.Framework.EnumToDataTableDisplayItem.Name:
                        {
                            DESCRIPTION = enumItem.Name;
                            break;
                        }
                    case Passero.Framework.EnumToDataTableDisplayItem.Value:
                        {

                            break;
                        }
                    case Passero.Framework.EnumToDataTableDisplayItem.Description:
                        {
                            DESCRIPTION = enumItem.Description;
                            break;
                        }
                    case Passero.Framework.EnumToDataTableDisplayItem.ValueName:
                        {
                            DESCRIPTION = string.Concat(ID, "- ", enumItem.Name);
                            break;
                        }
                    case Passero.Framework.EnumToDataTableDisplayItem.ValueDescription:
                        {
                            DESCRIPTION = string.Concat(ID, "- ", enumItem.Description);
                            break;
                        }
                    case Passero.Framework.EnumToDataTableDisplayItem.NameDescription:
                        {
                            DESCRIPTION = string.Concat(enumItem.Name, "- ", enumItem.Description);
                            break;
                        }

                    default:
                        {

                            break;
                        }
                }
                table.Rows.Add(new object[] { ID, DESCRIPTION });
            }
            return table;
        }


        /// <summary>
        /// is the enumerable to data table.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static DataTable IEnumerableToDataTable<TSource>(IEnumerable<TSource> source)
        {
            PropertyInfo[] props = typeof(TSource).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            source.ToList().ForEach(i => dt.Rows.Add(props.Select(p => p.GetValue(i, null)).ToArray()));
            return dt;
        }
        /// <summary>
        /// is the list to data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static DataTable IListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }



        /// <summary>
        /// is the enumerable to binding list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static BindingList<T> IEnumerableToBindingList<T>(IEnumerable<T> data)
        {
            BindingList<T> result = null;
            if (data is not null)
            {
                result = new BindingList<T>();
                foreach (T item in data)
                    result.Add(item);
            }
            return result;
        }



        /// <summary>
        /// Binds the enum int value to ComboBox.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="defaultSelection">The default selection.</param>
        /// <param name="ResourceFileName">Name of the resource file.</param>
        public static void BindEnumIntValueToComboBox<T>(ref Wisej.Web.ComboBox comboBox, object defaultSelection = null, string ResourceFileName = "Resources")
        {
            BindEnumToComboBox<T>(ref comboBox, true, defaultSelection, ResourceFileName);
        }
        /// <summary>
        /// Binds the enum value to ComboBox.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="defaultSelection">The default selection.</param>
        /// <param name="ResourceFileName">Name of the resource file.</param>
        public static void BindEnumValueToComboBox<T>(ref Wisej.Web.ComboBox comboBox, object defaultSelection = null, string ResourceFileName = "Resources")
        {
            BindEnumToComboBox<T>(ref comboBox, false, defaultSelection, ResourceFileName);
        }


        /// <summary>
        /// Binds the enum to ComboBox.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="UseIntValue">if set to <c>true</c> [use int value].</param>
        /// <param name="defaultSelection">The default selection.</param>
        /// <param name="ResourceFileName">Name of the resource file.</param>
        public static void BindEnumToComboBox<T>(ref Wisej.Web.ComboBox comboBox, bool UseIntValue = false, object defaultSelection = null, string ResourceFileName = "Resources")
        {

            object typevalue = defaultSelection;
            Type type = typeof(T);
            System.Collections.IList list;
            var assembly = Assembly.GetAssembly(type);
            string resourceSource = assembly.GetName().Name + "." + ResourceFileName;
            ResourceManager rm = new ResourceManager(resourceSource, assembly);
            rm.IgnoreCase = true;

            try
            {
                list = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(value => new
                {
                    Description = rm.GetString(type.Name + "_" + value.ToString()),
                    Value = value

                })
                //.OrderBy(item => item.Value.ToString())
                .ToList();
            }

            catch (Exception)
            {
                list = Enum.GetValues(typeof(T))
               .Cast<T>()
               .Select(value => new
               {
                   Description = (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description ?? value.ToString(),
                   Value = value

               })
                 //.OrderBy(item => item.Value.ToString())
                 .ToList();
            }

            if (ReflectionHelper.GetPropertyValue(list[0], "Description") == null)
            {
                list = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(value => new
                {
                    Description = (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description ?? value.ToString(),
                    Value = value

                })
                //.OrderBy(item => item.Value.ToString())
                .ToList();
            }

            var table = new DataTable();
            table.Columns.Add("Value", typeof(int));
            table.Columns.Add("EnumValue", typeof(string));
            table.Columns.Add("Description", typeof(string));
            for (int i = 0; i < list.Count; i++)
            {
                object item = list[i];

                int value = (int)ReflectionHelper.GetPropertyValue(item, "Value");
                string enumvalue = ReflectionHelper.GetPropertyValue(item, "Value").ToString();
                string description = ReflectionHelper.GetPropertyValue(item, "Description").ToString();
                table.Rows.Add(new object[] { value, enumvalue, description });
            }

            //List<EnumItem<T>> enumItems = new List<EnumItem<T>>();    
            //for (int i = 0; i < list.Count; i++)
            //{
            //    EnumItem<T> enumItem = new EnumItem<T>  ();

            //    object item = list[i];
            //    enumItem.Value = (int)ReflectionHelper.GetPropertyValue(item, "Value");
            //    enumItem.EnumValue  = (T)ReflectionHelper.GetPropertyValue(item, "Value");
            //    enumItem.Description = ReflectionHelper.GetPropertyValue(item, "Description").ToString();
            //    enumItems .Add(enumItem);
            //}




            // comboBox.DataSource = list;
            //comboBox.DataSource = enumItems ;
            comboBox.DataSource = table;
            comboBox.DisplayMember = "Description";
            if (UseIntValue)
                comboBox.ValueMember = "Value";
            else
                comboBox.ValueMember = "EnumValue";


            if (defaultSelection != null)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (UseIntValue)
                    {
                        if (table.Rows[i]["Value"].ToString() == defaultSelection.ToString())
                        {
                            comboBox.SelectedItem = defaultSelection.ToString();
                            break;
                        }
                    }
                    else
                    {
                        if (table.Rows[i]["EnumValue"].ToString() == defaultSelection.ToString())
                        {
                            comboBox.SelectedItem = defaultSelection.ToString();
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Binds the enum int value to data grid view ComboBox column.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="defaultSelection">The default selection.</param>
        /// <param name="ResourceFileName">Name of the resource file.</param>
        public static void BindEnumIntValueToDataGridViewComboBoxColumn<T>(ref Wisej.Web.DataGridViewComboBoxColumn comboBox, object defaultSelection = null, string ResourceFileName = "Resources")
        {
            BindEnumToDataGridViewComboBoxColumn<T>(ref comboBox, true, defaultSelection, ResourceFileName);
        }
        /// <summary>
        /// Binds the enum value to data grid view ComboBox column.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="defaultSelection">The default selection.</param>
        /// <param name="ResourceFileName">Name of the resource file.</param>
        public static void BindEnumValueToDataGridViewComboBoxColumn<T>(ref Wisej.Web.DataGridViewComboBoxColumn comboBox, object defaultSelection = null, string ResourceFileName = "Resources")
        {
            BindEnumToDataGridViewComboBoxColumn<T>(ref comboBox, false, defaultSelection, ResourceFileName);
        }

        /// <summary>
        /// Binds the enum to data grid view ComboBox column.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="UseIntValue">if set to <c>true</c> [use int value].</param>
        /// <param name="defaultSelection">The default selection.</param>
        /// <param name="ResourceFileName">Name of the resource file.</param>
        public static void BindEnumToDataGridViewComboBoxColumn<T>(ref Wisej.Web.DataGridViewComboBoxColumn comboBox, bool UseIntValue = false, object defaultSelection = null, string ResourceFileName = "Resources")
        {

            object typevalue = defaultSelection;
            Type type = typeof(T);
            System.Collections.IList list;
            var assembly = Assembly.GetAssembly(type);
            string resourceSource = assembly.GetName().Name + "." + ResourceFileName;
            ResourceManager rm = new ResourceManager(resourceSource, assembly);
            rm.IgnoreCase = true;

            try
            {
                list = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(value => new
                {
                    Description = rm.GetString(type.Name + "_" + value.ToString()),
                    Value = value

                })
                //.OrderBy(item => item.Value.ToString())
                .ToList();
            }

            catch (Exception)
            {
                list = Enum.GetValues(typeof(T))
               .Cast<T>()
               .Select(value => new
               {
                   Description = (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description ?? value.ToString(),
                   Value = value

               })
                 //.OrderBy(item => item.Value.ToString())
                 .ToList();
            }

            if (ReflectionHelper.GetPropertyValue(list[0], "Description") == null)
            {
                list = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(value => new
                {
                    Description = (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description ?? value.ToString(),
                    Value = value

                })
                //.OrderBy(item => item.Value.ToString())
                .ToList();
            }

            var table = new DataTable();
            table.Columns.Add("Value", typeof(int));
            table.Columns.Add("EnumValue", typeof(string));
            table.Columns.Add("Description", typeof(string));
            for (int i = 0; i < list.Count; i++)
            {
                object item = list[i];

                int value = (int)ReflectionHelper.GetPropertyValue(item, "Value");
                string enumvalue = ReflectionHelper.GetPropertyValue(item, "Value").ToString();
                string description = ReflectionHelper.GetPropertyValue(item, "Description").ToString();
                table.Rows.Add(new object[] { value, enumvalue, description });
            }

            comboBox.DataSource = table;
            comboBox.DisplayMember = "Description";
            if (UseIntValue)
                comboBox.ValueMember = "Value";
            else
                comboBox.ValueMember = "EnumValue";


            if (defaultSelection != null)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (UseIntValue)
                    {
                        if (table.Rows[i]["Value"].ToString() == defaultSelection.ToString())
                        {
                            //comboBox.SelectedItem = defaultSelection.ToString();
                            break;
                        }
                    }
                    else
                    {
                        if (table.Rows[i]["EnumValue"].ToString() == defaultSelection.ToString())
                        {
                            //comboBox.SelectedItem = defaultSelection.ToString();
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class EnumItem<T>
        {
            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public int Value { get; set; }
            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            /// <value>
            /// The description.
            /// </value>
            public string Description { get; set; }
            /// <summary>
            /// Gets or sets the enum value.
            /// </summary>
            /// <value>
            /// The enum value.
            /// </value>
            public T EnumValue { get; set; }
        }

    }
}
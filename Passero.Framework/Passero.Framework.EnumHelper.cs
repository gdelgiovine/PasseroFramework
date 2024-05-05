using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{
    public static class EnumHelper
    {

        public static string EnumDescription(Enum EnumConstant)
        {
            string str;
            var fi = EnumConstant.GetType().GetField(EnumConstant.ToString());
            /* TODO ERROR: Skipped WarningDirectiveTrivia
            #Disable Warning BC42016 ' Conversione implicita
            */
            DescriptionAttribute[] aattr = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            /* TODO ERROR: Skipped WarningDirectiveTrivia
            #Enable Warning BC42016 ' Conversione implicita
            */
            str = aattr.Length == 0 ? EnumConstant.ToString() : aattr[0].Description;
            return str;
        }



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


        public static DataTable IEnumerableToDataTable<TSource>(IEnumerable<TSource> source)
        {
            PropertyInfo[] props = typeof(TSource).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            source.ToList().ForEach(i => dt.Rows.Add(props.Select(p => p.GetValue(i, null)).ToArray()));
            return dt;
        }


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
    }
}

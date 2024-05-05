using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{
    public enum DataBindingMode
    {
        None = 0,
        Passero = 2,
        BindingSource = 1
    }
    public enum BindingBehaviour
    {
        InheritFromViewModel = 0,
        Select = 1,
        Update = 2,
        Insert = 4,
        SelectUpdate = 3,
        SelectInsertUpdate = 7,
        InsertUpdate = 6
    }

    public enum UseModelData
    {
        External = 1,
        InternalRepository = 0
    }

    public enum EnumToDataTableDisplayItem
    {
        Name,
        Value,
        Description,
        ValueName,
        ValueDescription,
        NameDescription

    }
    public class EnumItem
    {
        public string Name = "";
        public string Value = "";
        public string Description = "";
        public EnumItem()
        {
        }
    }



    public enum EnumSystemTypeIs
    {
        Empty = 0,
        String = 1,
        Numeric = 2,
        DateTime = 3,
        Boolean = 4,
        Object = 5
    }
    public enum EnumToDataTableValueItem
    {
        Name,
        Value


    }
    [Serializable]
    public enum ErrorNotificationModes
    {
        ThrowException = 0,
        ShowDialog = 1,
        Silent = 2
    }
    [Serializable]
    public enum DbObjectType
    {
        Table = 0,
        View = 1,
        StoredProcedure = 2,
        TableFunction = 3,
        ScalarFunction = 4,
        Sequence = 5
    }
}

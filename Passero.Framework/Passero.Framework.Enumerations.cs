using System;

namespace Passero.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public enum DataBindingMode
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,
        /// <summary>
        /// The passero
        /// </summary>
        Passero = 2,
        /// <summary>
        /// The binding source
        /// </summary>
        BindingSource = 1
    }
    /// <summary>
    /// 
    /// </summary>
    public enum BindingBehaviour
    {
        /// <summary>
        /// The inherit from view model
        /// </summary>
        InheritFromViewModel = 0,
        /// <summary>
        /// The select
        /// </summary>
        Select = 1,
        /// <summary>
        /// The update
        /// </summary>
        Update = 2,
        /// <summary>
        /// The insert
        /// </summary>
        Insert = 4,
        /// <summary>
        /// The select update
        /// </summary>
        SelectUpdate = 3,
        /// <summary>
        /// The select insert update
        /// </summary>
        SelectInsertUpdate = 7,
        /// <summary>
        /// The insert update
        /// </summary>
        InsertUpdate = 6
    }

    /// <summary>
    /// 
    /// </summary>
    public enum UseModelData
    {
        /// <summary>
        /// The external
        /// </summary>
        External = 1,
        /// <summary>
        /// The internal repository
        /// </summary>
        InternalRepository = 0
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EnumToDataTableDisplayItem
    {
        /// <summary>
        /// The name
        /// </summary>
        Name,
        /// <summary>
        /// The value
        /// </summary>
        Value,
        /// <summary>
        /// The description
        /// </summary>
        Description,
        /// <summary>
        /// The value name
        /// </summary>
        ValueName,
        /// <summary>
        /// The value description
        /// </summary>
        ValueDescription,
        /// <summary>
        /// The name description
        /// </summary>
        NameDescription

    }
    /// <summary>
    /// 
    /// </summary>
    public class EnumItem
    {
        /// <summary>
        /// The name
        /// </summary>
        public string Name = "";
        /// <summary>
        /// The value
        /// </summary>
        public string Value = "";
        /// <summary>
        /// The description
        /// </summary>
        public string Description = "";
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumItem"/> class.
        /// </summary>
        public EnumItem()
        {
        }
    }



    /// <summary>
    /// 
    /// </summary>
    public enum EnumSystemTypeIs
    {
        /// <summary>
        /// The empty
        /// </summary>
        Empty = 0,
        /// <summary>
        /// The string
        /// </summary>
        String = 1,
        /// <summary>
        /// The numeric
        /// </summary>
        Numeric = 2,
        /// <summary>
        /// The date time
        /// </summary>
        DateTime = 3,
        /// <summary>
        /// The boolean
        /// </summary>
        Boolean = 4,
        /// <summary>
        /// The object
        /// </summary>
        Object = 5
    }
    /// <summary>
    /// 
    /// </summary>
    public enum EnumToDataTableValueItem
    {
        /// <summary>
        /// The name
        /// </summary>
        Name,
        /// <summary>
        /// The value
        /// </summary>
        Value


    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum ErrorNotificationModes
    {
        /// <summary>
        /// The throw exception
        /// </summary>
        ThrowException = 0,
        /// <summary>
        /// The show dialog
        /// </summary>
        ShowDialog = 1,
        /// <summary>
        /// The silent
        /// </summary>
        Silent = 2
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum DbObjectType
    {
        /// <summary>
        /// The table
        /// </summary>
        Table = 0,
        /// <summary>
        /// The view
        /// </summary>
        View = 1,
        /// <summary>
        /// The stored procedure
        /// </summary>
        StoredProcedure = 2,
        /// <summary>
        /// The table function
        /// </summary>
        TableFunction = 3,
        /// <summary>
        /// The scalar function
        /// </summary>
        ScalarFunction = 4,
        /// <summary>
        /// The sequence
        /// </summary>
        Sequence = 5
    }
}

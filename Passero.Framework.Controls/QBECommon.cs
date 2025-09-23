using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    //internal class CustomComponentResourceManager : ComponentResourceManager
    //{
    //    public CustomComponentResourceManager(Type type, string resourceName)
    //       : base(type)
    //    {
    //        this.BaseNameField = resourceName;
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    public enum ViewModelGridModes
    {
        /// <summary>
        /// The no grid mode
        /// </summary>
        NoGridMode = 0,
        /// <summary>
        /// The data grid view
        /// </summary>
        DataGridView = 1,
        /// <summary>
        /// The data repeater
        /// </summary>
        DataRepeater = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataNavigatorViewModel
    {
        public ViewModelTypes ViewModelType { get; set; } = ViewModelTypes.Base;  
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public dynamic ViewModel { get; set; }
        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        public string FriendlyName { get; set; }
        /// <summary>
        /// Gets or sets the data grid view.
        /// </summary>
        /// <value>
        /// The data grid view.
        /// </value>
        public DataGridView DataGridView { get; set; }


        /// <summary>
        /// Gets or sets the data repeater.
        /// </summary>
        /// <value>
        /// The data repeater.
        /// </value>
        public DataRepeater DataRepeater { get; set; }
        /// <summary>
        /// The m grid mode
        /// </summary>
        private ViewModelGridModes mGridMode = ViewModelGridModes.NoGridMode;
        /// <summary>
        /// Gets or sets the grid mode.
        /// </summary>
        /// <value>
        /// The grid mode.
        /// </value>
        public ViewModelGridModes GridMode
        {
            get { return mGridMode; }
            set { mGridMode = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DataNavigatorViewModel"/> class.
        /// </summary>
        /// <param name="ViewModel">The view model.</param>
        /// <param name="Name">The name.</param>
        /// <param name="FriendlyName">Name of the friendly.</param>
        /// <param name="DataGridView">The data grid view.</param>
        /// <param name="DataRepeater">The data repeater.</param>
        public DataNavigatorViewModel(object ViewModel, string Name = "", string FriendlyName = "", DataGridView DataGridView = null, DataRepeater DataRepeater = null)
        {
            if (string.IsNullOrEmpty(Name))
                Name = ReflectionHelper.GetPropertyValue(ViewModel, "Name").ToString();

            if (string.IsNullOrEmpty(FriendlyName))
                FriendlyName = ReflectionHelper.GetPropertyValue(ViewModel, "FriendlyName").ToString();

            this.Name = Name;
            this.FriendlyName = FriendlyName;
            this.ViewModel = ViewModel;
            this.ViewModelType = (ViewModelTypes)ReflectionHelper.CallByName(ViewModel, "ViewModelType", CallType.Get);
            this.DataGridView = DataGridView;

            if (this.DataRepeater != null)
            {
                GridMode = ViewModelGridModes.DataRepeater;
            }
            if (this.DataGridView != null)
            {
                GridMode = ViewModelGridModes.DataGridView;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ModelPropertyMapping
    {
        /// <summary>
        /// Gets or sets the qbe model property.
        /// </summary>
        /// <value>
        /// The qbe model property.
        /// </value>
        public string QBEModelProperty { get; set; }
        /// <summary>
        /// Gets or sets the target model property.
        /// </summary>
        /// <value>
        /// The target model property.
        /// </value>
        public string TargetModelProperty { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPropertyMapping"/> class.
        /// </summary>
        public ModelPropertyMapping()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPropertyMapping"/> class.
        /// </summary>
        /// <param name="QBEModelProperty">The qbe model property.</param>
        /// <param name="TargetModelProperty">The target model property.</param>
        public ModelPropertyMapping(string QBEModelProperty, string TargetModelProperty)
        {
            this.TargetModelProperty = TargetModelProperty.Trim();
            this.QBEModelProperty = QBEModelProperty.Trim();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Collections.CollectionBase" />
    public class ModelPropertiesMapping : CollectionBase
    {

        /// <summary>
        /// Adds the specified qbe model property.
        /// </summary>
        /// <param name="QBEModelProperty">The qbe model property.</param>
        /// <param name="TargetModelProperty">The target model property.</param>
        /// <returns></returns>
        public ModelPropertyMapping Add(string QBEModelProperty, string TargetModelProperty)
        {
            var x = new ModelPropertyMapping();
            x.QBEModelProperty = QBEModelProperty;
            x.TargetModelProperty = TargetModelProperty;
            List.Add(x);
            return x;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <returns></returns>
        public ModelPropertyMapping get_Item(int Index)
        {
            ModelPropertyMapping ItemRet = default;
            ItemRet = (ModelPropertyMapping)List[Index];
            return ItemRet;
        }

        /// <summary>
        /// Sets the item.
        /// </summary>
        /// <param name="Index">The index.</param>
        /// <param name="value">The value.</param>
        public void set_Item(int Index, ModelPropertyMapping value)
        {
            List[Index] = value;
        }


    }
    // Stub Class QBEForm for Resource loading for QBEForm<ModelClass>
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Wisej.Web.Form" />
    public class QBEForm : Wisej.Web.Form
    {
    }


    #region QBEBoundControl Class
    /// <summary>
    /// 
    /// </summary>
    public class QBEBoundControl
    {
        /// <summary>
        /// The m control
        /// </summary>
        private object mControl;
        /// <summary>
        /// The m model property name
        /// </summary>
        private string mModelPropertyName;
        /// <summary>
        /// The m control property name
        /// </summary>
        private string mControlPropertyName;

        /// <summary>
        /// Gets or sets the name of the control property.
        /// </summary>
        /// <value>
        /// The name of the control property.
        /// </value>
        public string ControlPropertyName
        {
            get
            {
                string PropertyNameRet = default;
                PropertyNameRet = mControlPropertyName;
                return PropertyNameRet;

            }
            set
            {
                mControlPropertyName = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the model property.
        /// </summary>
        /// <value>
        /// The name of the model property.
        /// </value>
        public string ModelPropertyName
        {
            get
            {

                return mModelPropertyName;

            }
            set
            {
                mModelPropertyName = value;

            }
        }
        /// <summary>
        /// Gets or sets the control.
        /// </summary>
        /// <value>
        /// The control.
        /// </value>
        public object Control
        {
            get
            {
                object ControlRet = default;
                ControlRet = mControl;
                return ControlRet;

            }
            set
            {

                mControl = value;
            }
        }


    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Collections.CollectionBase" />
    public class QBEBoundControls : CollectionBase
    {

        /// <summary>
        /// Adds the specified qbe bound control.
        /// </summary>
        /// <param name="QBEBoundControl">The qbe bound control.</param>
        /// <returns></returns>
        public bool Add(QBEBoundControl QBEBoundControl)
        {
#pragma warning disable CS0168 // La variabile è dichiarata, ma non viene mai usata
            try
            {
                List.Add(QBEBoundControl);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
#pragma warning restore CS0168 // La variabile è dichiarata, ma non viene mai usata

        }
        /// <summary>
        /// Adds the specified model property name.
        /// </summary>
        /// <param name="ModelPropertyName">Name of the model property.</param>
        /// <param name="Control">The control.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <returns></returns>
        public bool Add(string ModelPropertyName, object Control, string PropertyName)
        {

            var QBEc = new QBEBoundControl();
            QBEc.Control = Control;
            QBEc.ModelPropertyName = ModelPropertyName;
            QBEc.ControlPropertyName = PropertyName;
#pragma warning disable CS0168 // La variabile è dichiarata, ma non viene mai usata
            try
            {
                List.Add(QBEc);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
#pragma warning restore CS0168 // La variabile è dichiarata, ma non viene mai usata


        }


    }


    #endregion


    //[Serializable]
    //public enum QBEMode
    //{
    //    Query = 0,
    //    Report = 1
    //}

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum ReportViewerMode
    {
        /// <summary>
        /// The web
        /// </summary>
        WEB = 0,
        /// <summary>
        /// The PDF URL
        /// </summary>
        PDFUrl = 1,
        /// <summary>
        /// The PDF stream
        /// </summary>
        PDFStream = 2
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum UseInQBEEnum
    {
        /// <summary>
        /// The use in que
        /// </summary>
        UseInQUE = 1,
        /// <summary>
        /// The do not use in qbe
        /// </summary>
        DoNotUseInQBE = 0
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum QBEResultMode
    {
        /// <summary>
        /// The bound controls
        /// </summary>
        BoundControls = 0,
        /// <summary>
        /// All rows SQL query
        /// </summary>
        AllRowsSQLQuery = 2,
        /// <summary>
        /// The single row SQL query
        /// </summary>
        SingleRowSQLQuery = 1,
        /// <summary>
        /// The multiple rows SQL query
        /// </summary>
        MultipleRowsSQLQuery = 3,
        /// <summary>
        /// The multiple rows items
        /// </summary>
        MultipleRowsItems = 4,
        /// <summary>
        /// The single row item
        /// </summary>
        SingleRowItem = 5,
        /// <summary>
        /// All rows items
        /// </summary>
        AllRowsItems = 6

    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum QBEColumnsTypes
    {
        /// <summary>
        /// The CheckBox
        /// </summary>
        CheckBox = 0,
        /// <summary>
        /// The ComboBox
        /// </summary>
        ComboBox = 1,
        /// <summary>
        /// The image
        /// </summary>
        Image = 2,
        /// <summary>
        /// The link
        /// </summary>
        Link = 3,
        /// <summary>
        /// The text box
        /// </summary>
        TextBox = 4


    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class QBEUserColumnsSet
    {
        /// <summary>
        /// The name
        /// </summary>
        public string Name = "";
        /// <summary>
        /// The description
        /// </summary>
        public string Description = "";
        /// <summary>
        /// The user name
        /// </summary>
        public string UserName = "";
        /// <summary>
        /// The apply to users or groups
        /// </summary>
        public string ApplyToUsersOrGroups = "everyone";
        /// <summary>
        /// The database object name
        /// </summary>
        public string DBObjectName = "";
        /// <summary>
        /// The qbe user columns
        /// </summary>
        public List<QBEUserColumn> QBEUserColumns;
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class QBEUserColumn
    {
        /// <summary>
        /// The database column name
        /// </summary>
        public string DBColumnName = "";
        /// <summary>
        /// The use in qbe
        /// </summary>
        public bool UseInQBE = true;
        /// <summary>
        /// The display in qbe result
        /// </summary>
        public bool DisplayInQBEResult = true;
        /// <summary>
        /// The friendly name
        /// </summary>
        public string FriendlyName = "";
        /// <summary>
        /// The qbe value
        /// </summary>
        public string QBEValue = "";
        /// <summary>
        /// The display format
        /// </summary>
        public string DisplayFormat = "";
        /// <summary>
        /// The back color
        /// </summary>
        public System.Drawing.Color BackColor;
        /// <summary>
        /// The fore color
        /// </summary>
        public System.Drawing.Color ForeColor;
        /// <summary>
        /// The qbe column type
        /// </summary>
        public QBEColumnsTypes QBEColumnType;
        /// <summary>
        /// The column width
        /// </summary>
        public int ColumnWidth = 0;
    }


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DbColumn
    {
        /// <summary>
        /// The name
        /// </summary>
        public string Name = "";
        /// <summary>
        /// The friendly name
        /// </summary>
        public string FriendlyName = "";
        /// <summary>
        /// The is boolean
        /// </summary>
        public bool IsBoolean = false;
        /// <summary>
        /// The is date
        /// </summary>
        public bool IsDate = false;
        /// <summary>
        /// The is time
        /// </summary>
        public bool IsTime = false;
        /// <summary>
        /// The is date time
        /// </summary>
        public bool IsDateTime = false;
        /// <summary>
        /// The is string
        /// </summary>
        public bool IsString = false;
        /// <summary>
        /// The is numeric
        /// </summary>
        public bool IsNumeric = false;
    }


    /// <summary>
    /// 
    /// </summary>
    public class QBEColumn
    {
        //public XQBEForm QBEForm = null;
        /// <summary>
        /// The m database column
        /// </summary>
        private string mDbColumn;
        /// <summary>
        /// The m use in qbe
        /// </summary>
        private bool mUseInQBE = false;
        /// <summary>
        /// The m display in qbe result
        /// </summary>
        private bool mDisplayInQBEResult = true;
        /// <summary>
        /// The m display in qbe
        /// </summary>
        private bool mDisplayInQBE = true;
        /// <summary>
        /// The m friendly name
        /// </summary>
        private string mFriendlyName = "";
        /// <summary>
        /// The m qbe value
        /// </summary>
        private string mQBEValue;
        /// <summary>
        /// The m display format
        /// </summary>
        private string mDisplayFormat = "";
        /// <summary>
        /// The m back color
        /// </summary>
        private System.Drawing.Color mBackColor;
        /// <summary>
        /// The m fore color
        /// </summary>
        private System.Drawing.Color mForeColor;
        /// <summary>
        /// The m qbe column type
        /// </summary>
        private QBEColumnsTypes mQBEColumnType = QBEColumnsTypes.TextBox;
        /// <summary>
        /// The m column size
        /// </summary>
        private int mColumnSize = 0;
        /// <summary>
        /// The m ordinal position
        /// </summary>
        private int mOrdinalPosition = 0;
        /// <summary>
        /// The m font
        /// </summary>
        private System.Drawing.Font mFont = null;
        /// <summary>
        /// The m alignment
        /// </summary>
        private Wisej.Web.DataGridViewContentAlignment mAlignment = Wisej.Web.DataGridViewContentAlignment.TopLeft;
        /// <summary>
        /// The m font style
        /// </summary>
        private System.Drawing.FontStyle mFontStyle = new System.Drawing.FontStyle();
        /// <summary>
        /// The m report name
        /// </summary>
        private string mReportName;
        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>
        /// The size of the font.
        /// </value>
        public float FontSize { get; set; }

        /// <summary>
        /// Gets or sets the qbe initial value.
        /// </summary>
        /// <value>
        /// The qbe initial value.
        /// </value>
        public object QBEInitialValue { get; set; } = "";
        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        /// <value>
        /// The name of the report.
        /// </value>
        public string ReportName
        {
            get
            {
                string ReportNameRet = default;
                ReportNameRet = mReportName;
                return ReportNameRet;


            }
            set
            {
                mReportName = value;
            }
        }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        /// <value>
        /// The font style.
        /// </value>
        public System.Drawing.FontStyle FontStyle
        {
            get
            {
                System.Drawing.FontStyle FontStyleRet = default;
                FontStyleRet = mFontStyle;
                return FontStyleRet;

            }
            set
            {
                mFontStyle = value;
            }
        }


        /// <summary>
        /// Gets or sets the aligment.
        /// </summary>
        /// <value>
        /// The aligment.
        /// </value>
        public Wisej.Web.DataGridViewContentAlignment Aligment
        {
            get
            {
                Wisej.Web.DataGridViewContentAlignment AligmentRet = default;
                AligmentRet = mAlignment;
                return AligmentRet;

            }
            set
            {
                mAlignment = value;
            }
        }


        /// <summary>
        /// Gets or sets the type of the qbe column.
        /// </summary>
        /// <value>
        /// The type of the qbe column.
        /// </value>
        public QBEColumnsTypes QBEColumnType
        {
            get
            {
                QBEColumnsTypes QBEColumnTypeRet = default;
                QBEColumnTypeRet = mQBEColumnType;
                return QBEColumnTypeRet;

            }
            set
            {
                mQBEColumnType = value;
            }
        }
        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public System.Drawing.Font Font
        {
            get
            {
                System.Drawing.Font FontRet = default;
                FontRet = mFont;
                return FontRet;

            }
            set
            {
                mFont = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of the fore.
        /// </summary>
        /// <value>
        /// The color of the fore.
        /// </value>
        public System.Drawing.Color ForeColor
        {
            get
            {
                System.Drawing.Color ForeColorRet = default;
                ForeColorRet = mForeColor;
                return ForeColorRet;

            }
            set
            {
                mForeColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of the back.
        /// </summary>
        /// <value>
        /// The color of the back.
        /// </value>
        public System.Drawing.Color BackColor
        {
            get
            {
                System.Drawing.Color BackColorRet = default;
                BackColorRet = mBackColor;
                return BackColorRet;

            }
            set
            {
                mBackColor = value;
            }
        }


        /// <summary>
        /// Gets or sets the display format.
        /// </summary>
        /// <value>
        /// The display format.
        /// </value>
        public string DisplayFormat
        {
            get
            {
                string DisplayFormatRet = default;
                DisplayFormatRet = mDisplayFormat;
                return DisplayFormatRet;

            }
            set
            {
                mDisplayFormat = value;
            }
        }
        /// <summary>
        /// Gets or sets the qbe value.
        /// </summary>
        /// <value>
        /// The qbe value.
        /// </value>
        public object QBEValue
        {
            get
            {
                object QBEValueRet = default;
                QBEValueRet = mQBEValue;
                return QBEValueRet;
            }
            set
            {
                mQBEValue = Conversions.ToString(value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>
        /// The name of the friendly.
        /// </value>
        public string FriendlyName
        {
            get
            {
                string FriendlyNameRet = default;
                FriendlyNameRet = mFriendlyName;
                if (mFriendlyName == null)
                    mFriendlyName = DbColumn;
                return FriendlyNameRet;
            }
            set
            {
                mFriendlyName = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [display in qbe result].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [display in qbe result]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayInQBEResult
        {
            get
            {
                bool DisplayInQBEResultRet = default;
                DisplayInQBEResultRet = mDisplayInQBEResult;
                return DisplayInQBEResultRet;
            }
            set
            {
                mDisplayInQBEResult = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [use in qbe].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use in qbe]; otherwise, <c>false</c>.
        /// </value>
        public bool UseInQBE
        {
            get
            {
                bool UseInQBERet = default;
                UseInQBERet = mUseInQBE;
                return UseInQBERet;
            }
            set
            {
                mUseInQBE = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [display in qbe].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [display in qbe]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayInQBE
        {
            get
            {
                return mDisplayInQBE;
            }
            set
            {
                mDisplayInQBE = value;
            }
        }


        /// <summary>
        /// Gets or sets the database column.
        /// </summary>
        /// <value>
        /// The database column.
        /// </value>
        public string DbColumn
        {
            get
            {

                return mDbColumn;
            }
            set
            {
                mDbColumn = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the column.
        /// </summary>
        /// <value>
        /// The size of the column.
        /// </value>
        public int ColumnSize
        {
            get
            {
                int ColumnWidthRet = default;
                ColumnWidthRet = mColumnSize;
                return ColumnWidthRet;
            }
            set
            {
                mColumnSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the ordinal position.
        /// </summary>
        /// <value>
        /// The ordinal position.
        /// </value>
        public int OrdinalPosition
        {
            get
            {
                int OrdinalPositionRet = default;
                OrdinalPositionRet = mOrdinalPosition;
                return OrdinalPositionRet;
            }
            set
            {
                mOrdinalPosition = value;
            }
        }


    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Collections.Generic.Dictionary&lt;System.String, Passero.Framework.Controls.QBEColumn&gt;" />
    public class QBEColumns : Dictionary<string, QBEColumn>
    {
        //public XQBEForm QBEForm;


        /// <summary>
        /// Initializes a new instance of the <see cref="QBEColumns"/> class.
        /// </summary>
        public QBEColumns() : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }

        /// <summary>
        /// Adds the specified database column.
        /// </summary>
        /// <param name="DbColumn">The database column.</param>
        /// <param name="FriendlyName">Name of the friendly.</param>
        /// <param name="DisplayFormat">The display format.</param>
        /// <param name="QBEValue">The qbe value.</param>
        /// <param name="UseInQBE">if set to <c>true</c> [use in qbe].</param>
        /// <param name="DisplayInQBEResult">if set to <c>true</c> [display in qbe result].</param>
        /// <param name="ColumnWidth">Width of the column.</param>
        /// <returns></returns>
        public QBEColumn Add(string DbColumn, string FriendlyName = "", string DisplayFormat = "", object QBEValue = null, bool UseInQBE = true, bool DisplayInQBEResult = true, int ColumnWidth = 0)
        {
            var x = new QBEColumn();
            return Add("", DbColumn, FriendlyName, DisplayFormat, QBEValue, UseInQBE, DisplayInQBEResult, QBEColumnsTypes.TextBox, ColumnWidth);

        }
        /// <summary>
        /// Adds the specified database column.
        /// </summary>
        /// <param name="DbColumn">The database column.</param>
        /// <param name="FriendlyName">Name of the friendly.</param>
        /// <param name="DisplayFormat">The display format.</param>
        /// <param name="QBEValue">The qbe value.</param>
        /// <param name="UseInQBE">if set to <c>true</c> [use in qbe].</param>
        /// <param name="DisplayInQBEResult">if set to <c>true</c> [display in qbe result].</param>
        /// <param name="QBEColumnType">Type of the qbe column.</param>
        /// <param name="ColumnWidth">Width of the column.</param>
        /// <returns></returns>
        public QBEColumn Add(string DbColumn, string FriendlyName, string DisplayFormat, object QBEValue, bool UseInQBE, bool DisplayInQBEResult, QBEColumnsTypes QBEColumnType, int ColumnWidth)
        {
            return Add("", DbColumn, FriendlyName, DisplayFormat, QBEValue, UseInQBE, DisplayInQBEResult, QBEColumnType, ColumnWidth);
        }


        /// <summary>
        /// Adds for report.
        /// </summary>
        /// <param name="ReportName">Name of the report.</param>
        /// <param name="DbColumn">The database column.</param>
        /// <param name="FriendlyName">Name of the friendly.</param>
        /// <param name="QBEValue">The qbe value.</param>
        /// <param name="QBEColumnType">Type of the qbe column.</param>
        /// <returns></returns>
        public QBEColumn AddForReport(string ReportName, string DbColumn, string FriendlyName, object QBEValue, QBEColumnsTypes QBEColumnType)
        {
            return Add(ReportName, DbColumn, FriendlyName, "", QBEValue, true, false, QBEColumnType, 0);
        }

        /// <summary>
        /// Adds for report.
        /// </summary>
        /// <param name="ReportName">Name of the report.</param>
        /// <param name="DbColumn">The database column.</param>
        /// <param name="FriendlyName">Name of the friendly.</param>
        /// <param name="QBEValue">The qbe value.</param>
        /// <returns></returns>
        public QBEColumn AddForReport(string ReportName, string DbColumn, string FriendlyName, object QBEValue = null)
        {
            return Add(ReportName, DbColumn, FriendlyName, "", QBEValue, true, false, QBEColumnsTypes.TextBox, 0);
        }

        /// <summary>
        /// Adds for report.
        /// </summary>
        /// <param name="Report">The report.</param>
        /// <param name="DbColumn">The database column.</param>
        /// <param name="FriendlyName">Name of the friendly.</param>
        /// <param name="QBEValue">The qbe value.</param>
        /// <returns></returns>
        public QBEColumn AddForReport(object Report, string DbColumn, string FriendlyName, object QBEValue = null)
        {
            return Add(DbColumn, FriendlyName, "", QBEValue, true, false, QBEColumnsTypes.TextBox, 0);
        }

        /// <summary>
        /// Adds the specified report name.
        /// </summary>
        /// <param name="ReportName">Name of the report.</param>
        /// <param name="DbColumn">The database column.</param>
        /// <param name="FriendlyName">Name of the friendly.</param>
        /// <param name="DisplayFormat">The display format.</param>
        /// <param name="QBEValue">The qbe value.</param>
        /// <param name="UseInQBE">if set to <c>true</c> [use in qbe].</param>
        /// <param name="DisplayInQBEResult">if set to <c>true</c> [display in qbe result].</param>
        /// <param name="QBEColumnType">Type of the qbe column.</param>
        /// <param name="ColumnWidth">Width of the column.</param>
        /// <returns></returns>
        private QBEColumn Add(string ReportName, string DbColumn, string FriendlyName, string DisplayFormat, object QBEValue, bool UseInQBE, bool DisplayInQBEResult, QBEColumnsTypes QBEColumnType, int ColumnWidth)
        {
            var x = new QBEColumn();

            x.ReportName = ReportName;
            //x.QBEForm = QBEForm;
            x.OrdinalPosition = Count;
            x.DbColumn = DbColumn;
            x.UseInQBE = UseInQBE;
            x.QBEValue = QBEValue;
            x.FriendlyName = FriendlyName;
            x.DisplayInQBEResult = DisplayInQBEResult;
            x.DisplayFormat = DisplayFormat;
            x.QBEColumnType = QBEColumnType;
            x.ColumnSize = ColumnWidth;
            x.Aligment = Wisej.Web.DataGridViewContentAlignment.TopLeft;
            x.QBEInitialValue = QBEValue;

            //if (DbColumn.IsNumeric() | DbColumn.IsDate())
            //{
            //    x.Aligment = DataGridViewContentAlignment.TopRight;
            //}
            //if (DbColumn.IsString())
            //{
            //    x.Aligment = DataGridViewContentAlignment.TopLeft;
            //}
            //if (DbColumn.IsTime() | DbColumn.IsDate())
            //{
            //    x.Aligment = DataGridViewContentAlignment.TopRight;
            //}
            //if (DbColumn.IsBoolean())
            //{
            //    x.Aligment = DataGridViewContentAlignment.MiddleCenter;
            //}

            if (string.IsNullOrEmpty(Strings.Trim(x.FriendlyName)))
                x.FriendlyName = x.DbColumn;
            if (ReportName.Trim() != "")
            {
                Add(ReportName.Trim().ToUpper() + "|" + DbColumn.Trim().ToUpper(), x);
            }
            else
            {
                Add(DbColumn.Trim().ToUpper(), x);
            }

            //List.Add(x);
            return x;
        }





    }


    //[Serializable]
    //public class QBEReportSortColumn
    //{
    //    public string Name { get; set; }
    //    public string FriendlyName { get; set; }
    //    public int Position { get; set; }
    //    public string AscDesc { get; set; }
    //}

    //[Serializable ]
    //public enum ReportTypes
    //{
    //    SSRSLocalReport =0,
    //    SSRSRemoteServer=1
    //}



    //[Serializable]
    //public class QBEReport
    //{
    //    public Dictionary<string, Passero.Framework.RsReports.DataSet> DataSets = new Dictionary<string, RsReports.DataSet>(StringComparer.InvariantCultureIgnoreCase );
    //    public ReportTypes ReportType = ReportTypes.SSRSLocalReport;
    //    public Dapper.DynamicParameters SQLQueryParameters = new Dapper.DynamicParameters();
    //    public Dictionary<string, QBEReportSortColumn> SortColumns = new Dictionary<string, QBEReportSortColumn>(StringComparer.InvariantCultureIgnoreCase);
    //    public Dictionary<string, QBEReportSortColumn> SelectedSortColumns = new Dictionary<string, QBEReportSortColumn>(StringComparer.InvariantCultureIgnoreCase);
    //    public string SQLQuery = "";

    //    private string mReportTitle;
    //    private string mReportFileName;
    //    private string mReportDescription;
    //    private bool mReportUseLike;
    //    public IDbConnection DbConnection { get; set; }
    //    public RsReports.DataSet PrimaryDataSet { get; set; }

    //    public bool SetPrimaryDataSet (string Name)
    //    {
    //        bool result = false;
    //        if (this.DataSets.ContainsKey(Name))
    //        {
    //            this.PrimaryDataSet = this.DataSets[Name];
    //            result = true;  
    //        }

    //        return result;  
    //    }
    //    public Passero.Framework.RsReports.DataSet AddDataSet<T>(string Name, IDbConnection DbConnection, string SQLQuery="", DynamicParameters Parameters =null)
    //    {
    //        Passero.Framework.RsReports.DataSet ds = new RsReports.DataSet();

    //        ds.Name = Name;
    //        ds.DbConnection = DbConnection; 

    //        if (SQLQuery != "")
    //            ds.SQLQuery  = SQLQuery;
    //        if (Parameters != null)
    //            ds.Parameters = Parameters;
    //        ds.ModelType = typeof(T);
    //        ds.EnsureReportDataSet();
    //        this.DataSets.Add(Name, ds);
    //        return ds;  
    //    }


    //    public string OrderBy()
    //    {
    //        string s = "";
    //        foreach (var item in this.SelectedSortColumns .Values )
    //        {
    //            s += $"{item.Name} {item.AscDesc}, "; 
    //        }

    //        s = s.Trim();
    //        if (s.EndsWith (","))
    //        {
    //            s=s.Substring (0,s.Length - 1);
    //        }

    //        if (s != "")
    //            s = $" ORDER BY {s}";
    //        return s;
    //    }


    //    public bool ReportUseLike
    //    {
    //        get
    //        {
    //            bool ReportUseLikeRet = default;
    //            ReportUseLikeRet = mReportUseLike;
    //            return ReportUseLikeRet;

    //        }
    //        set
    //        {
    //            mReportUseLike = value;

    //        }
    //    }
    //    public string ReportFileName
    //    {
    //        get
    //        {
    //            string ReportFileNameRet = default;
    //            ReportFileNameRet = mReportFileName;
    //            return ReportFileNameRet;
    //        }
    //        set
    //        {
    //            mReportFileName = value;

    //        }
    //    }



    //    public string ReportDescription
    //    {
    //        get
    //        {
    //            string ReportDescriptionRet = default;
    //            ReportDescriptionRet = mReportDescription;
    //            return ReportDescriptionRet;

    //        }
    //        set
    //        {
    //            mReportDescription = value;

    //        }
    //    }


    //    public string ReportTitle
    //    {
    //        get
    //        {
    //            string ReportTitleRet = default;
    //            ReportTitleRet = mReportTitle;
    //            return ReportTitleRet;

    //        }
    //        set
    //        {
    //            mReportTitle = value;

    //        }
    //    }
    //}
    //public class QBEReports : Dictionary<string,QBEReport >
    //{

    //    public QBEReport Add(string ReportTitle, string ReportFileName, string ReportDescription = "", IDbConnection DbConnection=null)
    //    {
    //        var x = new QBEReport();

    //        x.ReportDescription = ReportDescription;
    //        x.ReportFileName = ReportFileName;
    //        x.ReportTitle = ReportTitle;
    //        x.DbConnection = DbConnection;  
    //        Add(x.ReportTitle.Trim().ToUpper() ,x);
    //        return x;
    //    }


    //}

}

using Dapper;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Wisej.Web.TextBox" />
    [DefaultBindingProperty("Value")]
    public partial class DbLookUpTextBox : Wisej.Web.TextBox
    {
        /// <summary>
        /// Gets or sets the data bind controls.
        /// </summary>
        /// <value>
        /// The data bind controls.
        /// </value>
        public Dictionary<string, DataBindControl> DataBindControls { get; set; } = new Dictionary<string, DataBindControl>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// The parameters
        /// </summary>
        private DynamicParameters parameters = new DynamicParameters();

        /// <summary>
        /// Occurs when [e look up].
        /// </summary>
        public event eLookUpDataEventHandler eLookUp;
        /// <summary>
        /// 
        /// </summary>
        public delegate void eLookUpDataEventHandler();

        #region Initialization
        // Initialization Code 
        /// <summary>
        /// Initializes a new instance of the <see cref="DbLookUpTextBox"/> class.
        /// </summary>
        public DbLookUpTextBox()
        {
            InitializeComponent();
            SetControl();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbLookUpTextBox"/> class.
        /// </summary>
        /// <param name="ModelClass">The model class.</param>
        public DbLookUpTextBox(Type ModelClass = null)
        {
            InitializeComponent();
            SetControl();
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbLookUpTextBox"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public DbLookUpTextBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            SetControl();
        }

        /// <summary>
        /// Sets the control.
        /// </summary>
        private void SetControl()
        {
            //TextChanged += DbLookUpTextBox_TextChanged;
            KeyPress += DbLookUpTextBox_KeyPress;
            foreach (Binding item in DataBindings)
            {
                if (item.PropertyName == "Value")
                    item.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            }
        }
        /// <summary>
        /// Handles the TextChanged event of the DbLookUpTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DbLookUpTextBox_TextChanged(object sender, EventArgs e)
        {
            //e.Handled = true;
            return;
            {
                if (Lock == false)
                {
                    this.LookUp(true);
                }
            }
        }

        /// <summary>
        /// Handles the KeyPress event of the DbLookUpTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data.</param>
        private void DbLookUpTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (mLookUpOnEdit)
            {
                Lock = true;
                this.LookUp(true);
                e.Handled = true;
                Lock = false;
            }
            else
            {
                this.ClearControls(true);
                if (e.KeyChar == (char)Keys.Tab | e.KeyChar == (char)Keys.Enter)
                {
                    Lock = true;
                    this.LookUp(true);
                    //e.Handled = true;
                    Lock = false;
                }
                
            }

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public enum LookUpModes
        {
            /// <summary>
            /// The standard
            /// </summary>
            Standard = 0,
            /// <summary>
            /// The enhanced
            /// </summary>
            Enhanced = 1
        }

        /// <summary>
        /// Gets the name of the edit property.
        /// </summary>
        /// <value>
        /// The name of the edit property.
        /// </value>
        public string EditPropertyName { get; private set; } = "Value";
        /// <summary>
        /// The m look up mode
        /// </summary>
        private LookUpModes mLookUpMode = LookUpModes.Standard;
        /// <summary>
        /// Gets or sets the look up mode.
        /// </summary>
        /// <value>
        /// The look up mode.
        /// </value>
        public LookUpModes LookUpMode
        {
            get
            {
                return mLookUpMode;
            }
            set
            {
                mLookUpMode = value;
            }
        }
        /// <summary>
        /// The lock
        /// </summary>
        private bool Lock = false;
#pragma warning disable CS0414 // Il campo 'DbLookUpTextBox.SameColumn' è assegnato, ma il suo valore non viene mai usato
        /// <summary>
        /// The same column
        /// </summary>
        private bool SameColumn = false;
#pragma warning restore CS0414 // Il campo 'DbLookUpTextBox.SameColumn' è assegnato, ma il suo valore non viene mai usato
        /// <summary>
        /// The valid look up
        /// </summary>
        public bool ValidLookUp = false;

        //private object mDataSource=null;    
        //public object DataSource 
        //{ get
        //    {
        //        return mDataSource; 
        //    }
        //    set
        //    { 
        //        mDataSource =value;
        //        //Type type = Passero.Framework .ReflectionHelper .GetListType (mDataSource);
        //        DataTable dt = Passero.Framework.DataBaseHelper.ListToDataTable(mDataSource);

        //    }
        //}
        /// <summary>
        /// The m show search tool
        /// </summary>
        private bool mShowSearchTool = true;

        /// <summary>
        /// Gets or sets a value indicating whether [show search tool].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show search tool]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowSearchTool
        {
            get
            {
                return mShowSearchTool;
            }
            set
            {
                mShowSearchTool = value;
                Tools["search"].Visible = value;
            }
        }
        /// <summary>
        /// The m show clear tool
        /// </summary>
        private bool mShowClearTool = false;
        /// <summary>
        /// Gets or sets a value indicating whether [show clear tool].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show clear tool]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowClearTool
        {
            get
            {
                return mShowClearTool;
            }
            set
            {
                mShowClearTool = value;
                Tools["clear"].Visible = value;
            }
        }

        /// <summary>
        /// The m look up on edit
        /// </summary>
        private bool mLookUpOnEdit = false;
        /// <summary>
        /// Gets or sets a value indicating whether [look up on edit].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [look up on edit]; otherwise, <c>false</c>.
        /// </value>
        public bool LookUpOnEdit
        {
            get
            {
                return mLookUpOnEdit;
            }
            set
            {
                mLookUpOnEdit = value;
            }
        }

        /// <summary>
        /// The m value member
        /// </summary>
        private string mValueMember = "";
        /// <summary>
        /// Gets or sets the value member.
        /// </summary>
        /// <value>
        /// The value member.
        /// </value>
        public string ValueMember
        {
            get
            {
                string ValueMemberRet = default;
                ValueMemberRet = mValueMember;
                return ValueMemberRet;
            }
            set
            {
                mValueMember = value;
                if (mValueMember.Equals(mDisplayMember, StringComparison.InvariantCultureIgnoreCase))
                    LookUpMode = LookUpModes.Standard;
                else
                    LookUpMode = LookUpModes.Enhanced;

                SetupDefault();
            }
        }
        /// <summary>
        /// The m display member
        /// </summary>
        private string mDisplayMember = "";
        /// <summary>
        /// Gets or sets the display member.
        /// </summary>
        /// <value>
        /// The display member.
        /// </value>
        public string DisplayMember
        {
            get
            {
                string DisplayMemberRet = default;
                DisplayMemberRet = mDisplayMember;
                return DisplayMemberRet;
            }
            set
            {
                mDisplayMember = value;
                if (mValueMember.Equals(mDisplayMember, StringComparison.InvariantCultureIgnoreCase))
                    LookUpMode = LookUpModes.Standard;
                else
                    LookUpMode = LookUpModes.Enhanced;
                SetupDefault();
            }
        }
        /// <summary>
        /// The m value
        /// </summary>
        private object mValue = null;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [Bindable(true)]
        public object Value
        {
            get
            {
                object ValueRet = default;
                ValueRet = mValue;
                return ValueRet;
            }
            set
            {

                if (this.DisplayMember.Trim() == "" | this.ValueMember.Trim() == "")
                    return;

                if (value == null | value == System.DBNull.Value  )
                {
                    this.Text = "";
                    ClearControls();
                    return;
                }

                if (value != null && value.Equals(mValue))
                    return;

                if (mValue == (value))
                    return;

                mValue = value;
                this.Text = mValue.ToString();

                // deve cercare nel datasource il relativo DisplayValue
                //if (this.ValueMember.Equals(this.DisplayMember, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    this.SameColumn = true;
                //}
                //else
                //{
                //    this.SameColumn = false;
                //}
                this.LookUp(false);

            }
        }

        /// <summary>
        /// Gets or sets the database connection.
        /// </summary>
        /// <value>
        /// The database connection.
        /// </value>
        public IDbConnection DbConnection { get; set; }
        /// <summary>
        /// Gets the SQL query.
        /// </summary>
        /// <value>
        /// The SQL query.
        /// </value>
        public string SQLQuery { get; private set; }
        /// <summary>
        /// Gets or sets the database parameters.
        /// </summary>
        /// <value>
        /// The database parameters.
        /// </value>
        public DynamicParameters DbParameters { get; set; } = new DynamicParameters();
        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public IDictionary<string, object> Model { get; private set; } = null;
        /// <summary>
        /// The table name
        /// </summary>
        private string TableName = "";
        /// <summary>
        /// The m model class
        /// </summary>
        private Type mModelClass;
        /// <summary>
        /// Gets or sets the model class.
        /// </summary>
        /// <value>
        /// The model class.
        /// </value>
        public Type ModelClass
        {
            get
            { return mModelClass; }
            set
            {
                mModelClass = value;
                TableName = Passero.Framework.DapperHelper.Utilities.GetTableName(mModelClass);
            }
        }
        /// <summary>
        /// Gets or sets the select clause.
        /// </summary>
        /// <value>
        /// The select clause.
        /// </value>
        public string SelectClause { get; set; } = " * ";
        /// <summary>
        /// Gets or sets the where clause.
        /// </summary>
        /// <value>
        /// The where clause.
        /// </value>
        public string WhereClause { get; set; } = "";
        /// <summary>
        /// Setups the default.
        /// </summary>
        public void SetupDefault()
        {


        }

        /// <summary>
        /// Clears the controls.
        /// </summary>
        /// <param name="FromEditing">if set to <c>true</c> [from editing].</param>
        public void ClearControls(bool FromEditing = false)
        {
#pragma warning disable CS0168 // La variabile è dichiarata, ma non viene mai usata
            try
            {
                foreach (DataBindControl item in this.DataBindControls.Values)
                {
                    Control control = (Control)Activator.CreateInstance(item.Control.GetType());
                    object _value = Interaction.CallByName(control, item.ControlPropertyName, CallType.Get);
                    Interaction.CallByName(item.Control, item.ControlPropertyName, CallType.Set, _value);
                }

                //if (FromEditing == false)
                //    this.Text = "";
            }
            catch (Exception ex)
            {
            }
#pragma warning restore CS0168 // La variabile è dichiarata, ma non viene mai usata
        }
        /// <summary>
        /// Ensures the SQL query value member.
        /// </summary>
        private void EnsureSQLQueryValueMember()
        {
            this.SQLQuery = "";
            this.parameters = new DynamicParameters();

            if (this.ModelClass == null)
                return;

            //if (this.Text == "")
            //    return;

            string TableName = Passero.Framework.DapperHelper.Utilities.GetTableName(this.ModelClass);
            this.SQLQuery = $"SELECT {this.SelectClause} FROM {TableName} WHERE {this.ValueMember}=@ValueMember";


            if (String.IsNullOrEmpty(WhereClause) == false)
            {
                this.SQLQuery = this.SQLQuery + $" AND {WhereClause}";
            }

            foreach (var par in this.DbParameters.ParameterNames)
            {
                parameters.Add(par, 0);
            }
            //parameters.Add("@ValueMember", this.mValue);
            parameters.Add("@ValueMember",this.Text  );

        }

        /// <summary>
        /// Ensures the SQL query display member.
        /// </summary>
        private void EnsureSQLQueryDisplayMember()
        {
            this.SQLQuery = "";
            parameters = new DynamicParameters();
            if (this.ModelClass == null)
                return;

            //if (this.Text == "")
            //    return;


            string TableName = Passero.Framework.DapperHelper.Utilities.GetTableName(this.ModelClass);
            this.SQLQuery = $"SELECT {this.SelectClause} FROM {TableName} WHERE {this.DisplayMember}=@DisplayMember";

            if (String.IsNullOrEmpty(WhereClause) == false)
            {
                this.SQLQuery = this.SQLQuery + $" AND {WhereClause}";
            }

            foreach (var par in this.DbParameters.ParameterNames)
            {
                parameters.Add(par, 0);
            }


            parameters.Add("@DisplayMember", this.Text);

        }

        /// <summary>
        /// Looks up.
        /// </summary>
        /// <param name="FromEditing">if set to <c>true</c> [from editing].</param>
        /// <returns></returns>
        public bool LookUp(bool FromEditing = false)
        {
            ValidLookUp = false;
            this.ClearControls(FromEditing);

            if (FromEditing)
            {
                this.mValue = this.Text;
                if (LookUpMode  == LookUpModes.Standard   )
                {
                    this.EnsureSQLQueryValueMember();
                }
                else
                {
                    this.EnsureSQLQueryDisplayMember();
                }
            }
            else
            {
                this.EnsureSQLQueryValueMember();
            }
            string _sqlquery = Passero.Framework.DapperHelper.Utilities.ResolveSQL(SQLQuery, this.parameters);
            if (SQLQuery == "")
                return ValidLookUp ;
            if (DbConnection == null)
                return ValidLookUp;
            try
            {
                
                Model = (IDictionary<string, object>)DbConnection.Query(SQLQuery, this.parameters).FirstOrDefault();
                if (Model != null)
                {
                    this.Lock = true;
                    if (LookUpMode == LookUpModes.Standard)
                    {

                        this.Text = Model[this.ValueMember].ToString();
                        this.mValue = Model[this.ValueMember].ToString();
                    }
                    else
                    {
                        this.Text = Model[this.DisplayMember].ToString();
                        this.mValue = Model[this.ValueMember];
                    }
                    ValidLookUp = true;
                    this.Lock = false;

                    // DataBindControls
                    foreach (DataBindControl item in this.DataBindControls.Values)
                    {
                        if (Model[item.ModelPropertyName] is not null)
                        {
                            Interaction.CallByName(item.Control, item.ControlPropertyName, CallType.Set, Model[item.ModelPropertyName]);
                        }
                        else
                        {
                            Interaction.CallByName(item.Control, item.ControlPropertyName, CallType.Set, "");
                        }
                    }
                }
                else
                {

                }

            }
            catch (Exception)
            {
                this.Lock = true;
                this.Text = "";
                this.Lock = false;

            }
            if (this.eLookUp != null)
                this.eLookUp();

            return ValidLookUp;
        }
        /// <summary>
        /// Gets the bound control key.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <returns></returns>
        private string GetBoundControlKey(Control Control, string PropertyName)
        {
            string objname = Conversions.ToString(Interaction.CallByName(Control, "Name", CallType.Get, (object[])null));
            return (objname + "|" + PropertyName.Trim()).ToLower();
        }
        /// <summary>
        /// Adds the control.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <param name="ModelPropertyName">Name of the model property.</param>
        /// <returns></returns>
        public bool AddControl(Control Control, string ControlPropertyName, string ModelPropertyName)
        {
            string Key = GetBoundControlKey(Control, ControlPropertyName);

            var _DataBindControl = new DataBindControl();
            _DataBindControl.Control = Control;
            _DataBindControl.ControlPropertyName = ControlPropertyName;
            _DataBindControl.ModelPropertyName = ModelPropertyName;

            DataBindControls[Key] = _DataBindControl;
            return true;
        }

        /// <summary>
        /// Removes the control.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <returns></returns>
        public int RemoveControl(Control Control, string ControlPropertyName)
        {
            string Key = GetBoundControlKey(Control, ControlPropertyName);
            return RemoveControl(Key);
        }

        /// <summary>
        /// Removes the control.
        /// </summary>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        public int RemoveControl(string Key)
        {
            if (DataBindControls.ContainsKey(Key) == true)
            {
                DataBindControls.Remove(Key);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Removes the control.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <returns></returns>
        public int RemoveControl(Control Control)
        {
            string objname = Conversions.ToString(Interaction.CallByName(Control, "Name", CallType.Get, (object[])null));
            string keytofind = (objname + "|").ToLower();
            return _RemoveControl(keytofind);
        }

        /// <summary>
        /// Removes the control.
        /// </summary>
        /// <param name="keytofind">The keytofind.</param>
        /// <returns></returns>
        private int _RemoveControl(string keytofind)
        {
            int removedobjects = 0;
            List<string> keys;
            keys = DataBindControls.Keys.Where(key => key.StartsWith(keytofind)).ToList();
            if (keys.Count > 0)
            {

                foreach (string key in keys)
                {
                    DataBindControls.Remove(key);
                    removedobjects += 1;
                }
            }

            return removedobjects;
        }

    }
}

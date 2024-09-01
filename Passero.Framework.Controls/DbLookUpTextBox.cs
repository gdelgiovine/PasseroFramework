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
    [DefaultBindingProperty("Value")]
    public partial class DbLookUpTextBox : Wisej.Web.TextBox
    {
        public Dictionary<string, DataBindControl> DataBindControls { get; set; } = new Dictionary<string, DataBindControl>(StringComparer.InvariantCultureIgnoreCase);

        private DynamicParameters parameters = new DynamicParameters();

        public event eLookUpDataEventHandler eLookUp;
        public delegate void eLookUpDataEventHandler();

        #region Initialization
        // Initialization Code 
        public DbLookUpTextBox()
        {
            InitializeComponent();
            SetControl();
        }

        public DbLookUpTextBox(Type ModelClass = null)
        {
            InitializeComponent();
            SetControl();
            
        }

        public DbLookUpTextBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            SetControl();
        }

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

        public enum LookUpModes
        {
            Standard = 0,
            Enhanced = 1
        }

        public string EditPropertyName { get; private set; } = "Value";
        private LookUpModes mLookUpMode = LookUpModes.Standard;
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
        private bool Lock = false;
#pragma warning disable CS0414 // Il campo 'DbLookUpTextBox.SameColumn' è assegnato, ma il suo valore non viene mai usato
        private bool SameColumn = false;
#pragma warning restore CS0414 // Il campo 'DbLookUpTextBox.SameColumn' è assegnato, ma il suo valore non viene mai usato
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
        private bool mShowSearchTool = true;

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
        private bool mShowClearTool = false;
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
        
        private bool mLookUpOnEdit = false;
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

        private string mValueMember = "";
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
        private string mDisplayMember = "";
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
        private object mValue = null;

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

        public IDbConnection DbConnection { get; set; }
        public string SQLQuery { get; private set; }
        public DynamicParameters DbParameters { get; set; } = new DynamicParameters();
        public IDictionary<string, object> Model { get; private set; } = null;
        private string TableName = "";
        private Type mModelClass;
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
        public string SelectClause { get; set; } = " * ";
        public string WhereClause { get; set; } = "";
        public void SetupDefault()
        {


        }

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
        private string GetBoundControlKey(Control Control, string PropertyName)
        {
            string objname = Conversions.ToString(Interaction.CallByName(Control, "Name", CallType.Get, (object[])null));
            return (objname + "|" + PropertyName.Trim()).ToLower();
        }
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

        public int RemoveControl(Control Control, string ControlPropertyName)
        {
            string Key = GetBoundControlKey(Control, ControlPropertyName);
            return RemoveControl(Key);
        }

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

        public int RemoveControl(Control Control)
        {
            string objname = Conversions.ToString(Interaction.CallByName(Control, "Name", CallType.Get, (object[])null));
            string keytofind = (objname + "|").ToLower();
            return _RemoveControl(keytofind);
        }

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

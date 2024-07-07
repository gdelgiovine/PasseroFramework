using Dapper;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    public partial class DbLookUpTextBox : Wisej.Web.TextBox
    {
        public Dictionary<string, DataBindControl> DataBindControls { get; set; } = new Dictionary<string, DataBindControl>(StringComparer.InvariantCultureIgnoreCase);



        #region Initialization
        // Initialization Code 
        public DbLookUpTextBox()
        {
            InitializeComponent();
            TextChanged += DbLookUpTextBox_TextChanged;
            KeyPress += DbLookUpTextBox_KeyPress;
        }

        public DbLookUpTextBox(Type ModelClass = null)
        {
            InitializeComponent();
            TextChanged += DbLookUpTextBox_TextChanged;
            KeyPress += DbLookUpTextBox_KeyPress;
        }

        public DbLookUpTextBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            TextChanged += DbLookUpTextBox_TextChanged;
            KeyPress += DbLookUpTextBox_KeyPress;
        }



        private void DbLookUpTextBox_TextChanged(object sender, EventArgs e)
        {
            if (LookUpMode == LookUpModes.Standard)
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
        }

        #endregion

        public enum LookUpModes
        {
            Standard = 0,
            Enhanced = 1
        }

        public string BoundPropertyName { get; private set; } = "Text";
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
                if (mLookUpMode == LookUpModes.Standard)
                    this.BoundPropertyName = "Text";
                else
                    this.BoundPropertyName = "Value";
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
        //private bool mLookUpOnDisplayMember = false;
        //public bool LookUpOnDisplayMember
        //{
        //    get
        //    {
        //        return mLookUpOnDisplayMember;
        //    }
        //    set
        //    {
        //        mLookUpOnDisplayMember = value;
        //    }
        //}
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
                if (LookUpMode == LookUpModes.Standard)
                {
                    this.mDisplayMember = value;
                }
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
                if (LookUpMode == LookUpModes.Standard)
                {
                    this.mValueMember = value;
                }
                SetupDefault();
            }
        }
        private object mValue = null;
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

                if (value == null)
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

                // deve cercare nel datasource il relativo DisplayValue
                if (this.ValueMember.Equals(this.DisplayMember, StringComparison.InvariantCultureIgnoreCase))
                {
                    this.SameColumn = true;
                }
                else
                {
                    this.SameColumn = false;
                }

                //this.EnsureSQLQueryValueMember();
                this.LookUp();
                if (this.ValidLookUp == false)
                    this.Text = "";

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
        public string SelectClause { get; set; } = "SELECT * ";
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

                if (FromEditing == false)
                    this.Text = "";
            }
            catch (Exception ex)
            {
            }
#pragma warning restore CS0168 // La variabile è dichiarata, ma non viene mai usata
        }


        private void EnsureSQLQuery()
        {
            this.SQLQuery = "";
            this.DbParameters = new DynamicParameters();

            if (this.ModelClass == null)
                return;


            object _value = Interaction.CallByName(this, this.BoundPropertyName, CallType.Get);

            if (_value.ToString().Trim() == "")
                return;

            string TableName = Passero.Framework.DapperHelper.Utilities.GetTableName(this.ModelClass);
            this.SQLQuery = $"{this.SelectClause} FROM {TableName} WHERE {this.ValueMember}=@ValueMember";
            this.DbParameters.Add("@ValueMember", _value);

        }


        private void EnsureSQLQueryValueMember()
        {
            this.SQLQuery = "";
            this.DbParameters = new DynamicParameters();

            if (this.ModelClass == null)
                return;
            if (this.mValue == null)
                return;

            if (this.mValue.ToString().Trim() == "")
                return;
            string TableName = Passero.Framework.DapperHelper.Utilities.GetTableName(this.ModelClass);
            this.SQLQuery = $"{this.SelectClause} FROM {TableName} WHERE {this.ValueMember}=@ValueMember";

            this.DbParameters.Add("@ValueMember", this.mValue);

        }

        private void EnsureSQLQueryDisplayMember()
        {
            this.SQLQuery = "";
            this.DbParameters = new DynamicParameters();
            if (this.ModelClass == null)
                return;

            string TableName = Passero.Framework.DapperHelper.Utilities.GetTableName(this.ModelClass);
            this.SQLQuery = $"{this.SelectClause} FROM {TableName} WHERE {this.DisplayMember}=@DisplayMember";

            this.DbParameters.Add("@DisplayMember", this.Text);

        }

        public void LookUp(bool FromEditing = false)
        {
            ValidLookUp = false;
            this.ClearControls(FromEditing);

            //if (FromEditing)
            //{
            //    if (LookUpMode  == LookUpModes.Standard   )
            //    {
            //        this.EnsureSQLQueryDisplayMember();
            //    }
            //    else
            //    {
            //        this.EnsureSQLQueryValueMember();
            //    }
            //}
            //else
            //{
            //    this.EnsureSQLQueryValueMember();
            //}

            this.EnsureSQLQuery();

            string _sqlquery = Passero.Framework.DapperHelper.Utilities.ResolveSQL(SQLQuery, DbParameters);
            if (SQLQuery == "")
                return;
            if (DbConnection == null)
                return;
            try
            {

                Model = (IDictionary<string, object>)DbConnection.Query(SQLQuery, DbParameters).FirstOrDefault();
                if (Model != null)
                {
                    this.Lock = true;
                    if (LookUpMode == LookUpModes.Standard)
                    {
                        this.Text = Model[this.ValueMember].ToString();
                    }
                    else
                    {
                        this.Text = Model[this.DisplayMember].ToString();

                        this.mValue = Model[this.ValueMember];
                    }
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

                    ValidLookUp = true;
                }
                else
                {
                    //this.Lock = true;
                    //this.Text = "";
                    //this.Lock = false;

                }

            }
            catch (Exception)
            {
                this.Lock = true;
                this.Text = "";
                this.Lock = false;
                throw;
            }

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

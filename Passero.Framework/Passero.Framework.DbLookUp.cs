using Passero.Framework;
using System.Collections.Generic;
using System;
using Wisej.Web;
using System.Data;
using Dapper;
using System.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Passero.Framework
{
    public class DbLookUp<ModelClass> where ModelClass : class
    {
        public BindingSource BindingSource { get; set; } = null;
        public DataBindingMode DataBindingMode { get; set; } = DataBindingMode.Passero;
        public IDbConnection DbConnection { get; set; }
        public Dictionary<string, DataBindControl> DataBindControls { get; set; } = new Dictionary<string, DataBindControl>(StringComparer.InvariantCultureIgnoreCase);
        public  DynamicParameters  DbParameters {get; set; } = new DynamicParameters();
        public string SQLQuery { get; set; } = "";
        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult();
        public ModelClass Model { get; private set; }= Activator.CreateInstance<ModelClass>();
        public DbLookUp()
        {
            
        }

        public DbLookUp(IDbConnection DbConnection)
        {
            this.DbConnection = DbConnection;
        }

        public bool Lookup()
        {
            LastExecutionResult.Reset();
            bool result = false;    
            try
            {
                this.Model = Activator.CreateInstance<ModelClass>();
                this.Model = this.DbConnection.Query<ModelClass>(this.SQLQuery, (object)this.DbParameters).FirstOrDefault<ModelClass>();
                if (this.Model== null) 
                {
                    this.Model=Activator .CreateInstance<ModelClass>(); 
                }
                else
                {
                    result = true;
                }
                // Databinding
                switch (this.DataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero:
                        this.WriteControls();
                        break;
                    case DataBindingMode.BindingSource:
                        if (this.BindingSource == null)
                            break;
                        this.BindingSource .DataSource =Model;
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                LastExecutionResult .ResultCode = ExecutionResult.eResultCode.Failed;
                LastExecutionResult .ErrorCode = 1;
                LastExecutionResult.ResultMessage = ex.Message;
                LastExecutionResult.Exception = ex;
                 
            }


            return result;
        }

        private string GetBoundControlKey(Control Control, string PropertyName)
        {
            string objname = Conversions.ToString(Interaction.CallByName(Control, "Name", CallType.Get, (object[])null));
            return (objname + "|" + PropertyName.Trim()).ToLower();
        }


        // Scrive il valore della proprietà del Model nella proprietà del controllo
        public int WriteControl(ModelClass Model, Control Control, string ControlPropertyName = "")
        {

            if (DataBindingMode == DataBindingMode.BindingSource | DataBindingMode == DataBindingMode.None)
            {
                return 0;
            }
            int _writedcontrols = 0;

            if (Control is null | Model is null)
            {
                return _writedcontrols;
            }
            string keytofind = GetBoundControlKey(Control, ControlPropertyName);
            List<string> keys;
            keys = DataBindControls.Keys.Where(key => key.StartsWith(keytofind)).ToList();
            if (keys.Count > 0)
            {
                foreach (string key in keys)
                {
                    var DataBindControl = DataBindControls[key];
                    var Value = Interaction.CallByName(Model, DataBindControl.ModelPropertyName, CallType.Get, (object[])null);
                    if (Value is not null)
                    {
                        Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Set, Value);
                    }
                    else
                    {
                        Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Set, "");
                    }

                    _writedcontrols += 1;
                }
            }
            //this.OnWriteControlsdCompleted(new EventArgs());
            return _writedcontrols;

        }


        public int WriteControls(ModelClass Model = null)
        {
            int _writedcontrols = 0;

            if (Model is null)
            {
                Model = this.Model;
            }

            switch (DataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    foreach (DataBindControl Control in DataBindControls.Values)
                    {
                        _writedcontrols = _writedcontrols + WriteControl(Model, Control.Control);
                    }
                    break;
                case DataBindingMode.BindingSource:

                    break;
                default:
                    break;
            }



            return _writedcontrols;

        }


        public bool AddControl(Control Control, string ControlPropertyName, string ModelPropertyName, BindingBehaviour BindingBehaviour = (BindingBehaviour)((int)BindingBehaviour.Insert + (int)BindingBehaviour.Update + (int)BindingBehaviour.Select))
        {
            string Key = GetBoundControlKey(Control, ControlPropertyName);

            var _DataBindControl = new DataBindControl();
            _DataBindControl.Control = Control;
            _DataBindControl.ControlPropertyName = ControlPropertyName;
            _DataBindControl.ModelPropertyName = ModelPropertyName;
            _DataBindControl.BindingBehaviour = BindingBehaviour;

            DataBindControls[Key] = _DataBindControl;

            if (DataBindingMode == DataBindingMode.BindingSource)
            {
                //this.RemoveControl(Control,ControlPropertyName);
                Control.DataBindings.Clear();
                Control.DataBindings.Add(ControlPropertyName, BindingSource, ModelPropertyName);
            }

            return true;
            // Else
            // Return False
            // End If
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


                if (DataBindingMode == DataBindingMode.BindingSource)
                {
                    DataBindControls[Key].Control.DataBindings.Clear();
                }                    
                
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
                    if (DataBindingMode == DataBindingMode.BindingSource)
                    {
                        DataBindControls[key].Control.DataBindings.Clear();
                    }
                    DataBindControls.Remove(key);
                    removedobjects += 1;
                }
            }

            return removedobjects;
        }



    }
}
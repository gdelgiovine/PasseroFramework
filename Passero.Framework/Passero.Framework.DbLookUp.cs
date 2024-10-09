using Dapper;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Wisej.Web;

namespace Passero.Framework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ModelClass">The type of the odel class.</typeparam>
    public class DbLookUp<ModelClass> where ModelClass : class
    {
        /// <summary>
        /// Gets or sets the binding source.
        /// </summary>
        /// <value>
        /// The binding source.
        /// </value>
        public BindingSource BindingSource { get; set; } = null;
        /// <summary>
        /// Gets or sets the data binding mode.
        /// </summary>
        /// <value>
        /// The data binding mode.
        /// </value>
        public DataBindingMode DataBindingMode { get; set; } = DataBindingMode.Passero;
        /// <summary>
        /// Gets or sets the database connection.
        /// </summary>
        /// <value>
        /// The database connection.
        /// </value>
        public IDbConnection DbConnection { get; set; }
        /// <summary>
        /// Gets or sets the data bind controls.
        /// </summary>
        /// <value>
        /// The data bind controls.
        /// </value>
        public Dictionary<string, DataBindControl> DataBindControls { get; set; } = new Dictionary<string, DataBindControl>(StringComparer.InvariantCultureIgnoreCase);
        /// <summary>
        /// Gets or sets the database parameters.
        /// </summary>
        /// <value>
        /// The database parameters.
        /// </value>
        public DynamicParameters DbParameters { get; set; } = new DynamicParameters();
        /// <summary>
        /// Gets or sets the SQL query.
        /// </summary>
        /// <value>
        /// The SQL query.
        /// </value>
        public string SQLQuery { get; set; } = "";
        /// <summary>
        /// Gets or sets the last execution result.
        /// </summary>
        /// <value>
        /// The last execution result.
        /// </value>
        public ExecutionResult LastExecutionResult { get; set; } = new ExecutionResult();
        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public ModelClass Model { get; private set; } = Activator.CreateInstance<ModelClass>();
        /// <summary>
        /// Initializes a new instance of the <see cref="DbLookUp{ModelClass}"/> class.
        /// </summary>
        public DbLookUp()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbLookUp{ModelClass}"/> class.
        /// </summary>
        /// <param name="DbConnection">The database connection.</param>
        public DbLookUp(IDbConnection DbConnection)
        {
            this.DbConnection = DbConnection;
        }

        /// <summary>
        /// Lookups this instance.
        /// </summary>
        /// <returns></returns>
        public bool Lookup()
        {
            LastExecutionResult.Reset();
            bool result = false;
            try
            {
                Model = Activator.CreateInstance<ModelClass>();
                Model = DbConnection.Query<ModelClass>(SQLQuery, DbParameters).FirstOrDefault<ModelClass>();
                if (Model == null)
                {
                    Model = Activator.CreateInstance<ModelClass>();
                }
                else
                {
                    result = true;
                }
                // Databinding
                switch (DataBindingMode)
                {
                    case DataBindingMode.None:
                        break;
                    case DataBindingMode.Passero:
                        WriteControls();
                        break;
                    case DataBindingMode.BindingSource:
                        if (BindingSource == null)
                            break;
                        BindingSource.DataSource = Model;
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {

                LastExecutionResult.ResultCode = ExecutionResultCodes.Failed;
                LastExecutionResult.ErrorCode = 1;
                LastExecutionResult.ResultMessage = ex.Message;
                LastExecutionResult.Exception = ex;

            }


            return result;
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


        // Scrive il valore della proprietà del Model nella proprietà del controllo
        /// <summary>
        /// Writes the control.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <returns></returns>
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


        /// <summary>
        /// Writes the controls.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <returns></returns>
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


        /// <summary>
        /// Adds the control.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <param name="ModelPropertyName">Name of the model property.</param>
        /// <param name="BindingBehaviour">The binding behaviour.</param>
        /// <returns></returns>
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
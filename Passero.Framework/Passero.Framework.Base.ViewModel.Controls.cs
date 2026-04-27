using Dapper;
using FastDeepCloner;
using Microsoft.Ajax.Utilities;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Wisej.Web;
using Wisej.Web.Data;

namespace Passero.Framework
{
    public partial class ViewModel<ModelClass> : INotifyPropertyChanged, INotifyPropertyChanging where ModelClass : class
    {
        /// <summary>
        /// Gets the bound control key.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="PropertyName">Name of the property.</param>
        /// <returns></returns>
        private string GetBoundControlKey(Control Control, string PropertyName)
        {
            //string objname = Conversions.ToString(Microsoft.VisualBasic.Interaction.CallByName(Control, "Name", CallType.Get, null));
            //return (objname + "|" + PropertyName.Trim()).ToLower();


            // Versione ottimizzata compatibile con .NET Framework 4.8
            if (Control == null)
                throw new ArgumentNullException(nameof(Control));
            if (string.IsNullOrEmpty(PropertyName))
                throw new ArgumentException("Property name cannot be null or empty", nameof(PropertyName));

            var controlName = Control.Name ?? string.Empty;
            var trimmedPropertyName = PropertyName.Trim();

            // Utilizzo StringBuilder per concatenazioni multiple
            var sb = new StringBuilder(controlName.Length + trimmedPropertyName.Length + 1);
            sb.Append(controlName)
              .Append('|')
              .Append(trimmedPropertyName);

            return sb.ToString().ToLowerInvariant();

        }


        /// <summary>
        /// Adds the control.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <param name="ModelPropertyName">Name of the model property.</param>
        /// <param name="BindingBehaviour">The binding behaviour.</param>
        /// <returns></returns>
        //public bool AddControl(Control Control, string ControlPropertyName, string ModelPropertyName, BindingBehaviour BindingBehaviour = (BindingBehaviour)((int)BindingBehaviour.Insert + (int)BindingBehaviour.Update + (int)BindingBehaviour.Select))
        //{
        //    string Key = GetBoundControlKey(Control, ControlPropertyName);

        //    var _DataBindControl = new DataBindControl();
        //    _DataBindControl.Control = Control;
        //    _DataBindControl.ControlPropertyName = ControlPropertyName;
        //    _DataBindControl.ModelPropertyName = ModelPropertyName;
        //    _DataBindControl.BindingBehaviour = BindingBehaviour;

        //    DataBindControls[Key] = _DataBindControl;

        //    if (DataBindControlsAutoSetMaxLenght == true)
        //    {
        //        if (Utilities.ObjectPropertyExist(_DataBindControl.Control, "MaxLength"))
        //        {
        //            if (Repository.DbObject.DbColumns.ContainsKey(ModelPropertyName))
        //            {
        //                int maxlength = Repository.DbObject.DbColumns[ModelPropertyName].DataColumn.MaxLength;
        //                Interaction.CallByName(_DataBindControl.Control, "MaxLength", CallType.Set, maxlength);
        //            }
        //        }
        //    }

        //    //if (mDataBindingMode == DataBindingMode.BindingSource)
        //    //{
        //    //    Control.DataBindings.Add(ControlPropertyName, mBindingSource , ModelPropertyName);
        //    //}

        //    return true;
        //    // Else
        //    // Return False
        //    // End If
        //}

        public bool AddControl(Control Control, string ControlPropertyName, string ModelPropertyName,
         BindingBehaviour BindingBehaviour = (BindingBehaviour)((int)BindingBehaviour.Insert + (int)BindingBehaviour.Update + (int)BindingBehaviour.Select))
        {
            string Key = GetBoundControlKey(Control, ControlPropertyName);

            var _DataBindControl = new DataBindControl();
            _DataBindControl.Control = Control;
            _DataBindControl.ControlPropertyName = ControlPropertyName;
            _DataBindControl.ModelPropertyName = ModelPropertyName;
            _DataBindControl.BindingBehaviour = BindingBehaviour;

            DataBindControls[Key] = _DataBindControl;

            if (DataBindControlsAutoSetMaxLenght == true && DapperRepository != null)
            {
                if (Utilities.ObjectPropertyExist(_DataBindControl.Control, "MaxLength"))
                {
                    if (DapperRepository.DbObject.DbColumns.ContainsKey(ModelPropertyName))
                    {
                        int maxlength = DapperRepository.DbObject.DbColumns[ModelPropertyName].DataColumn.MaxLength;
                        Interaction.CallByName(_DataBindControl.Control, "MaxLength", CallType.Set, maxlength);
                    }
                }
            }

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

                if (mDataBindingMode == DataBindingMode.BindingSource)
                {
                    DataBindControls[Key].Control.DataBindings.Clear();
                }
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
            string objname = Conversions.ToString(Interaction.CallByName(Control, "Name", CallType.Get, null));
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
                    if (mDataBindingMode == DataBindingMode.BindingSource)
                    {
                        DataBindControls[key].Control.DataBindings.Clear();
                    }
                    DataBindControls.Remove(key);
                    removedobjects += 1;
                }
            }

            return removedobjects;
        }





        // Scrive il valore della proprietà del Model nella proprietà del controllo
        /// <summary>
        /// Writes the control.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <returns></returns>
        /// 
        public int WriteControl(ModelClass Model, Control Control, string ControlPropertyName = "")
        {
            if (mDataBindingMode == DataBindingMode.None || Control is null || Model is null)
            {
                return 0;
            }

            int _writedcontrols = 0;
            string keytofind = GetBoundControlKey(Control, ControlPropertyName);

            foreach (var key in DataBindControls.Keys.Where(k => k.StartsWith(keytofind)))
            {
                if (DataBindControls.TryGetValue(key, out var DataBindControl))
                {
                    var Value = Interaction.CallByName(Model, DataBindControl.ModelPropertyName, CallType.Get, null);
                    Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Set, Value ?? "");
                    _writedcontrols++;
                }
            }

            return _writedcontrols;
        }
        public int WriteControl_OLD(ModelClass Model, Control Control, string ControlPropertyName = "")
        {
            //if (mDataBindingMode == DataBindingMode.BindingSource | mDataBindingMode == DataBindingMode.None)
            if (mDataBindingMode == DataBindingMode.None)
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
                    var Value = Interaction.CallByName(Model, DataBindControl.ModelPropertyName, CallType.Get, null);
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
                Model = ModelItem;
            }

            switch (mDataBindingMode)
            {
                case DataBindingMode.None:
                    break;
                case DataBindingMode.Passero:
                    foreach (DataBindControl Control in DataBindControls.Values)
                    {
                        //_writedcontrols = _writedcontrols + WriteControl(Model, Control.Control);
                        _writedcontrols += WriteControl(Model, Control.Control);
                    }
                    break;
                case DataBindingMode.BindingSource:
                    foreach (DataBindControl Control in DataBindControls.Values)
                    {
                        //_writedcontrols = _writedcontrols + WriteControl(Model, Control.Control);
                        _writedcontrols += WriteControl(Model, Control.Control);
                    }
                    break;
                default:
                    break;
            }
            return _writedcontrols;

        }

        // Scrive il valore della proprietà del Controllo nella proprietà del Model
        /// <summary>
        /// Reads the control.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <returns></returns>
        public int ReadControl(ModelClass Model, Control Control, string ControlPropertyName = "")
        {

            if (mDataBindingMode == DataBindingMode.BindingSource)
            {
                return 0;
            }

            int _readedcontrols = 0;
            if (Control is null)
            {
                return _readedcontrols;
            }


            string keytofind = GetBoundControlKey(Control, ControlPropertyName);
            List<string> keys;
            keys = DataBindControls.Keys.Where(key => key.StartsWith(keytofind)).ToList();
            if (keys.Count > 0)
            {
                foreach (string key in keys)
                {
                    var DataBindControl = DataBindControls[key];
                    if (((int)DataBindControl.BindingBehaviour & (int)BindingBehaviour.Insert + (int)BindingBehaviour.Update) > 0)
                    {
                        if (AddNewState == false)
                        {
                            var Value = Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Get, null);
                            Value = CheckControlValue(Value, DataBindControl.ModelPropertyName);
                            Interaction.CallByName(Model, DataBindControl.ModelPropertyName, CallType.Set, Value);
                            _readedcontrols += 1;
                        }
                        else if ((int)(DataBindControl.BindingBehaviour & BindingBehaviour.Insert) > 0)
                        {
                            var Value = Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Get, null);
                            Value = CheckControlValue(Value, DataBindControl.ModelPropertyName);
                            Interaction.CallByName(Model, DataBindControl.ModelPropertyName, CallType.Set, Value);
                            _readedcontrols += 1;
                        }
                    }

                }
            }
            return _readedcontrols;

        }
        /// <summary>
        /// Checks the control value.
        /// </summary>
        /// <param name="Value">The value.</param>
        /// <param name="ModelPropertyName">Name of the model property.</param>
        /// <returns></returns>
        //private object CheckControlValue(object Value, string ModelPropertyName)
        //{
        //    if (Value is not null)
        //    {
        //        switch (Value.GetType())
        //        {
        //            case var @case when @case == typeof(DateTime):
        //                {
        //                    DateTime d = Conversions.ToDate(Value);
        //                    if (d < MinDateTime | d > MaxDateTime)
        //                    {
        //                        Value = new DateTime?();
        //                    }

        //                    break;
        //                }
        //            case var case1 when case1 == typeof(string):
        //                {
        //                    if (mAutoFitColumnsLenght == true)
        //                    {
        //                        if (Repository.DbObject.DbColumns.ContainsKey(ModelPropertyName))
        //                        {
        //                            string s = Conversions.ToString(Value);
        //                            int maxlength = Repository.DbObject.DbColumns[ModelPropertyName].DataColumn.MaxLength;
        //                            if (s.Length > maxlength)
        //                            {
        //                                //s = s.Substring(0, maxlength);
        //                                // Sostituzione di Substring con AsSpan
        //                                s = s.AsSpan(0, maxlength).ToString();

        //                                Value = s;
        //                            }
        //                        }
        //                    }

        //                    break;
        //                }

        //            default:
        //                {
        //                    break;
        //                }

        //        }
        //    }

        //    return Value;
        //}

        private object CheckControlValue(object Value, string ModelPropertyName)
        {
            if (Value is not null)
            {
                switch (Value.GetType())
                {
                    case var @case when @case == typeof(DateTime):
                        {
                            DateTime d = Conversions.ToDate(Value);
                            if (d < MinDateTime | d > MaxDateTime)
                            {
                                Value = new DateTime?();
                            }
                            break;
                        }
                    case var case1 when case1 == typeof(string):
                        {
                            if (mAutoFitColumnsLenght == true && DapperRepository != null)
                            {
                                if (DapperRepository.DbObject.DbColumns.ContainsKey(ModelPropertyName))
                                {
                                    string s = Conversions.ToString(Value);
                                    int maxlength = DapperRepository.DbObject.DbColumns[ModelPropertyName].DataColumn.MaxLength;
                                    if (s.Length > maxlength)
                                    {
                                        s = s.AsSpan(0, maxlength).ToString();
                                        Value = s;
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            return Value;
        }

        //public ModelClass SetModelShadow()
        //{
        //    return Repository.SetModelShadow();
        //}

        /// <summary>
        /// Sets the model shadow.
        /// </summary>
        /// <returns></returns>
        public ModelClass SetModelShadow()
        {
            ModelItemShadow = Utilities.Clone(ModelItem);

            return ModelItemShadow;
        }

        /// <summary>
        /// Sets the model items shadow.
        /// </summary>
        /// <returns></returns>
        public IList<ModelClass> SetModelItemsShadow()
        {
            mModelItemsShadow = Utilities.Clone(mModelItems);

            return mModelItemsShadow;
        }
        /// <summary>
        /// Reads the controls.
        /// </summary>
        /// <param name="Model">The model.</param>
        /// <returns></returns>
        public int ReadControls(ModelClass Model = null)
        {

            if (mDataBindingMode == DataBindingMode.BindingSource | mDataBindingMode == DataBindingMode.None)
            {
                return 0;
            }

            if (Model is null)
            {
                Model = ModelItem;
            }

            int _readedcontrols = 0;
            foreach (DataBindControl Control in DataBindControls.Values)
                _readedcontrols = _readedcontrols + ReadControl(Model, Control.Control);
            return _readedcontrols;

        }

        /// <summary>
        /// Clears the control.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <returns></returns>
        public int ClearControl(Control Control, string ControlPropertyName = "")
        {

            int writedproperties = 0;

            if (Control is null)
            {
                return writedproperties;
            }

            string keytofind = GetBoundControlKey(Control, ControlPropertyName);
            List<string> keys;
            keys = DataBindControls.Keys.Where(key => key.StartsWith(keytofind)).ToList();
            if (keys.Count > 0)
            {
                foreach (string key in keys)
                {
                    var DataBindControl = DataBindControls[key];
                    object Value = null;
                    Interaction.CallByName(DataBindControl.Control, DataBindControl.ControlPropertyName, CallType.Set, Value);
                    writedproperties += 1;
                }
            }
            return writedproperties;

        }

        /// <summary>
        /// Clears the controls.
        /// </summary>
        /// <returns></returns>
        public int ClearControls()
        {
            int writedproperties = 0;
            foreach (DataBindControl Control in DataBindControls.Values)
                writedproperties = writedproperties + ClearControl(Control.Control);
            return writedproperties;
        }
    }
}

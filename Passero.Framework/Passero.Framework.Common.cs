using System;
using System.Collections.Generic;
using System.Data;
using Wisej.Web;

namespace Passero.Framework
{


    //    Public Class BasicDALMessageBox

    //    Public Sub Show(ByVal Text As String, Optional ByVal Caption As String = "Errore Gestione Dati")
    //        MessageBox.Show(Text, Caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
    //    End Sub

    //End Class
    /// <summary>
    /// 
    /// </summary>
    public class ErrorNotificationMessageBox
    {
        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="Text">The text.</param>
        /// <param name="Caption">The caption.</param>
        /// <param name="MessageBoxButtons">The message box buttons.</param>
        /// <param name="MessageBoxIcon">The message box icon.</param>
        public void Show(string Text, string Caption = "Passero.Framework Error Notification!", MessageBoxButtons MessageBoxButtons = MessageBoxButtons.OK, MessageBoxIcon MessageBoxIcon = MessageBoxIcon.Error)
        {
            MessageBox.Show(Text, Caption, MessageBoxButtons, MessageBoxIcon, MessageBoxDefaultButton.Button1, true, false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ModelClass">The type of the odel class.</typeparam>
    public class TargetModelItems<ModelBase>
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IList<ModelBase> Items { get; set; } = new List<ModelBase>();
    }



    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class DbColumn
    {
        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        /// <value>
        /// The name of the column.
        /// </value>
        public string ColumnName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is key.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is key; otherwise, <c>false</c>.
        /// </value>
        public bool IsKey { get; set; }
        /// <summary>
        /// Gets or sets the data column.
        /// </summary>
        /// <value>
        /// The data column.
        /// </value>
        public DataColumn DataColumn { get; set; }


    }

    /// <summary>
    /// 
    /// </summary>
    public class DataBindControl
    {
        /// <summary>
        /// The m class name
        /// </summary>
        private const string mClassName = "Passero.Framework.Base.DataBindControl";
        /// <summary>
        /// Gets or sets the control.
        /// </summary>
        /// <value>
        /// The control.
        /// </value>
        public Control Control { get; set; }
        /// <summary>
        /// Gets or sets the name of the control property.
        /// </summary>
        /// <value>
        /// The name of the control property.
        /// </value>
        public string ControlPropertyName { get; set; }
        /// <summary>
        /// Gets or sets the binding behaviour.
        /// </summary>
        /// <value>
        /// The binding behaviour.
        /// </value>
        public BindingBehaviour BindingBehaviour { get; set; }
        /// <summary>
        /// Gets or sets the name of the model property.
        /// </summary>
        /// <value>
        /// The name of the model property.
        /// </value>
        public string ModelPropertyName { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBindControl"/> class.
        /// </summary>
        public DataBindControl()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBindControl"/> class.
        /// </summary>
        /// <param name="Control">The control.</param>
        /// <param name="ControlPropertyName">Name of the control property.</param>
        /// <param name="ModelPropertyName">Name of the model property.</param>
        /// <param name="BindingBehaviour">The binding behaviour.</param>
        public DataBindControl(Control Control, string ControlPropertyName, string ModelPropertyName, BindingBehaviour BindingBehaviour = BindingBehaviour.SelectInsertUpdate)
        {
            this.BindingBehaviour = BindingBehaviour;
            this.Control = Control;
            this.ControlPropertyName = ControlPropertyName;
            this.ModelPropertyName = ModelPropertyName;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            string objname = Control.GetType().Name;
            return (nameof(objname) + "|" + ControlPropertyName.Trim()).ToLower();
        }
    }

}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Wisej.Web;

namespace Passero.Framework
{


    //    Public Class BasicDALMessageBox

    //    Public Sub Show(ByVal Text As String, Optional ByVal Caption As String = "Errore Gestione Dati")
    //        MessageBox.Show(Text, Caption, MessageBoxButtons.OK, MessageBoxIcon.Error)
    //    End Sub

    //End Class
    public class ErrorNotificationMessageBox
    {
        public void Show(string Text, string Caption = "Passero.Framework Error Notification!", MessageBoxButtons MessageBoxButtons= MessageBoxButtons.OK , MessageBoxIcon MessageBoxIcon = MessageBoxIcon.Error  )
        {
            MessageBox.Show(Text, Caption, MessageBoxButtons, MessageBoxIcon,MessageBoxDefaultButton.Button1 ,true,false);
        }
    }

    public class TargetModelItems<ModelClass>
    {
        public List<ModelClass> Items { get; set; } = new List<ModelClass>();
    }


  
    [Serializable]
    public class DbColumn
    {
        public string ColumnName { get; set; }
        public bool IsKey { get; set; }
        public DataColumn DataColumn { get; set; }


    }

    public class DataBindControl
    {
        private const string mClassName = "Passero.Framework.Base.DataBindControl";
        public Control Control { get; set; }
        public string ControlPropertyName { get; set; }
        public BindingBehaviour BindingBehaviour { get; set; }
        public string ModelPropertyName { get; set; }
        public DataBindControl()
        {

        }
        public DataBindControl(Control Control, string ControlPropertyName, string ModelPropertyName, BindingBehaviour BindingBehaviour = BindingBehaviour.SelectInsertUpdate)
        {
            this.BindingBehaviour = BindingBehaviour;
            this.Control = Control;
            this.ControlPropertyName = ControlPropertyName;
            this.ModelPropertyName = ModelPropertyName;
        }

        public string GetKey()
        {
            string objname = Control.GetType().Name;
            return (nameof(objname) + "|" + ControlPropertyName.Trim()).ToLower();
        }
    }

}

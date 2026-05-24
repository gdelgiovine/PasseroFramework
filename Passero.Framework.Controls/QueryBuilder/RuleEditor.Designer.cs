using System.Drawing;
using System.Drawing;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    partial class RuleEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Wisej.NET Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._dragHandleButton = new Wisej.Web.Button();
            this.valueString = new Wisej.Web.TextBox();
            this.valueEnum = new Wisej.Web.ComboBox();
            this.value2String = new Wisej.Web.TextBox();
            this.value2Enum = new Wisej.Web.ComboBox();
            this._fieldComboBox = new Wisej.Web.ComboBox();
            this._operatorComboBox = new Wisej.Web.ComboBox();
            this._rightPanel = new Wisej.Web.FlowLayoutPanel();
            this.valueNumeric = new Wisej.Web.NumericUpDown();
            this.value2Numeric = new Wisej.Web.NumericUpDown();
            this.valueDateTime = new Wisej.Web.DateTimePicker();
            this.value2DateTime = new Wisej.Web.DateTimePicker();
            this.valueBoolean = new Wisej.Web.CheckBox();
            this.value2Boolean = new Wisej.Web.CheckBox();
            this._deleteButton = new Wisej.Web.Button();
            this._rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valueNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.value2Numeric)).BeginInit();
            this.SuspendLayout();
            // 
            // _dragHandleButton
            // 
            this._dragHandleButton.Location = new System.Drawing.Point(3, 3);
            this._dragHandleButton.Name = "_dragHandleButton";
            this._dragHandleButton.Size = new System.Drawing.Size(32, 30);
            this._dragHandleButton.TabIndex = 12;
            this._dragHandleButton.Text = "::";
            // 
            // valueString
            // 
            this._rightPanel.SetFillWeight(this.valueString, 30);
            this.valueString.Location = new System.Drawing.Point(256, 3);
            this.valueString.Name = "valueString";
            this.valueString.Size = new System.Drawing.Size(110, 30);
            this.valueString.TabIndex = 2;
            this.valueString.Visible = false;
            // 
            // valueEnum
            // 
            this.valueEnum.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.valueEnum.Location = new System.Drawing.Point(487, 3);
            this.valueEnum.Name = "valueEnum";
            this.valueEnum.Size = new System.Drawing.Size(75, 30);
            this.valueEnum.TabIndex = 4;
            this.valueEnum.Visible = false;
            // 
            // value2String
            // 
            this._rightPanel.SetFillWeight(this.value2String, 30);
            this.value2String.Location = new System.Drawing.Point(372, 3);
            this.value2String.Name = "value2String";
            this.value2String.Size = new System.Drawing.Size(109, 30);
            this.value2String.TabIndex = 3;
            this.value2String.Visible = false;
            // 
            // value2Enum
            // 
            this.value2Enum.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.value2Enum.Location = new System.Drawing.Point(568, 3);
            this.value2Enum.Name = "value2Enum";
            this.value2Enum.Size = new System.Drawing.Size(75, 30);
            this.value2Enum.TabIndex = 5;
            this.value2Enum.Visible = false;
            // 
            // _fieldComboBox
            // 
            this._fieldComboBox.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this._fieldComboBox.Location = new System.Drawing.Point(3, 3);
            this._fieldComboBox.Name = "_fieldComboBox";
            this._fieldComboBox.Size = new System.Drawing.Size(105, 30);
            this._fieldComboBox.TabIndex = 0;
            // 
            // _operatorComboBox
            // 
            this._operatorComboBox.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this._operatorComboBox.Location = new System.Drawing.Point(114, 3);
            this._operatorComboBox.Name = "_operatorComboBox";
            this._operatorComboBox.Size = new System.Drawing.Size(136, 30);
            this._operatorComboBox.TabIndex = 1;
            // 
            // _rightPanel
            // 
            this._rightPanel.Controls.Add(this._dragHandleButton);
            this._rightPanel.Controls.Add(this._fieldComboBox);
            this._rightPanel.Controls.Add(this._operatorComboBox);
            this._rightPanel.Controls.Add(this.valueString);
            this._rightPanel.Controls.Add(this.value2String);
            this._rightPanel.Controls.Add(this.valueEnum);
            this._rightPanel.Controls.Add(this.value2Enum);
            this._rightPanel.Controls.Add(this.valueNumeric);
            this._rightPanel.Controls.Add(this.value2Numeric);
            this._rightPanel.Controls.Add(this.valueDateTime);
            this._rightPanel.Controls.Add(this.value2DateTime);
            this._rightPanel.Controls.Add(this.valueBoolean);
            this._rightPanel.Controls.Add(this.value2Boolean);
            this._rightPanel.Controls.Add(this._deleteButton);
            this._rightPanel.Dock = Wisej.Web.DockStyle.Fill;
            this._rightPanel.Location = new System.Drawing.Point(0, 0);
            this._rightPanel.Name = "_rightPanel";
            this._rightPanel.Size = new System.Drawing.Size(1132, 73);
            this._rightPanel.TabIndex = 1;
            // 
            // valueNumeric
            // 
            this._rightPanel.SetFillWeight(this.valueNumeric, 30);
            this.valueNumeric.Location = new System.Drawing.Point(649, 3);
            this.valueNumeric.Maximum = new decimal(((long)(9999999999)));
            this.valueNumeric.Name = "valueNumeric";
            this.valueNumeric.Size = new System.Drawing.Size(109, 30);
            this.valueNumeric.TabIndex = 6;
            this.valueNumeric.Visible = false;
            // 
            // value2Numeric
            // 
            this.value2Numeric.Anchor = Wisej.Web.AnchorStyles.Top;
            this._rightPanel.SetFillWeight(this.value2Numeric, 30);
            this.value2Numeric.Location = new System.Drawing.Point(764, 3);
            this.value2Numeric.Maximum = new decimal(((long)(9999999999)));
            this.value2Numeric.Name = "value2Numeric";
            this.value2Numeric.Size = new System.Drawing.Size(109, 30);
            this.value2Numeric.TabIndex = 7;
            this.value2Numeric.Visible = false;
            // 
            // valueDateTime
            // 
            this.valueDateTime.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.valueDateTime.Location = new System.Drawing.Point(879, 3);
            this.valueDateTime.Name = "valueDateTime";
            this.valueDateTime.Size = new System.Drawing.Size(122, 30);
            this.valueDateTime.TabIndex = 8;
            this.valueDateTime.Visible = false;
            // 
            // value2DateTime
            // 
            this.value2DateTime.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.value2DateTime.Format = Wisej.Web.DateTimePickerFormat.Short;
            this.value2DateTime.Location = new System.Drawing.Point(1007, 3);
            this.value2DateTime.Name = "value2DateTime";
            this.value2DateTime.Size = new System.Drawing.Size(122, 30);
            this.value2DateTime.TabIndex = 9;
            this.value2DateTime.Visible = false;
            // 
            // valueBoolean
            // 
            this.valueBoolean.Location = new System.Drawing.Point(3, 39);
            this.valueBoolean.Name = "valueBoolean";
            this.valueBoolean.Size = new System.Drawing.Size(30, 23);
            this.valueBoolean.TabIndex = 10;
            this.valueBoolean.Visible = false;
            // 
            // value2Boolean
            // 
            this.value2Boolean.Location = new System.Drawing.Point(39, 39);
            this.value2Boolean.Name = "value2Boolean";
            this.value2Boolean.Size = new System.Drawing.Size(30, 23);
            this.value2Boolean.TabIndex = 11;
            this.value2Boolean.Visible = false;
            // 
            // _deleteButton
            // 
            this._deleteButton.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Right)));
            this._rightPanel.SetFlowBreak(this._deleteButton, true);
            this._deleteButton.ForeColor = System.Drawing.Color.DarkRed;
            this._deleteButton.Location = new System.Drawing.Point(75, 39);
            this._deleteButton.Name = "_deleteButton";
            this._deleteButton.Size = new System.Drawing.Size(32, 30);
            this._deleteButton.TabIndex = 12;
            this._deleteButton.Text = "✕";
            // 
            // RuleEditor
            // 
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.Controls.Add(this._rightPanel);
            this.Margin = new Wisej.Web.Padding(0);
            this.Name = "RuleEditor";
            this.Size = new System.Drawing.Size(1134, 75);
            this._rightPanel.ResumeLayout(false);
            this._rightPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valueNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.value2Numeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Button _dragHandleButton;
        private Wisej.Web.ComboBox _fieldComboBox;
        private Wisej.Web.Button _deleteButton;
        private Wisej.Web.Control _value2Control;
        private Wisej.Web.Control _valueControl;
        private Wisej.Web.ComboBox _operatorComboBox;
        private Wisej.Web.FlowLayoutPanel _rightPanel;
        private TextBox valueString;
        private ComboBox valueEnum;
        private TextBox value2String;
        private ComboBox value2Enum;
        private NumericUpDown valueNumeric;
        private NumericUpDown value2Numeric;
        private CheckBox valueBoolean;
        private CheckBox value2Boolean;
        private DateTimePicker valueDateTime;
        private DateTimePicker value2DateTime;
    }
}

using System.Drawing;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    partial class GroupEditor
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
            this._headerPanel = new Wisej.Web.Panel();
            this._dragHandleButton = new Wisej.Web.Button();
            this._conditionComboBox = new Wisej.Web.ComboBox();
            this._addRuleButton = new Wisej.Web.Button();
            this._addGroupButton = new Wisej.Web.Button();
            this._deleteButton = new Wisej.Web.Button();
            this._childrenPanel = new Wisej.Web.FlowLayoutPanel();
            this._headerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _headerPanel
            // 
            this._headerPanel.Controls.Add(this._dragHandleButton);
            this._headerPanel.Controls.Add(this._conditionComboBox);
            this._headerPanel.Controls.Add(this._addRuleButton);
            this._headerPanel.Controls.Add(this._addGroupButton);
            this._headerPanel.Controls.Add(this._deleteButton);
            this._headerPanel.Dock = Wisej.Web.DockStyle.Top;
            this._headerPanel.Location = new System.Drawing.Point(8, 8);
            this._headerPanel.Name = "_headerPanel";
            this._headerPanel.Size = new System.Drawing.Size(438, 36);
            this._headerPanel.TabIndex = 1;
            // 
            // _dragHandleButton
            // 
            this._dragHandleButton.Location = new System.Drawing.Point(0, 3);
            this._dragHandleButton.Name = "_dragHandleButton";
            this._dragHandleButton.Size = new System.Drawing.Size(32, 30);
            this._dragHandleButton.TabIndex = 4;
            this._dragHandleButton.Text = "::";
            // 
            // _conditionComboBox
            // 
            this._conditionComboBox.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this._conditionComboBox.Location = new System.Drawing.Point(38, 3);
            this._conditionComboBox.Name = "_conditionComboBox";
            this._conditionComboBox.Size = new System.Drawing.Size(70, 30);
            this._conditionComboBox.TabIndex = 0;
            this._conditionComboBox.Text = "AND";
            // 
            // _addRuleButton
            // 
            this._addRuleButton.ImageSource = "resource.wx/Wisej.Ext.VisualStudioIcons/NewRule.svg";
            this._addRuleButton.Location = new System.Drawing.Point(118, 3);
            this._addRuleButton.Name = "_addRuleButton";
            this._addRuleButton.Size = new System.Drawing.Size(90, 30);
            this._addRuleButton.TabIndex = 1;
            this._addRuleButton.Text = "+ Regola";
            // 
            // _addGroupButton
            // 
            this._addGroupButton.ImageSource = "resource.wx/Wisej.Ext.VisualStudioIcons/AddGroupBox.svg";
            this._addGroupButton.Location = new System.Drawing.Point(213, 3);
            this._addGroupButton.Name = "_addGroupButton";
            this._addGroupButton.Size = new System.Drawing.Size(90, 30);
            this._addGroupButton.TabIndex = 2;
            this._addGroupButton.Text = "+ Gruppo";
            // 
            // _deleteButton
            // 
            this._deleteButton.ForeColor = System.Drawing.Color.DarkRed;
            this._deleteButton.ImageSource = "resource.wx/Wisej.Ext.VisualStudioIcons/DeleteGroupBox.svg";
            this._deleteButton.Location = new System.Drawing.Point(313, 3);
            this._deleteButton.Name = "_deleteButton";
            this._deleteButton.Size = new System.Drawing.Size(90, 30);
            this._deleteButton.TabIndex = 3;
            this._deleteButton.Text = "- Gruppo";
            // 
            // _childrenPanel
            // 
            this._childrenPanel.AutoSize = true;
            this._childrenPanel.AutoSizeMode = Wisej.Web.AutoSizeMode.GrowAndShrink;
            this._childrenPanel.Dock = Wisej.Web.DockStyle.Top;
            this._childrenPanel.FlowDirection = Wisej.Web.FlowDirection.TopDown;
            this._childrenPanel.Location = new System.Drawing.Point(8, 44);
            this._childrenPanel.Name = "_childrenPanel";
            this._childrenPanel.Size = new System.Drawing.Size(438, 0);
            this._childrenPanel.TabIndex = 0;
            this._childrenPanel.WrapContents = false;
            // 
            // GroupEditor
            // 
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(248, 248, 248);
            this.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.Controls.Add(this._childrenPanel);
            this.Controls.Add(this._headerPanel);
            this.Name = "GroupEditor";
            this.Padding = new Wisej.Web.Padding(8);
            this.Size = new System.Drawing.Size(456, 56);
            this._headerPanel.ResumeLayout(false);
            this._headerPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wisej.Web.Panel _headerPanel;
    private Wisej.Web.Button _dragHandleButton;
        private Wisej.Web.ComboBox _conditionComboBox;
        private Wisej.Web.Button _addRuleButton;
        private Wisej.Web.Button _addGroupButton;
        private Wisej.Web.Button _deleteButton;
        private Wisej.Web.FlowLayoutPanel _childrenPanel;
    }
}

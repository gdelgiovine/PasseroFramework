using System.Drawing;
using Wisej.Web;

namespace Passero.Framework.Controls;

partial class QueryBuilderControl
{
    private System.ComponentModel.IContainer components = null!;

    private ToolBar _toolBar;
    private ToolBarButton _toolBarButtonAddRule;
    private ToolBarButton _toolBarButtonAddGroup;
    private ToolBarButton _toolBarButtonSeparator;
    private ToolBarButton _toolBarButtonImportJson;
    private ToolBarButton _toolBarButtonExportJson;
    private FlowLayoutPanel flowLayoutPanelRoot;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent()
    {
            this._toolBar = new Wisej.Web.ToolBar();
            this.toolBarButtonAddRule = new Wisej.Web.ToolBarButton();
            this.toolBarButtonAddGroup = new Wisej.Web.ToolBarButton();
            this.toolBarButtonSeparator = new Wisej.Web.ToolBarButton();
            this.toolBarButtonImportJson = new Wisej.Web.ToolBarButton();
            this.toolBarButtonExportJson = new Wisej.Web.ToolBarButton();
            this.flowLayoutPanelRoot = new Wisej.Web.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // _toolBar
            // 
            this._toolBar.Buttons.AddRange(new Wisej.Web.ToolBarButton[] {
            this.toolBarButtonAddRule,
            this.toolBarButtonAddGroup,
            this.toolBarButtonSeparator,
            this.toolBarButtonImportJson,
            this.toolBarButtonExportJson});
            this._toolBar.Location = new System.Drawing.Point(0, 0);
            this._toolBar.Name = "_toolBar";
            this._toolBar.Size = new System.Drawing.Size(390, 51);
            this._toolBar.TabIndex = 1;
            this._toolBar.TabStop = false;
            // 
            // toolBarButtonAddRule
            // 
            this.toolBarButtonAddRule.Name = "toolBarButtonAddRule";
            this.toolBarButtonAddRule.Text = "Aggiungi regola";
            this.toolBarButtonAddRule.Visible = false;
            // 
            // toolBarButtonAddGroup
            // 
            this.toolBarButtonAddGroup.Name = "toolBarButtonAddGroup";
            this.toolBarButtonAddGroup.Text = "Aggiungi gruppo";
            this.toolBarButtonAddGroup.Visible = false;
            // 
            // toolBarButtonSeparator
            // 
            this.toolBarButtonSeparator.Name = "toolBarButtonSeparator";
            this.toolBarButtonSeparator.Style = Wisej.Web.ToolBarButtonStyle.Separator;
            this.toolBarButtonSeparator.Visible = false;
            // 
            // toolBarButtonImportJson
            // 
            this.toolBarButtonImportJson.ImageSource = "resource.wx/Wisej.Ext.VisualStudioIcons/AddQuery.svg";
            this.toolBarButtonImportJson.Name = "toolBarButtonImportJson";
            this.toolBarButtonImportJson.Text = "Load Query";
            // 
            // toolBarButtonExportJson
            // 
            this.toolBarButtonExportJson.ImageSource = "resource.wx/Wisej.Ext.VisualStudioIcons/Save.svg";
            this.toolBarButtonExportJson.Name = "toolBarButtonExportJson";
            this.toolBarButtonExportJson.Text = "Save Query";
            // 
            // flowLayoutPanelRoot
            // 
            this.flowLayoutPanelRoot.AutoScroll = true;
            this.flowLayoutPanelRoot.Dock = Wisej.Web.DockStyle.Fill;
            this.flowLayoutPanelRoot.FlowDirection = Wisej.Web.FlowDirection.TopDown;
            this.flowLayoutPanelRoot.Location = new System.Drawing.Point(0, 51);
            this.flowLayoutPanelRoot.Name = "flowLayoutPanelRoot";
            this.flowLayoutPanelRoot.Padding = new Wisej.Web.Padding(8, 0, 8, 0);
            this.flowLayoutPanelRoot.Size = new System.Drawing.Size(390, 82);
            this.flowLayoutPanelRoot.TabIndex = 0;
            this.flowLayoutPanelRoot.WrapContents = false;
            // 
            // QueryBuilderControl
            // 
            this.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.Controls.Add(this.flowLayoutPanelRoot);
            this.Controls.Add(this._toolBar);
            this.Name = "QueryBuilderControl";
            this.Size = new System.Drawing.Size(392, 135);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private ToolBarButton toolBarButtonAddRule;
    private ToolBarButton toolBarButtonAddGroup;
    private ToolBarButton toolBarButtonSeparator;
    private ToolBarButton toolBarButtonImportJson;
    private ToolBarButton toolBarButtonExportJson;
}

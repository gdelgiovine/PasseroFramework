using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Wisej.Web;

namespace Passero.Framework.Controls

{

    public partial class DataNavigator : UserControl
    {

        // UserControl overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Wisej Form Designer
#pragma warning disable CS0649 // Non è possibile assegnare un valore diverso al campo 'DataNavigator.components'. Il valore predefinito è null
        private System.ComponentModel.IContainer components;
#pragma warning restore CS0649 // Non è possibile assegnare un valore diverso al campo 'DataNavigator.components'. Il valore predefinito è null

        // NOTE: The following procedure is required by the Wisej Designer
        // It can be modified using the Wisej Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this._ToolBar = new Wisej.Web.ToolBar();
            this.bFirst = new Wisej.Web.ToolBarButton();
            this.bPrev = new Wisej.Web.ToolBarButton();
            this.RecordLabel = new Wisej.Web.ToolBarButton();
            this.bNext = new Wisej.Web.ToolBarButton();
            this.bLast = new Wisej.Web.ToolBarButton();
            this.bRefresh = new Wisej.Web.ToolBarButton();
            this.bNew = new Wisej.Web.ToolBarButton();
            this.bDelete = new Wisej.Web.ToolBarButton();
            this.bFind = new Wisej.Web.ToolBarButton();
            this.bPrint = new Wisej.Web.ToolBarButton();
            this.bUndo = new Wisej.Web.ToolBarButton();
            this.bSave = new Wisej.Web.ToolBarButton();
            this.bClose = new Wisej.Web.ToolBarButton();
            this.Panel = new Wisej.Web.Panel();
            this.Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _ToolBar
            // 
            this._ToolBar.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this._ToolBar.AutoSize = false;
            this._ToolBar.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this._ToolBar.Buttons.AddRange(new Wisej.Web.ToolBarButton[] {
            this.bFirst,
            this.bPrev,
            this.RecordLabel,
            this.bNext,
            this.bLast,
            this.bRefresh,
            this.bNew,
            this.bDelete,
            this.bFind,
            this.bPrint,
            this.bUndo,
            this.bSave,
            this.bClose});
            this._ToolBar.Dock = Wisej.Web.DockStyle.None;
            this._ToolBar.Font = new System.Drawing.Font("default", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this._ToolBar.Location = new System.Drawing.Point(0, 0);
            this._ToolBar.Margin = new Wisej.Web.Padding(0);
            this._ToolBar.Name = "_ToolBar";
            this._ToolBar.Size = new System.Drawing.Size(722, 55);
            this._ToolBar.TabIndex = 0;
            this._ToolBar.TabStop = false;
            // 
            // bFirst
            // 
            this.bFirst.AllowHtml = true;
            this.bFirst.ImageSource = "icon-first";
            this.bFirst.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bFirst.Name = "bFirst";
            this.bFirst.Text = "First<br>Shift-F6";
            this.bFirst.Click += new System.EventHandler(this.bFirst_Click);
            // 
            // bPrev
            // 
            this.bPrev.AllowHtml = true;
            this.bPrev.ImageSource = "icon-left";
            this.bPrev.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bPrev.Name = "bPrev";
            this.bPrev.Text = "Prev.<br>F6";
            this.bPrev.Click += new System.EventHandler(this.bPrev_Click);
            // 
            // RecordLabel
            // 
            this.RecordLabel.AllowHtml = true;
            this.RecordLabel.Enabled = false;
            this.RecordLabel.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.RecordLabel.Name = "RecordLabel";
            this.RecordLabel.Text = "0<br>0";
            // 
            // bNext
            // 
            this.bNext.AllowHtml = true;
            this.bNext.ImageSource = "icon-right";
            this.bNext.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bNext.Name = "bNext";
            this.bNext.Text = "Next<br>F7";
            this.bNext.Click += new System.EventHandler(this.bNext_Click);
            // 
            // bLast
            // 
            this.bLast.AllowHtml = true;
            this.bLast.ImageSource = "icon-last";
            this.bLast.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bLast.Name = "bLast";
            this.bLast.Text = "Last<br>Shift-F7";
            this.bLast.Click += new System.EventHandler(this.bLast_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.AllowHtml = true;
            this.bRefresh.ImageSource = "icon-redo";
            this.bRefresh.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Text = "Refresh dati<br>F5";
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bNew
            // 
            this.bNew.AllowHtml = true;
            this.bNew.ImageSource = "icon-new";
            this.bNew.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bNew.Name = "bNew";
            this.bNew.Text = "New<br>F2";
            this.bNew.Click += new System.EventHandler(this.bNew_Click);
            // 
            // bDelete
            // 
            this.bDelete.AllowHtml = true;
            this.bDelete.ImageSource = "tab-close";
            this.bDelete.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bDelete.Name = "bDelete";
            this.bDelete.Text = "Delete<br>F3";
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bFind
            // 
            this.bFind.AllowHtml = true;
            this.bFind.ImageSource = "icon-search";
            this.bFind.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bFind.Name = "bFind";
            this.bFind.Text = "Find<br>F4";
            this.bFind.Click += new System.EventHandler(this.bFind_Click);
            // 
            // bPrint
            // 
            this.bPrint.AllowHtml = true;
            this.bPrint.ImageSource = "icon-print";
            this.bPrint.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bPrint.Name = "bPrint";
            this.bPrint.Text = "Print<br>F8";
            this.bPrint.Click += new System.EventHandler(this.bPrint_Click);
            // 
            // bUndo
            // 
            this.bUndo.AllowHtml = true;
            this.bUndo.ImageSource = "icon-undo";
            this.bUndo.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bUndo.Name = "bUndo";
            this.bUndo.Text = "Undo<br>F9";
            this.bUndo.Click += new System.EventHandler(this.bUndo_Click);
            // 
            // bSave
            // 
            this.bSave.AllowHtml = true;
            this.bSave.ImageSource = "icon-save";
            this.bSave.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bSave.Name = "bSave";
            this.bSave.Text = "Save<br>F10";
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bClose
            // 
            this.bClose.AllowHtml = true;
            this.bClose.ImageSource = "icon-exit";
            this.bClose.Margin = new Wisej.Web.Padding(0, -5, 0, 0);
            this.bClose.Name = "bClose";
            this.bClose.Text = "<p style=\'margin-top: 0px;line-height:1.2;text-align:center;\'>Close<br>F12</p>";
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // Panel
            // 
            this.Panel.Controls.Add(this._ToolBar);
            this.Panel.Font = new System.Drawing.Font("default", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Panel.HeaderSize = 14;
            this.Panel.Location = new System.Drawing.Point(0, 0);
            this.Panel.Margin = new Wisej.Web.Padding(0);
            this.Panel.Name = "Panel";
            this.Panel.ShowCloseButton = false;
            this.Panel.ShowHeader = true;
            this.Panel.Size = new System.Drawing.Size(763, 70);
            this.Panel.TabIndex = 1;
            this.Panel.Text = "DataNavigator";
            // 
            // DataNavigator
            // 
            this.Controls.Add(this.Panel);
            this.Name = "DataNavigator";
            this.Size = new System.Drawing.Size(794, 70);
            this.Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        internal ToolBarButton bFirst;
        internal ToolBarButton bPrev;
        internal ToolBarButton bNext;
        internal ToolBarButton bLast;
        internal ToolBarButton bRefresh;
        internal ToolBarButton bNew;
        internal ToolBarButton bDelete;
        internal ToolBarButton bFind;
        internal ToolBarButton bUndo;
        internal ToolBarButton bSave;
        internal ToolBarButton bPrint;
        internal ToolBarButton bClose;
        internal ToolBarButton RecordLabel;

        public virtual ToolBar ToolBar
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ToolBar;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _ToolBar = value;
            }
        }
        private ToolBar _ToolBar;
        public Panel Panel;
    }
}
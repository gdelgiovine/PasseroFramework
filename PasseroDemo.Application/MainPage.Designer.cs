namespace PasseroDemo.Application
{
    partial class MainPage
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
            Wisej.Web.ComponentTool componentTool1 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool2 = new Wisej.Web.ComponentTool();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
            this.NavigationBar = new Wisej.Web.Ext.NavigationBar.NavigationBar();
            this.mnuAuthors = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.mnuPublishers = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.mnuPubInfo = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.mnuDiscounts = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.mnuEmployees = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.mnuJobs = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.mnuRoyalties = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.mnuTitles = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.mnuStores = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.mnuSales = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.mnuLanguage = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.mnuItaliano = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.mnuInglese = new Wisej.Web.Ext.NavigationBar.NavigationBarItem();
            this.ToolBar = new Wisej.Web.ToolBar();
            this.tbMenuOwerFlow = new Wisej.Web.ToolBarButton();
            this.tbAppLogo = new Wisej.Web.ToolBarControl();
            this.pbAppLogo = new Wisej.Web.PictureBox();
            this.tbAppTitle = new Wisej.Web.ToolBarControl();
            this.lbAppTitle = new Wisej.Web.Label();
            this.txtSearchBox = new Wisej.Web.TextBox();
            this.pbGDGLogo = new Wisej.Web.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbAppLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGDGLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // NavigationBar
            // 
            this.NavigationBar.ItemHeight = 32;
            this.NavigationBar.Items.AddRange(new Wisej.Web.Ext.NavigationBar.NavigationBarItem[] {
            this.mnuAuthors,
            this.mnuPublishers,
            this.mnuPubInfo,
            this.mnuDiscounts,
            this.mnuEmployees,
            this.mnuJobs,
            this.mnuRoyalties,
            this.mnuTitles,
            this.mnuStores,
            this.mnuSales,
            this.mnuLanguage});
            this.NavigationBar.Location = new System.Drawing.Point(14, 46);
            this.NavigationBar.Logo = "Images\\Passero.png";
            this.NavigationBar.Margin = new Wisej.Web.Padding(0);
            this.NavigationBar.Name = "NavigationBar";
            this.NavigationBar.ResizableEdges = Wisej.Web.AnchorStyles.Right;
            this.NavigationBar.ShowHeader = false;
            this.NavigationBar.ShowUser = false;
            this.NavigationBar.Size = new System.Drawing.Size(173, 399);
            this.NavigationBar.TabIndex = 0;
            this.NavigationBar.Text = "Passero Pubs Demo";
            this.NavigationBar.ItemClick += new Wisej.Web.Ext.NavigationBar.NavigationBarItemClickEventHandler(this.NavigationBar_ItemClick);
            this.NavigationBar.Resize += new System.EventHandler(this.NavigationBar_Resize);
            // 
            // mnuAuthors
            // 
            this.mnuAuthors.Icon = "table-row-editing";
            this.mnuAuthors.Name = "mnuAuthors";
            this.mnuAuthors.Text = "Authors";
            // 
            // mnuPublishers
            // 
            this.mnuPublishers.Icon = "icon-print";
            this.mnuPublishers.Name = "mnuPublishers";
            this.mnuPublishers.Text = "Publishers";
            // 
            // mnuPubInfo
            // 
            this.mnuPubInfo.Icon = "resource.wx/Wisej.Ext.BootstrapIcons/info-circle.svg";
            this.mnuPubInfo.Name = "mnuPubInfo";
            this.mnuPubInfo.Text = "Pub Info";
            // 
            // mnuDiscounts
            // 
            this.mnuDiscounts.Icon = "resource.wx/Wisej.Ext.BootstrapIcons/piggy-bank.svg";
            this.mnuDiscounts.Name = "mnuDiscounts";
            this.mnuDiscounts.Text = "Discounts";
            // 
            // mnuEmployees
            // 
            this.mnuEmployees.Icon = "resource.wx/Wisej.Ext.BootstrapIcons/people.svg";
            this.mnuEmployees.Name = "mnuEmployees";
            this.mnuEmployees.Text = "Employees";
            // 
            // mnuJobs
            // 
            this.mnuJobs.Icon = "resource.wx/Wisej.Ext.BootstrapIcons/briefcase.svg";
            this.mnuJobs.Name = "mnuJobs";
            this.mnuJobs.Text = "Jobs";
            // 
            // mnuRoyalties
            // 
            this.mnuRoyalties.Icon = "resource.wx/Wisej.Ext.BootstrapIcons/percent.svg";
            this.mnuRoyalties.Name = "mnuRoyalties";
            this.mnuRoyalties.Text = "Royalties";
            // 
            // mnuTitles
            // 
            this.mnuTitles.Icon = "resource.wx/Wisej.Ext.BootstrapIcons/bootstrap-fill.svg";
            this.mnuTitles.Name = "mnuTitles";
            this.mnuTitles.Text = "Titles";
            // 
            // mnuStores
            // 
            this.mnuStores.Icon = "resource.wx/Wisej.Ext.BootstrapIcons/shop-window.svg";
            this.mnuStores.Name = "mnuStores";
            this.mnuStores.Text = "Stores";
            // 
            // mnuSales
            // 
            this.mnuSales.Icon = "resource.wx/Wisej.Ext.BootstrapIcons/cart.svg";
            this.mnuSales.Name = "mnuSales";
            this.mnuSales.Text = "Sales";
            // 
            // mnuLanguage
            // 
            this.mnuLanguage.Icon = "icon-columns";
            this.mnuLanguage.Items.AddRange(new Wisej.Web.Ext.NavigationBar.NavigationBarItem[] {
            this.mnuItaliano,
            this.mnuInglese});
            this.mnuLanguage.Name = "mnuLanguage";
            this.mnuLanguage.Text = "Language";
            // 
            // mnuItaliano
            // 
            this.mnuItaliano.Expanded = true;
            this.mnuItaliano.Name = "mnuItaliano";
            this.mnuItaliano.Text = "Italiano";
            // 
            // mnuInglese
            // 
            this.mnuInglese.Expanded = true;
            this.mnuInglese.Name = "mnuInglese";
            this.mnuInglese.Text = "Inglese";
            // 
            // ToolBar
            // 
            this.ToolBar.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.ToolBar.Buttons.AddRange(new Wisej.Web.ToolBarButton[] {
            this.tbMenuOwerFlow,
            this.tbAppLogo,
            this.tbAppTitle});
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(1218, 43);
            this.ToolBar.TabIndex = 1;
            this.ToolBar.TabStop = false;
            this.ToolBar.ButtonClick += new Wisej.Web.ToolBarButtonClickEventHandler(this.ToolBar_ButtonClick);
            // 
            // tbMenuOwerFlow
            // 
            this.tbMenuOwerFlow.ImageSource = "menu-overflow";
            this.tbMenuOwerFlow.Name = "tbMenuOwerFlow";
            this.tbMenuOwerFlow.Style = Wisej.Web.ToolBarButtonStyle.ToggleButton;
            // 
            // tbAppLogo
            // 
            this.tbAppLogo.Control = this.pbAppLogo;
            this.tbAppLogo.Name = "tbAppLogo";
            // 
            // pbAppLogo
            // 
            this.pbAppLogo.Enabled = false;
            this.pbAppLogo.ImageSource = "Images\\Passero.png";
            this.pbAppLogo.Location = new System.Drawing.Point(0, 0);
            this.pbAppLogo.Name = "pbAppLogo";
            this.pbAppLogo.Size = new System.Drawing.Size(40, 40);
            this.pbAppLogo.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            // 
            // tbAppTitle
            // 
            this.tbAppTitle.Control = this.lbAppTitle;
            this.tbAppTitle.Name = "tbAppTitle";
            // 
            // lbAppTitle
            // 
            this.lbAppTitle.BackColor = System.Drawing.Color.Transparent;
            this.lbAppTitle.Font = new System.Drawing.Font("default", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lbAppTitle.Location = new System.Drawing.Point(0, 11);
            this.lbAppTitle.Name = "lbAppTitle";
            this.lbAppTitle.Size = new System.Drawing.Size(81, 18);
            this.lbAppTitle.TabIndex = 2;
            this.lbAppTitle.Text = "PasseroPubs";
            this.lbAppTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSearchBox
            // 
            this.txtSearchBox.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Right)));
            this.txtSearchBox.Location = new System.Drawing.Point(978, 8);
            this.txtSearchBox.Name = "txtSearchBox";
            this.txtSearchBox.Size = new System.Drawing.Size(180, 30);
            this.txtSearchBox.TabIndex = 2;
            this.txtSearchBox.TabStop = false;
            componentTool1.ImageSource = "window-close";
            componentTool1.Name = "clear";
            componentTool1.ToolTipText = "Clear Search";
            componentTool2.ImageSource = "icon-search";
            componentTool2.Name = "search";
            componentTool2.ToolTipText = "Search";
            this.txtSearchBox.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool1,
            componentTool2});
            this.txtSearchBox.Watermark = "Search";
            // 
            // pbGDGLogo
            // 
            this.pbGDGLogo.Anchor = ((Wisej.Web.AnchorStyles)((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Right)));
            this.pbGDGLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbGDGLogo.Image")));
            this.pbGDGLogo.Location = new System.Drawing.Point(1163, 5);
            this.pbGDGLogo.Name = "pbGDGLogo";
            this.pbGDGLogo.Size = new System.Drawing.Size(51, 35);
            this.pbGDGLogo.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            this.pbGDGLogo.DoubleClick += new System.EventHandler(this.pbGDGLogo_DoubleClick);
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pbGDGLogo);
            this.Controls.Add(this.txtSearchBox);
            this.Controls.Add(this.ToolBar);
            this.Controls.Add(this.NavigationBar);
            this.Name = "MainPage";
            this.Size = new System.Drawing.Size(1218, 599);
            this.Load += new System.EventHandler(this.MaiPage_Load);
            this.Resize += new System.EventHandler(this.MaiPage_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbAppLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGDGLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Wisej.Web.Ext.NavigationBar.NavigationBar NavigationBar;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuAuthors;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuPublishers;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuTitles;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuLanguage;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuItaliano;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuInglese;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuDiscounts;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuEmployees;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuPubInfo;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuJobs;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuRoyalties;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuStores;
        private Wisej.Web.Ext.NavigationBar.NavigationBarItem mnuSales;
        private Wisej.Web.ToolBar ToolBar;
        private Wisej.Web.ToolBarButton tbMenuOwerFlow;
        private Wisej.Web.ToolBarControl tbAppLogo;
        private Wisej.Web.PictureBox pbAppLogo;
        private Wisej.Web.ToolBarControl tbAppTitle;
        private Wisej.Web.Label lbAppTitle;
        private Wisej.Web.TextBox txtSearchBox;
        private Wisej.Web.PictureBox pbGDGLogo;
    }
}

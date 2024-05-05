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
            this.SuspendLayout();
            // 
            // NavigationBar
            // 
            this.NavigationBar.Dock = Wisej.Web.DockStyle.Left;
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
            this.NavigationBar.Logo = "Images\\Passero.png";
            this.NavigationBar.Name = "NavigationBar";
            this.NavigationBar.ResizableEdges = Wisej.Web.AnchorStyles.Right;
            this.NavigationBar.ShowUser = false;
            this.NavigationBar.Size = new System.Drawing.Size(247, 674);
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
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.Controls.Add(this.NavigationBar);
            this.Name = "MainPage";
            this.Size = new System.Drawing.Size(1434, 674);
            this.Load += new System.EventHandler(this.MaiPage_Load);
            this.Resize += new System.EventHandler(this.MaiPage_Resize);
            this.ResumeLayout(false);

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
    }
}

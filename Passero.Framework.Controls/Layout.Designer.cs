namespace Passero.Framework.Controls
{
    partial class Layout
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
            Wisej.Web.ComponentTool componentTool5 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool6 = new Wisej.Web.ComponentTool();
            this._TitleBar = new Wisej.Web.Panel();
            this.TitleBarSearchTextBox = new Wisej.Web.TextBox();
            this.TitleBarLabel = new Wisej.Web.Label();
            this.TitleBarMenuOverFlow = new Wisej.Web.PictureBox();
            this.TitleBarLogo = new Wisej.Web.PictureBox();
            this._NavigationBar = new Wisej.Web.Ext.NavigationBar.NavigationBar();
            this._TitleBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TitleBarMenuOverFlow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitleBarLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // _TitleBar
            // 
            this._TitleBar.BackColor = System.Drawing.SystemColors.ControlLight;
            this._TitleBar.Controls.Add(this.TitleBarSearchTextBox);
            this._TitleBar.Controls.Add(this.TitleBarLabel);
            this._TitleBar.Controls.Add(this.TitleBarMenuOverFlow);
            this._TitleBar.Controls.Add(this.TitleBarLogo);
            this._TitleBar.Location = new System.Drawing.Point(0, 0);
            this._TitleBar.Name = "_TitleBar";
            this._TitleBar.Size = new System.Drawing.Size(526, 50);
            this._TitleBar.TabIndex = 5;
            // 
            // TitleBarSearchTextBox
            // 
            this.TitleBarSearchTextBox.Location = new System.Drawing.Point(186, 12);
            this.TitleBarSearchTextBox.Name = "TitleBarSearchTextBox";
            this.TitleBarSearchTextBox.Size = new System.Drawing.Size(218, 30);
            this.TitleBarSearchTextBox.TabIndex = 7;
            this.TitleBarSearchTextBox.TabStop = false;
            componentTool5.ImageSource = "resource.wx/Wisej.Ext.BootstrapIcons/x-circle.svg";
            componentTool5.Name = "Clear";
            componentTool5.ToolTipText = "Pulisci testo";
            componentTool6.ImageSource = "icon-search";
            componentTool6.Name = "Search";
            componentTool6.ToolTipText = "Ricerca";
            this.TitleBarSearchTextBox.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool5,
            componentTool6});
            this.TitleBarSearchTextBox.Watermark = "Ricerca";
            this.TitleBarSearchTextBox.WordWrap = false;
            // 
            // TitleBarLabel
            // 
            this.TitleBarLabel.AutoSize = true;
            this.TitleBarLabel.Font = new System.Drawing.Font("defaultBold", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.TitleBarLabel.ForeColor = System.Drawing.Color.FromName("@windowText");
            this.TitleBarLabel.Location = new System.Drawing.Point(104, 12);
            this.TitleBarLabel.Name = "TitleBarLabel";
            this.TitleBarLabel.Size = new System.Drawing.Size(46, 27);
            this.TitleBarLabel.TabIndex = 6;
            this.TitleBarLabel.Text = "Title";
            this.TitleBarLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TitleBarMenuOverFlow
            // 
            this.TitleBarMenuOverFlow.Dock = Wisej.Web.DockStyle.Left;
            this.TitleBarMenuOverFlow.ImageSource = "menu-overflow";
            this.TitleBarMenuOverFlow.Location = new System.Drawing.Point(0, 0);
            this.TitleBarMenuOverFlow.Name = "TitleBarMenuOverFlow";
            this.TitleBarMenuOverFlow.Size = new System.Drawing.Size(28, 50);
            this.TitleBarMenuOverFlow.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            // 
            // TitleBarLogo
            // 
            this.TitleBarLogo.Location = new System.Drawing.Point(33, 7);
            this.TitleBarLogo.Name = "TitleBarLogo";
            this.TitleBarLogo.Size = new System.Drawing.Size(64, 38);
            this.TitleBarLogo.SizeMode = Wisej.Web.PictureBoxSizeMode.StretchImage;
            // 
            // _NavigationBar
            // 
            this._NavigationBar.Location = new System.Drawing.Point(33, 56);
            this._NavigationBar.Name = "_NavigationBar";
            this._NavigationBar.ResizableEdges = Wisej.Web.AnchorStyles.Right;
            this._NavigationBar.Size = new System.Drawing.Size(201, 437);
            this._NavigationBar.TabIndex = 6;
            this._NavigationBar.Text = "NavBar";
            this._NavigationBar.Resize += new System.EventHandler(this._NavigationBar_Resize);
            // 
            // Layout
            // 
            this.Controls.Add(this._NavigationBar);
            this.Controls.Add(this._TitleBar);
            this.Name = "Layout";
            this.Size = new System.Drawing.Size(565, 496);
            this.Load += new System.EventHandler(this.Layout_Load);
            this.Resize += new System.EventHandler(this.Layout_Resize);
            this._TitleBar.ResumeLayout(false);
            this._TitleBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TitleBarMenuOverFlow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TitleBarLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Wisej.Web.Panel _TitleBar;
        private Wisej.Web.PictureBox TitleBarMenuOverFlow;
        private Wisej.Web.PictureBox TitleBarLogo;
        private Wisej.Web.Ext.NavigationBar.NavigationBar _NavigationBar;
        public Wisej.Web.TextBox TitleBarSearchTextBox;
        public Wisej.Web.Label TitleBarLabel;
    }
}

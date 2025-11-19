using Passero.Framework.Controls;
using System;
using Wisej.Web;

namespace PasseroDemo.Views 
{
    partial class frmPubsInfo : Form
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

        #region Wisej Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Wisej.Web.ComponentTool componentTool4 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool5 = new Wisej.Web.ComponentTool();
            Wisej.Web.ComponentTool componentTool6 = new Wisej.Web.ComponentTool();
            this.bsPubsInfo = new Wisej.Web.BindingSource(this.components);
            this.dataNavigator1 = new Passero.Framework.Controls.DataNavigator();
            this.flowLayoutPanel1 = new Wisej.Web.FlowLayoutPanel();
            this.txt_pub_id = new Passero.Framework.Controls.DbLookUpTextBox(this.components);
            this.txt_pub_name = new Wisej.Web.TextBox();
            this.txt_pr_info = new Wisej.Web.TextBox();
            this.panelLogo = new Wisej.Web.Panel();
            this.uploadLogo = new Wisej.Web.Upload();
            this.pbLOGO = new Wisej.Web.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.bsPubsInfo)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.panelLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLOGO)).BeginInit();
            this.SuspendLayout();
            // 
            // bsPubsInfo
            // 
            this.bsPubsInfo.DataSource = typeof(PasseroDemo.Models.Pub_Info);
            // 
            // dataNavigator1
            // 
            this.dataNavigator1.Caption = "";
            this.dataNavigator1.DeleteMessage = "Ok to delete data?";
            this.dataNavigator1.Dock = Wisej.Web.DockStyle.Bottom;
            this.dataNavigator1.Language = "en";
            this.dataNavigator1.Location = new System.Drawing.Point(0, 396);
            this.dataNavigator1.Name = "dataNavigator1";
            this.dataNavigator1.SaveMessage = "Ok to save data ?";
            this.dataNavigator1.Size = new System.Drawing.Size(752, 60);
            this.dataNavigator1.TabIndex = 1;
            this.dataNavigator1.eFind += new Passero.Framework.Controls.DataNavigator.eFindEventHandler(this.dataNavigator1_eFind);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.txt_pub_id);
            this.flowLayoutPanel1.Controls.Add(this.txt_pub_name);
            this.flowLayoutPanel1.Controls.Add(this.txt_pr_info);
            this.flowLayoutPanel1.Controls.Add(this.panelLogo);
            this.flowLayoutPanel1.Controls.Add(this.uploadLogo);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 8);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new Wisej.Web.Padding(3);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(741, 382);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // txt_pub_id
            // 
            this.txt_pub_id.AutoSize = false;
            this.txt_pub_id.DataBindings.Add(new Wisej.Web.Binding("Value", this.bsPubsInfo, "pub_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_pub_id.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsPubsInfo, "pub_id", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.txt_pub_id.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_pub_id.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_pub_id.LabelText = "Pub ID";
            this.txt_pub_id.Location = new System.Drawing.Point(6, 6);
            this.txt_pub_id.Name = "txt_pub_id";
            this.txt_pub_id.Size = new System.Drawing.Size(150, 46);
            this.txt_pub_id.TabIndex = 0;
            componentTool4.ImageSource = "icon-search";
            componentTool4.Name = "Search";
            componentTool4.ToolTipText = "Search Publishers";
            this.txt_pub_id.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool4});
            // 
            // txt_pub_name
            // 
            this.txt_pub_name.BackColor = System.Drawing.Color.FromName("@tabHighlight");
            this.flowLayoutPanel1.SetFlowBreak(this.txt_pub_name, true);
            this.txt_pub_name.Label.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_pub_name.Label.Padding = new Wisej.Web.Padding(0, 0, 0, 3);
            this.txt_pub_name.LabelText = "Publisher";
            this.txt_pub_name.Location = new System.Drawing.Point(162, 6);
            this.txt_pub_name.Name = "txt_pub_name";
            this.txt_pub_name.ReadOnly = true;
            this.txt_pub_name.Size = new System.Drawing.Size(323, 46);
            this.txt_pub_name.TabIndex = 1;
            // 
            // txt_pr_info
            // 
            this.txt_pr_info.BackgroundImageLayout = Wisej.Web.ImageLayout.Center;
            this.txt_pr_info.DataBindings.Add(new Wisej.Web.Binding("Text", this.bsPubsInfo, "pr_info", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.flowLayoutPanel1.SetFillWeight(this.txt_pr_info, 50);
            this.txt_pr_info.Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txt_pr_info.LabelText = "PR Info";
            this.txt_pr_info.Location = new System.Drawing.Point(6, 58);
            this.txt_pr_info.Multiline = true;
            this.txt_pr_info.Name = "txt_pr_info";
            this.txt_pr_info.Size = new System.Drawing.Size(365, 285);
            this.txt_pr_info.TabIndex = 2;
            // 
            // panelLogo
            // 
            this.panelLogo.Anchor = ((Wisej.Web.AnchorStyles)(((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left)));
            this.panelLogo.AutoSizeMode = Wisej.Web.AutoSizeMode.GrowAndShrink;
            this.panelLogo.BackgroundImageLayout = Wisej.Web.ImageLayout.BestFit;
            this.panelLogo.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.panelLogo.Controls.Add(this.pbLOGO);
            this.flowLayoutPanel1.SetFillWeight(this.panelLogo, 50);
            this.flowLayoutPanel1.SetFlowBreak(this.panelLogo, true);
            this.panelLogo.Font = new System.Drawing.Font("default", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.panelLogo.HeaderSize = 19;
            this.panelLogo.Location = new System.Drawing.Point(374, 55);
            this.panelLogo.Margin = new Wisej.Web.Padding(0);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.ShowCloseButton = false;
            this.panelLogo.ShowHeader = true;
            this.panelLogo.Size = new System.Drawing.Size(364, 288);
            this.panelLogo.TabIndex = 3;
            this.panelLogo.Text = "Logo";
            componentTool5.ImageSource = "icon-upload";
            componentTool5.Name = "logo_upload";
            componentTool5.ToolTipText = "Upload logo image";
            componentTool6.ImageSource = "icon-close";
            componentTool6.Name = "logo_clear";
            componentTool6.ToolTipText = "Clear Logo Image";
            this.panelLogo.Tools.AddRange(new Wisej.Web.ComponentTool[] {
            componentTool5,
            componentTool6});
            this.panelLogo.ToolClick += new Wisej.Web.ToolClickEventHandler(this.panelLogo_ToolClick);
            // 
            // uploadLogo
            // 
            this.flowLayoutPanel1.SetFlowBreak(this.uploadLogo, true);
            this.uploadLogo.HideValue = true;
            this.uploadLogo.Location = new System.Drawing.Point(6, 349);
            this.uploadLogo.Name = "uploadLogo";
            this.uploadLogo.Size = new System.Drawing.Size(128, 30);
            this.uploadLogo.TabIndex = 4;
            this.uploadLogo.Text = "Upload Logo";
            this.uploadLogo.Visible = false;
            this.uploadLogo.Uploaded += new Wisej.Web.UploadedEventHandler(this.uploadLogo_Uploaded);
            // 
            // pbLOGO
            // 
            this.pbLOGO.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.pbLOGO.DataBindings.Add(new Wisej.Web.Binding("Image", this.bsPubsInfo, "logo", true, Wisej.Web.DataSourceUpdateMode.OnValidation, null, ""));
            this.pbLOGO.Location = new System.Drawing.Point(3, 3);
            this.pbLOGO.Name = "pbLOGO";
            this.pbLOGO.Size = new System.Drawing.Size(356, 261);
            this.pbLOGO.SizeMode = Wisej.Web.PictureBoxSizeMode.Zoom;
            // 
            // frmPubsInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 456);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataNavigator1);
            this.Name = "frmPubsInfo";
            this.Text = "Pubs Info";
            this.Load += new System.EventHandler(this.frmPubsInfo_Load);
            this.Resize += new System.EventHandler(this.frmPubsInfo_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.bsPubsInfo)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panelLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLOGO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Passero .Framework .Controls .DataNavigator  dataNavigator1;
        private BindingSource bsPubsInfo;
        private FlowLayoutPanel flowLayoutPanel1;
        internal DbLookUpTextBox txt_pub_id;
        private TextBox txt_pub_name;
        internal TextBox txt_pr_info;
        private Panel panelLogo;
        private Upload uploadLogo;
        private PictureBox pbLOGO;
    }
}

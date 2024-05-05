using System;
using System.Diagnostics;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    //[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class VideoImageAcquireForm : Form
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

        // Required by the Wisej Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Wisej Designer
        // It can be modified using the Wisej Designer.  
        // Do not modify it using the code editor.
        private void InitializeComponent()
        {
            var ComponentTool1 = new ComponentTool();
            PictureBoxFile = new PictureBox();
            btn_Upload = new Button();
            btn_Upload.Click += new EventHandler(btn_Upload_Click);
            Upload1 = new Upload();
            Upload1.Uploaded += new UploadedEventHandler(Upload1_Uploaded);
            btn_OK = new Button();
            btn_OK.Click += new EventHandler(btn_OK_Click);
            btn_Cancel = new Button();
            btn_Cancel.Click += new EventHandler(btn_Cancel_Click);
            TabControl = new TabControl();
            TabControl.SelectedIndexChanged += new EventHandler(TabControl_SelectedIndexChanged);
            TabPageFromFile = new TabPage();
            VideoFile = new Video();
            TabPageFromCamera = new TabPage();
            VideoCamera = new Video();
            PictureBoxCamera = new PictureBox();
            TabPageFromWeb = new TabPage();
            PanelFromWeb = new Panel();
            VideoWeb = new Video();
            PictureBoxWeb = new PictureBox();
            txt_SourceURL = new TextBox();
            txt_SourceURL.ToolClick += new ToolClickEventHandler(TextBox1_ToolClick);
            btn_OpenCamera = new Button();
            btn_OpenCamera.Click += new EventHandler(btn_OpenCamera_Click);
            btn_WebDownload = new Button();
            btn_WebDownload.Click += new EventHandler(btn_WebDownload_Click);
            ((System.ComponentModel.ISupportInitialize)PictureBoxFile).BeginInit();
            TabControl.SuspendLayout();
            TabPageFromFile.SuspendLayout();
            TabPageFromCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxCamera).BeginInit();
            TabPageFromWeb.SuspendLayout();
            PanelFromWeb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBoxWeb).BeginInit();
            SuspendLayout();
            // 
            // PictureBoxFile
            // 
            PictureBoxFile.BorderStyle = BorderStyle.Solid;
            PictureBoxFile.Location = new System.Drawing.Point(3, 3);
            PictureBoxFile.Name = "PictureBoxFile";
            PictureBoxFile.Size = new System.Drawing.Size(206, 155);
            PictureBoxFile.SizeMode = PictureBoxSizeMode.Zoom;
            // 
            // btn_Upload
            // 
            btn_Upload.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btn_Upload.ImageSource = "icon-upload";
            btn_Upload.Location = new System.Drawing.Point(5, 440);
            btn_Upload.Name = "btn_Upload";
            btn_Upload.Size = new System.Drawing.Size(96, 30);
            btn_Upload.TabIndex = 1;
            btn_Upload.Text = "Get File";
            // 
            // Upload1
            // 
            Upload1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Upload1.HideValue = true;
            Upload1.Location = new System.Drawing.Point(408, 439);
            Upload1.Name = "Upload1";
            Upload1.Size = new System.Drawing.Size(47, 30);
            Upload1.TabIndex = 2;
            Upload1.Text = "Upload from files";
            Upload1.Visible = false;
            // 
            // btn_OK
            // 
            btn_OK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btn_OK.Location = new System.Drawing.Point(567, 440);
            btn_OK.Name = "btn_OK";
            btn_OK.Size = new System.Drawing.Size(100, 30);
            btn_OK.TabIndex = 5;
            btn_OK.Text = "OK";
            // 
            // btn_Cancel
            // 
            btn_Cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btn_Cancel.DialogResult = DialogResult.Cancel;
            btn_Cancel.Location = new System.Drawing.Point(461, 440);
            btn_Cancel.Name = "btn_Cancel";
            btn_Cancel.Size = new System.Drawing.Size(100, 30);
            btn_Cancel.TabIndex = 4;
            btn_Cancel.Text = "Cancel";
            // 
            // TabControl
            // 
            TabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            TabControl.Controls.Add(TabPageFromFile);
            TabControl.Controls.Add(TabPageFromCamera);
            TabControl.Controls.Add(TabPageFromWeb);
            TabControl.Location = new System.Drawing.Point(4, 4);
            TabControl.Name = "TabControl";
            TabControl.PageInsets = new Padding(1, 40, 1, 1);
            TabControl.Size = new System.Drawing.Size(668, 431);
            TabControl.TabIndex = 1;
            // 
            // TabPageFromFile
            // 
            TabPageFromFile.Controls.Add(VideoFile);
            TabPageFromFile.Controls.Add(PictureBoxFile);
            TabPageFromFile.ImageSource = "icon-file";
            TabPageFromFile.Location = new System.Drawing.Point(1, 40);
            TabPageFromFile.Name = "TabPageFromFile";
            TabPageFromFile.Size = new System.Drawing.Size(666, 390);
            TabPageFromFile.Text = "From File";
            // 
            // VideoFile
            // 
            VideoFile.Location = new System.Drawing.Point(224, 3);
            VideoFile.Name = "VideoFile";
            VideoFile.Size = new System.Drawing.Size(214, 155);
            VideoFile.TabIndex = 2;
            VideoFile.Volume = 0.5d;
            // 
            // TabPageFromCamera
            // 
            TabPageFromCamera.Controls.Add(VideoCamera);
            TabPageFromCamera.Controls.Add(PictureBoxCamera);
            TabPageFromCamera.ImageSource = "window-icon";
            TabPageFromCamera.Location = new System.Drawing.Point(1, 40);
            TabPageFromCamera.Name = "TabPageFromCamera";
            TabPageFromCamera.Size = new System.Drawing.Size(666, 390);
            TabPageFromCamera.Text = "From Camera";
            // 
            // VideoCamera
            // 
            VideoCamera.Location = new System.Drawing.Point(337, 118);
            VideoCamera.Name = "VideoCamera";
            VideoCamera.Size = new System.Drawing.Size(214, 155);
            VideoCamera.TabIndex = 4;
            VideoCamera.Volume = 0.5d;
            // 
            // PictureBoxCamera
            // 
            PictureBoxCamera.BorderStyle = BorderStyle.Solid;
            PictureBoxCamera.Location = new System.Drawing.Point(116, 118);
            PictureBoxCamera.Name = "PictureBoxCamera";
            PictureBoxCamera.Size = new System.Drawing.Size(206, 155);
            PictureBoxCamera.SizeMode = PictureBoxSizeMode.Zoom;
            // 
            // TabPageFromWeb
            // 
            TabPageFromWeb.Controls.Add(PanelFromWeb);
            TabPageFromWeb.Controls.Add(txt_SourceURL);
            TabPageFromWeb.ImageSource = "icon-calendar";
            TabPageFromWeb.Location = new System.Drawing.Point(1, 40);
            TabPageFromWeb.Name = "TabPageFromWeb";
            TabPageFromWeb.Size = new System.Drawing.Size(666, 390);
            TabPageFromWeb.Text = "From Web";
            // 
            // PanelFromWeb
            // 
            PanelFromWeb.Controls.Add(VideoWeb);
            PanelFromWeb.Controls.Add(PictureBoxWeb);
            PanelFromWeb.Location = new System.Drawing.Point(4, 54);
            PanelFromWeb.Name = "PanelFromWeb";
            PanelFromWeb.Size = new System.Drawing.Size(658, 333);
            PanelFromWeb.TabIndex = 1;
            // 
            // VideoWeb
            // 
            VideoWeb.Location = new System.Drawing.Point(333, 89);
            VideoWeb.Name = "VideoWeb";
            VideoWeb.Size = new System.Drawing.Size(214, 155);
            VideoWeb.TabIndex = 4;
            VideoWeb.Volume = 0.5d;
            // 
            // PictureBoxWeb
            // 
            PictureBoxWeb.BorderStyle = BorderStyle.Solid;
            PictureBoxWeb.Location = new System.Drawing.Point(112, 89);
            PictureBoxWeb.Name = "PictureBoxWeb";
            PictureBoxWeb.Size = new System.Drawing.Size(206, 155);
            PictureBoxWeb.SizeMode = PictureBoxSizeMode.Zoom;
            // 
            // txt_SourceURL
            // 
            txt_SourceURL.Label.Font = new System.Drawing.Font("default", 8.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txt_SourceURL.Label.Padding = new Padding(0);
            txt_SourceURL.LabelText = "Source URL";
            txt_SourceURL.Location = new System.Drawing.Point(4, 4);
            txt_SourceURL.Name = "txt_SourceURL";
            txt_SourceURL.Size = new System.Drawing.Size(658, 45);
            txt_SourceURL.TabIndex = 0;
            ComponentTool1.ImageSource = "window-close";
            ComponentTool1.Name = "clear";
            txt_SourceURL.Tools.AddRange(new ComponentTool[] { ComponentTool1 });
            txt_SourceURL.Watermark = "Source URL";
            // 
            // btn_OpenCamera
            // 
            btn_OpenCamera.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btn_OpenCamera.Location = new System.Drawing.Point(107, 440);
            btn_OpenCamera.Name = "btn_OpenCamera";
            btn_OpenCamera.Size = new System.Drawing.Size(96, 30);
            btn_OpenCamera.TabIndex = 2;
            btn_OpenCamera.Text = "Open Camera";
            // 
            // btn_WebDownload
            // 
            btn_WebDownload.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btn_WebDownload.Location = new System.Drawing.Point(209, 440);
            btn_WebDownload.Name = "btn_WebDownload";
            btn_WebDownload.Size = new System.Drawing.Size(96, 30);
            btn_WebDownload.TabIndex = 3;
            btn_WebDownload.Text = "Download";
            // 
            // VideoImageAcquireForm
            // 
            ClientSize = new System.Drawing.Size(675, 477);
            Controls.Add(btn_WebDownload);
            Controls.Add(btn_OpenCamera);
            Controls.Add(TabControl);
            Controls.Add(btn_Upload);
            Controls.Add(btn_Cancel);
            Controls.Add(Upload1);
            Controls.Add(btn_OK);
            Name = "VideoImageAcquireForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Image Acquire";
            ((System.ComponentModel.ISupportInitialize)PictureBoxFile).EndInit();
            TabControl.ResumeLayout(false);
            TabPageFromFile.ResumeLayout(false);
            TabPageFromCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PictureBoxCamera).EndInit();
            TabPageFromWeb.ResumeLayout(false);
            TabPageFromWeb.PerformLayout();
            PanelFromWeb.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PictureBoxWeb).EndInit();
            Load += new EventHandler(ImageAcquire_Load);
            FormClosed += new FormClosedEventHandler(ImageAcquire_FormClosed);
            Closed += new EventHandler(CameraAcquire_Closed);
            ResumeLayout(false);

        }

        internal PictureBox PictureBoxFile;
        internal Button btn_Upload;
        internal Upload Upload1;
        internal Button btn_OK;
        internal Button btn_Cancel;
        internal TabControl TabControl;
        internal TabPage TabPageFromFile;
        internal TabPage TabPageFromCamera;
        internal Button btn_OpenCamera;
        internal Video VideoFile;
        internal TabPage TabPageFromWeb;
        internal Panel PanelFromWeb;
        internal TextBox txt_SourceURL;
        internal Button btn_WebDownload;
        internal Video VideoCamera;
        internal PictureBox PictureBoxCamera;
        internal Video VideoWeb;
        internal PictureBox PictureBoxWeb;
    }
}
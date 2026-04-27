using System;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    partial class CameraForm: Form
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
            this.Camera = new Wisej.Web.Ext.Camera.Camera();
            this.ToolBar = new Wisej.Web.ToolBar();
            this.tbPhoto = new Wisej.Web.ToolBarButton();
            this.tbVideo = new Wisej.Web.ToolBarButton();
            this.tbAudio = new Wisej.Web.ToolBarControl();
            this.tbMaxRecordTime = new Wisej.Web.ToolBarControl();
            this.sep2 = new Wisej.Web.ToolBarButton();
            this.tbFacing = new Wisej.Web.ToolBarControl();
            this.tbResolution = new Wisej.Web.ToolBarControl();
            this.sep3 = new Wisej.Web.ToolBarButton();
            this.tbGetFromCamera = new Wisej.Web.ToolBarButton();
            this.tbClose = new Wisej.Web.ToolBarButton();
            this.cmbResolution = new Wisej.Web.ComboBox();
            this.cmbFacing = new Wisej.Web.ComboBox();
            this.chkAudio = new Wisej.Web.CheckBox();
            this.StatusBar = new Wisej.Web.StatusBar();
            this.Timer = new Wisej.Web.Timer(this.components);
            this.txt_MaxRecordTime = new Wisej.Web.TextBox();
            this.sep1 = new Wisej.Web.ToolBarButton();
            this.SuspendLayout();
            // 
            // Camera
            // 
            this.Camera.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom) 
            | Wisej.Web.AnchorStyles.Left) 
            | Wisej.Web.AnchorStyles.Right)));
            this.Camera.FacingMode = Wisej.Web.Ext.Camera.Camera.VideoFacingMode.Environment;
            this.Camera.Location = new System.Drawing.Point(3, 60);
            this.Camera.Name = "Camera";
            this.Camera.Size = new System.Drawing.Size(309, 367);
            this.Camera.TabIndex = 0;
            this.Camera.Text = "Camera";
            this.Camera.Uploaded += new Wisej.Web.UploadedEventHandler(this.Camera_Uploaded);
            // 
            // ToolBar
            // 
            this.ToolBar.AutoSize = false;
            this.ToolBar.BorderStyle = Wisej.Web.BorderStyle.Solid;
            this.ToolBar.Buttons.AddRange(new Wisej.Web.ToolBarButton[] {
            this.tbPhoto,
            this.tbVideo,
            this.tbAudio,
            this.tbMaxRecordTime,
            this.sep2,
            this.tbFacing,
            this.tbResolution,
            this.sep3,
            this.tbGetFromCamera,
            this.sep1,
            this.tbClose});
            this.ToolBar.Font = new System.Drawing.Font("default", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(699, 57);
            this.ToolBar.TabIndex = 2;
            this.ToolBar.TabStop = false;
            // 
            // tbPhoto
            // 
            this.tbPhoto.ImageSource = "resource.wx/Wisej.Ext.BootstrapIcons/camera.svg";
            this.tbPhoto.Name = "tbPhoto";
            this.tbPhoto.Text = "Photo";
            this.tbPhoto.Click += new System.EventHandler(this.tbPhoto_Click);
            // 
            // tbVideo
            // 
            this.tbVideo.ImageSource = "resource.wx/Wisej.Ext.BootstrapIcons/camera-video.svg";
            this.tbVideo.Name = "tbVideo";
            this.tbVideo.Text = "Video";
            this.tbVideo.Click += new System.EventHandler(this.tbVideo_Click);
            // 
            // tbAudio
            // 
            this.tbAudio.Name = "tbAudio";
            // 
            // tbMaxRecordTime
            // 
            this.tbMaxRecordTime.Name = "tbMaxRecordTime";
            this.tbMaxRecordTime.Text = "Max Rec. Time";
            // 
            // sep2
            // 
            this.sep2.Name = "sep2";
            this.sep2.Style = Wisej.Web.ToolBarButtonStyle.Separator;
            // 
            // tbFacing
            // 
            this.tbFacing.Name = "tbFacing";
            this.tbFacing.Text = "Facing";
            // 
            // tbResolution
            // 
            this.tbResolution.Name = "tbResolution";
            this.tbResolution.Text = "Resolution";
            // 
            // sep3
            // 
            this.sep3.Name = "sep3";
            this.sep3.Style = Wisej.Web.ToolBarButtonStyle.Separator;
            // 
            // tbGetFromCamera
            // 
            this.tbGetFromCamera.ImageSource = "resource.wx/Wisej.Ext.BootstrapIcons/record-fill.svg?color=#FF0000";
            this.tbGetFromCamera.Name = "tbGetFromCamera";
            this.tbGetFromCamera.Text = "Take Photo";
            this.tbGetFromCamera.Click += new System.EventHandler(this.tbGetFromCamera_Click);
            // 
            // tbClose
            // 
            this.tbClose.Name = "tbClose";
            this.tbClose.SizeMode = Wisej.Web.ToolBarButtonSizeMode.Fill;
            this.tbClose.Text = "Cancel";
            this.tbClose.Click += new System.EventHandler(this.tbClose_Click);
            // 
            // cmbResolution
            // 
            this.cmbResolution.AutoSize = false;
            this.cmbResolution.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbResolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbResolution.Items.AddRange(new object[] {
            "640x480",
            "800x600",
            "1024x768",
            "1280x1024",
            "1600x1200",
            "1920x1080",
            "2048x1536",
            "2560x1920",
            "2816x2112",
            "3264x2468",
            "4200x2800"});
            this.cmbResolution.Location = new System.Drawing.Point(318, 91);
            this.cmbResolution.Name = "cmbResolution";
            this.cmbResolution.Size = new System.Drawing.Size(97, 30);
            this.cmbResolution.TabIndex = 3;
            this.cmbResolution.SelectedIndexChanged += new System.EventHandler(this.cmbResolution_SelectedIndexChanged);
            // 
            // cmbFacing
            // 
            this.cmbFacing.AutoSize = false;
            this.cmbFacing.DropDownStyle = Wisej.Web.ComboBoxStyle.DropDownList;
            this.cmbFacing.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbFacing.Items.AddRange(new object[] {
            "User",
            "Environment",
            "Left",
            "Right"});
            this.cmbFacing.Location = new System.Drawing.Point(318, 127);
            this.cmbFacing.Name = "cmbFacing";
            this.cmbFacing.Size = new System.Drawing.Size(97, 30);
            this.cmbFacing.TabIndex = 4;
            this.cmbFacing.SelectedIndexChanged += new System.EventHandler(this.cmbFacing_SelectedIndexChanged);
            // 
            // chkAudio
            // 
            this.chkAudio.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkAudio.Location = new System.Drawing.Point(320, 65);
            this.chkAudio.Name = "chkAudio";
            this.chkAudio.Size = new System.Drawing.Size(59, 18);
            this.chkAudio.TabIndex = 6;
            this.chkAudio.Text = "Audio";
            this.chkAudio.CheckedChanged += new System.EventHandler(this.chkAudio_CheckedChanged);
            // 
            // StatusBar
            // 
            this.StatusBar.Font = new System.Drawing.Font("default", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.StatusBar.Location = new System.Drawing.Point(0, 430);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(699, 22);
            this.StatusBar.TabIndex = 7;
            this.StatusBar.Text = "Camera is in view mode";
            // 
            // Timer
            // 
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // txt_MaxRecordTime
            // 
            this.txt_MaxRecordTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txt_MaxRecordTime.InputType.Max = "60";
            this.txt_MaxRecordTime.InputType.Min = "1";
            this.txt_MaxRecordTime.InputType.Mode = Wisej.Web.TextBoxMode.Numeric;
            this.txt_MaxRecordTime.InputType.Step = 1D;
            this.txt_MaxRecordTime.InputType.Type = Wisej.Web.TextBoxType.Number;
            this.txt_MaxRecordTime.Location = new System.Drawing.Point(318, 163);
            this.txt_MaxRecordTime.Name = "txt_MaxRecordTime";
            this.txt_MaxRecordTime.Size = new System.Drawing.Size(60, 30);
            this.txt_MaxRecordTime.TabIndex = 8;
            this.txt_MaxRecordTime.Text = "5";
            // 
            // sep1
            // 
            this.sep1.Name = "sep1";
            this.sep1.Style = Wisej.Web.ToolBarButtonStyle.Separator;
            // 
            // CameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = Wisej.Web.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 452);
            this.Controls.Add(this.txt_MaxRecordTime);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.chkAudio);
            this.Controls.Add(this.cmbFacing);
            this.Controls.Add(this.cmbResolution);
            this.Controls.Add(this.ToolBar);
            this.Controls.Add(this.Camera);
            this.IconSource = "resource.wx/Wisej.Ext.BootstrapIcons/camera-reels.svg";
            this.Name = "CameraForm";
            this.StartPosition = Wisej.Web.FormStartPosition.CenterScreen;
            this.Text = "Camera View";
            this.Load += new System.EventHandler(this.CameraImagePreview_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion
        internal Wisej.Web.Ext.Camera.Camera Camera;
        internal ToolBar ToolBar;

        internal ToolBarControl tbFacing;
        internal ToolBarControl tbResolution;
        internal ComboBox cmbResolution;
        internal ComboBox cmbFacing;
        internal ToolBarButton tbPhoto;
        internal ToolBarButton tbVideo;

        internal ToolBarButton tbGetFromCamera;
        internal ToolBarControl tbAudio;
        internal CheckBox chkAudio;
        internal ToolBarButton sep2;
        internal ToolBarButton sep3;
        internal StatusBar StatusBar;
        internal Timer Timer;
        internal ToolBarControl tbMaxRecordTime;
        internal TextBox txt_MaxRecordTime;
        internal ToolBarButton tbCancel;
        internal ToolBarButton tbClose;
        private ToolBarButton sep1;
    }
}
using System;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    public partial class CameraForm : Form
    {
        public System.Drawing.Image CameraImage { get; set; }
        public System.IO.StreamWriter CameraVideo { get; set; }
        public string RealFileName { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; } = "image/jpeg";
        public long ContentLenght { get; set; } = 0;
        public string VideoSourceURL { get; set; } = "";
        public AcquiredObjectTypes AcquiredObjectType { get; set; } = AcquiredObjectTypes.Null;
        public ObjectTypesToAcquire ObjectTypeToAcquire { get; set; } = ObjectTypesToAcquire.ImageOrVideo;
        public string ApplicationTempPath { get; set; } = "temp";
        public int MaxRecordTime { get; set; } = 5;
        public string TakePhotoText { get; set; } = "Take Photo";
        public string StartRecordVideoText { get; set; } = "Record Video";
        public string StopRecordVideoText { get; set; } = "Stop Record Video";
        public string StatusBarCameraRecordingText { get; set; } = "Camera is recording video.";
        public string StatusBarCameraViewModeText { get; set; } = "Camera is view mode.";

        public ToolBarButton VideoToolBarButton
        {
            get { return this.tbVideo; }
            set { this.tbVideo = value; }
        }

        public ToolBarButton PhotoToolBarButton
        {
            get { return this.tbPhoto; }
            set { this.tbPhoto = value; }
        }

        public ToolBarButton CancelToolBarButton
        {
            get { return this.tbCancel; }
            set { this.tbCancel = value; }
        }

        public ToolBarButton GetFromCameraToolBarButton
        {
            get { return this.tbGetFromCamera; }
            set { this.tbGetFromCamera = value; }
        }

        public ToolBarControl FacingToolBarControl
        {
            get { return this.tbFacing; }
            set { this.tbFacing = value; }
        }

        public ToolBarControl ResolutionToolBarControl
        {
            get { return this.tbResolution; }
            set { this.tbResolution = value; }
        }

        public ToolBarControl AudioToolBarControl
        {
            get { return this.tbAudio; }
            set { this.tbAudio = value; }
        }

        public ToolBarControl MaxRecordTimeToolBarControl
        {
            get { return this.tbMaxRecordTime; }
            set { this.tbMaxRecordTime = value; }
        }


        private bool mCameraRecording = false;
        private DateTime mStartRecordTime;
        private bool IsOk = false;
        private System.Drawing.Color tbPhotoForeColor;
#pragma warning disable CS0169 // Il campo 'CameraForm.tbPhotoBackColor' non viene mai usato
        private System.Drawing.Color tbPhotoBackColor;
#pragma warning restore CS0169 // Il campo 'CameraForm.tbPhotoBackColor' non viene mai usato
#pragma warning disable CS0649 // Non è possibile assegnare un valore diverso al campo 'CameraForm.tbVideoForeColor'. Il valore predefinito è
        private System.Drawing.Color tbVideoForeColor;
#pragma warning restore CS0649 // Non è possibile assegnare un valore diverso al campo 'CameraForm.tbVideoForeColor'. Il valore predefinito è
        private System.Drawing.Color tbVideoBackColor;


        public enum AcquiredObjectTypes : int
        {
            Null = 0,
            Image = 1,
            Video = 2
        }

        public enum ObjectTypesToAcquire : int
        {
            Image = 1,
            Video = 2,
            ImageOrVideo = 3
        }

        public CameraForm()
        {
            InitializeComponent();
        }

        private async void GetImage()
        {
            ContentType = "image/jpeg";
            if (string.IsNullOrEmpty(FileName))
            {
                FileName = Guid.NewGuid().ToString() + ".jpg";
            }
            else
            {
                string ext = System.IO.Path.GetExtension(FileName);
                if (string.IsNullOrEmpty(ext))
                {
                    FileName = FileName + ".jpg";
                }
                else
                {
                    FileName.Replace(ext, ".jpg");
                }
            }

            CameraImage = await this.Camera.GetImageAsync();
            RealFileName = Application.MapPath(ApplicationTempPath + @"\" + FileName);
            if (System.IO.Directory.Exists(Application.MapPath(ApplicationTempPath)) == false)
            {
                MessageBox.Show(string.Format("ApplicationTempPath {0} not exist!", ApplicationTempPath), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.AcquiredObjectType = AcquiredObjectTypes.Null;
                IsOk = false;
                this.Close();
                return;
            }

            if (System.IO.File.Exists(RealFileName))
            {
                System.IO.File.Delete(RealFileName);
            }

            try
            {
                CameraImage.Save(RealFileName);
                ContentLenght = new System.IO.FileInfo(RealFileName).Length;
                AcquiredObjectType = AcquiredObjectTypes.Image;
                IsOk = true;
                this.Close();
            }
            catch (Exception)
            {

                this.AcquiredObjectType = AcquiredObjectTypes.Null;
                IsOk = false;
                this.Close();
            }

        }

        private void StarRecordingVideo()
        {

            mStartRecordTime = DateTime.Now;
            this.Timer.Enabled = true;
            this.Timer.Start();
            this.Camera.StartRecording(ContentType, updateInterval: 1000);
        }

        private void CameraImagePreview_Load(object sender, EventArgs e)
        {
            this.Camera.Width = this.Width - this.Camera.Left * 2;
            this.txt_MaxRecordTime.Text = MaxRecordTime.ToString();
            this.tbFacing.Control = this.cmbFacing;
            this.tbResolution.Control = this.cmbResolution;
            this.tbAudio.Control = this.chkAudio;
            this.tbMaxRecordTime.Control = this.txt_MaxRecordTime;
            this.tbPhotoForeColor = this.tbPhoto.ForeColor;
            this.tbVideoBackColor = this.tbVideo.ForeColor;
            switch (ObjectTypeToAcquire)
            {
                case ObjectTypesToAcquire.Video:
                    {
                        this.tbVideo.Visible = true;
                        this.tbPhoto.Visible = false;
                        this.tbVideo.PerformClick();
                        break;
                    }

                case ObjectTypesToAcquire.Image:
                    {
                        this.tbVideo.Visible = false;
                        this.tbPhoto.Visible = true;
                        this.tbPhoto.PerformClick();
                        break;
                    }

                case ObjectTypesToAcquire.ImageOrVideo:
                    {
                        this.tbVideo.Visible = true;
                        this.tbPhoto.Visible = true;
                        this.tbPhoto.PerformClick();
                        break;
                    }

                default:
                    {
                        this.Close();
                        break;
                    }
            }

            this.Camera.ObjectFit = ObjectFit.Contain;
            string x = this.Camera.WidthCapture.ToString() + "x" + this.Camera.HeightCapture.ToString();
            if (this.cmbResolution.Items.Contains(x) == false)
            {
                this.cmbResolution.Items.Add(x);
            }

            this.cmbResolution.SelectedIndex = this.cmbResolution.Items.IndexOf(x);
            this.cmbFacing.SelectedIndex = 1;
        }

        private void cmbFacing_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Camera.FacingMode = (Wisej.Web.Ext.Camera.Camera.VideoFacingMode)this.cmbFacing.SelectedIndex;
        }

        private void cmbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCamera();
        }

        private void SetCameraResolution()
        {
            string[] res;
            string Item = Convert.ToString(this.cmbResolution.SelectedItem);
            if (!string.IsNullOrEmpty(Item))
            {
                res = Item.Split('x');
                this.Camera.HeightCapture = Convert.ToInt16(res[1]);
                this.Camera.WidthCapture = Convert.ToInt16(res[0]);
            }
        }

        private void SetCamera(bool audio = false)
        {
            string[] res;
            string Item = Convert.ToString(this.cmbResolution.SelectedItem);
            if (!string.IsNullOrEmpty(Item))
            {
                res = Item.Split('x');
                this.Camera.HeightCapture = Convert.ToInt16(res[1]);
                this.Camera.WidthCapture = Convert.ToInt16(res[0]);
                this.Camera.FacingMode = (Wisej.Web.Ext.Camera.Camera.VideoFacingMode)this.cmbFacing.SelectedIndex;
                this.Camera.Audio = audio;
            }
        }

        private void ManageToolBarControls(bool Enabled)
        {
            this.tbAudio.Enabled = Enabled;
            this.tbFacing.Enabled = Enabled;
            this.tbPhoto.Enabled = Enabled;
            this.tbVideo.Enabled = false;
            this.tbResolution.Enabled = false;
            this.tbMaxRecordTime.Enabled = Enabled;
            this.tbCancel.Enabled = Enabled;
        }

        private void tbGetFromCamera_Click(object sender, EventArgs e)
        {
            ManageGetFromCameraClick();
        }

        private void ManageGetFromCameraClick()
        {
            if (this.tbPhoto.Pushed)
            {
                GetImage();
            }

            if (this.tbVideo.Pushed)
            {
                if (mCameraRecording)
                {
                    this.Timer.Stop();
                    this.Timer.Enabled = false;
                    this.Camera.StopRecording();
                    this.mCameraRecording = false;
                    this.tbGetFromCamera.Text = StartRecordVideoText;
                    this.StatusBar.Text = this.StatusBarCameraRecordingText;
                    this.Camera.Audio = false;
                    this.Camera.Visible = false;
                    ManageToolBarControls(true);
                }
                else
                {
                    ManageToolBarControls(false);
                    this.tbGetFromCamera.Text = StopRecordVideoText;
                    this.StatusBar.Text = this.StatusBarCameraViewModeText;
                    this.mCameraRecording = true;
                    this.Camera.FacingMode = (Wisej.Web.Ext.Camera.Camera.VideoFacingMode)this.cmbFacing.SelectedIndex;
                    SetCameraResolution();
                    this.Camera.Audio = this.chkAudio.Checked;
                    this.Camera.Visible = true;
                    StarRecordingVideo();
                }
            }
        }

        private void tbPhoto_Click(object sender, EventArgs e)
        {
            ManagePhotoClick();
        }

        private void ManagePhotoClick()
        {
            this.tbPhoto.Pushed = true;
            this.tbVideo.Pushed = false;
            this.tbPhoto.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.tbVideo.ForeColor = tbVideoForeColor;
            this.tbAudio.Visible = false;
            this.tbMaxRecordTime.Visible = false;
            this.Camera.FacingMode = (Wisej.Web.Ext.Camera.Camera.VideoFacingMode)this.cmbFacing.SelectedIndex;
            this.Camera.Audio = false;
            SetCameraResolution();
            this.Camera.Visible = true;
            AcquiredObjectType = AcquiredObjectTypes.Image;
        }

        private void tbVideo_Click(object sender, EventArgs e)
        {
            ManageVideoClick();
        }

        private void ManageVideoClick()
        {
            this.Camera.FacingMode = (Wisej.Web.Ext.Camera.Camera.VideoFacingMode)this.cmbFacing.SelectedIndex;
            this.Camera.Audio = this.chkAudio.Checked;
            SetCameraResolution();

            this.Camera.Visible = true;
            this.tbAudio.Visible = true;
            this.tbMaxRecordTime.Visible = true;
            this.tbPhoto.Pushed = false;
            this.tbVideo.Pushed = true;
            this.tbVideo.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.tbPhoto.ForeColor = this.tbPhotoForeColor;
            this.tbGetFromCamera.Text = StartRecordVideoText;
            AcquiredObjectType = AcquiredObjectTypes.Video;
        }

        private void CameraImagePreview_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (AcquiredObjectType == AcquiredObjectTypes.Video)
            {
                this.Timer.Stop();
                this.Timer.Enabled = false;
                this.Camera.StopRecording();
            }

            if (string.IsNullOrEmpty(RealFileName))
            {
                AcquiredObjectType = AcquiredObjectTypes.Null;
                IsOk = false;
            }

            if (IsOk == false)
            {
                if (!string.IsNullOrEmpty(RealFileName))
                {
                    if (System.IO.File.Exists(RealFileName))
                    {
                        System.IO.File.Delete(RealFileName);
                    }

                    AcquiredObjectType = AcquiredObjectTypes.Null;
                    RealFileName = "";
                    VideoSourceURL = "";
                }
            }

            this.Camera.Dispose();
        }

        private void Camera_Error(object sender, Wisej.Web.Ext.Camera.CameraErrorEventArgs e)
        {
            MessageBox.Show("Error on Camera\n" + e.Message);
        }

        private void Camera_Uploaded(object sender, UploadedEventArgs e)
        {
            RealFileName = "";
            VideoSourceURL = "";
            if (string.IsNullOrEmpty(FileName))
            {
                FileName = Guid.NewGuid().ToString() + ".webm";
            }
            else
            {
                string ext = System.IO.Path.GetExtension(FileName);
                if (string.IsNullOrEmpty(ext))
                {
                    FileName = FileName + ".webm";
                }
                else
                {
                    FileName.Replace(ext, ".webm");
                }
            }

            string path = Application.MapPath(ApplicationTempPath + @"\" + FileName);
            RealFileName = path;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            using (var stream = new System.IO.StreamWriter(path))
            {
                e.Files[0].InputStream.CopyTo(stream.BaseStream);
            }

            ContentType = "video/webm";
            ContentLenght = new System.IO.FileInfo(path).Length;
            VideoSourceURL = ApplicationTempPath + @"\" + FileName;
            this.Camera.Visible = false;
            this.chkAudio.Checked = false;
            AcquiredObjectType = AcquiredObjectTypes.Video;
            IsOk = true;
            this.Close();
        }

        private void UpdateRecordingTime()
        {
            var diff = DateTime.Now.Subtract(mStartRecordTime);
            if (diff.TotalSeconds > 0d)
            {
                this.StatusBar.Text = "Recording Time " + string.Format("{0:D2}:{1:D2}:{2:D2}", diff.Hours, diff.Minutes, diff.Seconds);
            }
            else
            {
                this.StatusBar.Text = "00:00:00";
            }

            if (diff.Minutes >= Convert.ToInt16(this.txt_MaxRecordTime.Text))
            {
                ManageGetFromCameraClick();
            }
        }

        private void chkAudio_CheckedChanged(object sender, EventArgs e)
        {
            this.Camera.Audio = this.chkAudio.Checked;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateRecordingTime();
        }

        private void tbClose_Click(object sender, EventArgs e)
        {
            IsOk = false;
            AcquiredObjectType = AcquiredObjectTypes.Null;
            this.Close();
        }
    }
}
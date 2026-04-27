using System;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Wisej.Web.Form" />
    public partial class CameraForm : Form
    {
        /// <summary>
        /// Gets or sets the camera image.
        /// </summary>
        /// <value>
        /// The camera image.
        /// </value>
        public System.Drawing.Image CameraImage { get; set; }
        /// <summary>
        /// Gets or sets the camera video.
        /// </summary>
        /// <value>
        /// The camera video.
        /// </value>
        public System.IO.StreamWriter CameraVideo { get; set; }
        /// <summary>
        /// Gets or sets the name of the real file.
        /// </summary>
        /// <value>
        /// The name of the real file.
        /// </value>
        public string RealFileName { get; set; }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string ContentType { get; set; } = "image/jpeg";
        /// <summary>
        /// Gets or sets the content lenght.
        /// </summary>
        /// <value>
        /// The content lenght.
        /// </value>
        public long ContentLenght { get; set; } = 0;
        /// <summary>
        /// Gets or sets the video source URL.
        /// </summary>
        /// <value>
        /// The video source URL.
        /// </value>
        public string VideoSourceURL { get; set; } = "";
        /// <summary>
        /// Gets or sets the type of the acquired object.
        /// </summary>
        /// <value>
        /// The type of the acquired object.
        /// </value>
        public AcquiredObjectTypes AcquiredObjectType { get; set; } = AcquiredObjectTypes.Null;
        /// <summary>
        /// Gets or sets the object type to acquire.
        /// </summary>
        /// <value>
        /// The object type to acquire.
        /// </value>
        public ObjectTypesToAcquire ObjectTypeToAcquire { get; set; } = ObjectTypesToAcquire.ImageOrVideo;
        /// <summary>
        /// Gets or sets the application temporary path.
        /// </summary>
        /// <value>
        /// The application temporary path.
        /// </value>
        public string ApplicationTempPath { get; set; } = "temp";
        /// <summary>
        /// Gets or sets the maximum record time.
        /// </summary>
        /// <value>
        /// The maximum record time.
        /// </value>
        public int MaxRecordTime { get; set; } = 5;
        /// <summary>
        /// Gets or sets the take photo text.
        /// </summary>
        /// <value>
        /// The take photo text.
        /// </value>
        public string TakePhotoText { get; set; } = "Take Photo";
        /// <summary>
        /// Gets or sets the start record video text.
        /// </summary>
        /// <value>
        /// The start record video text.
        /// </value>
        public string StartRecordVideoText { get; set; } = "Record Video";
        /// <summary>
        /// Gets or sets the stop record video text.
        /// </summary>
        /// <value>
        /// The stop record video text.
        /// </value>
        public string StopRecordVideoText { get; set; } = "Stop Record Video";
        /// <summary>
        /// Gets or sets the status bar camera recording text.
        /// </summary>
        /// <value>
        /// The status bar camera recording text.
        /// </value>
        public string StatusBarCameraRecordingText { get; set; } = "Camera is recording video.";
        /// <summary>
        /// Gets or sets the status bar camera view mode text.
        /// </summary>
        /// <value>
        /// The status bar camera view mode text.
        /// </value>
        public string StatusBarCameraViewModeText { get; set; } = "Camera is view mode.";

        /// <summary>
        /// Gets or sets the video tool bar button.
        /// </summary>
        /// <value>
        /// The video tool bar button.
        /// </value>
        public ToolBarButton VideoToolBarButton
        {
            get { return tbVideo; }
            set { tbVideo = value; }
        }

        /// <summary>
        /// Gets or sets the photo tool bar button.
        /// </summary>
        /// <value>
        /// The photo tool bar button.
        /// </value>
        public ToolBarButton PhotoToolBarButton
        {
            get { return tbPhoto; }
            set { tbPhoto = value; }
        }

        /// <summary>
        /// Gets or sets the cancel tool bar button.
        /// </summary>
        /// <value>
        /// The cancel tool bar button.
        /// </value>
        public ToolBarButton CancelToolBarButton
        {
            get { return tbCancel; }
            set { tbCancel = value; }
        }

        /// <summary>
        /// Gets or sets the get from camera tool bar button.
        /// </summary>
        /// <value>
        /// The get from camera tool bar button.
        /// </value>
        public ToolBarButton GetFromCameraToolBarButton
        {
            get { return tbGetFromCamera; }
            set { tbGetFromCamera = value; }
        }

        /// <summary>
        /// Gets or sets the facing tool bar control.
        /// </summary>
        /// <value>
        /// The facing tool bar control.
        /// </value>
        public ToolBarControl FacingToolBarControl
        {
            get { return tbFacing; }
            set { tbFacing = value; }
        }

        /// <summary>
        /// Gets or sets the resolution tool bar control.
        /// </summary>
        /// <value>
        /// The resolution tool bar control.
        /// </value>
        public ToolBarControl ResolutionToolBarControl
        {
            get { return tbResolution; }
            set { tbResolution = value; }
        }

        /// <summary>
        /// Gets or sets the audio tool bar control.
        /// </summary>
        /// <value>
        /// The audio tool bar control.
        /// </value>
        public ToolBarControl AudioToolBarControl
        {
            get { return tbAudio; }
            set { tbAudio = value; }
        }

        /// <summary>
        /// Gets or sets the maximum record time tool bar control.
        /// </summary>
        /// <value>
        /// The maximum record time tool bar control.
        /// </value>
        public ToolBarControl MaxRecordTimeToolBarControl
        {
            get { return tbMaxRecordTime; }
            set { tbMaxRecordTime = value; }
        }


        /// <summary>
        /// The m camera recording
        /// </summary>
        private bool mCameraRecording = false;
        /// <summary>
        /// The m start record time
        /// </summary>
        private DateTime mStartRecordTime;
        /// <summary>
        /// The is ok
        /// </summary>
        private bool IsOk = false;
        /// <summary>
        /// The tb photo fore color
        /// </summary>
        private System.Drawing.Color tbPhotoForeColor;
#pragma warning disable CS0169 // Il campo 'CameraForm.tbPhotoBackColor' non viene mai usato
        /// <summary>
        /// The tb photo back color
        /// </summary>
        private System.Drawing.Color tbPhotoBackColor;
#pragma warning restore CS0169 // Il campo 'CameraForm.tbPhotoBackColor' non viene mai usato
#pragma warning disable CS0649 // Non è possibile assegnare un valore diverso al campo 'CameraForm.tbVideoForeColor'. Il valore predefinito è
        /// <summary>
        /// The tb video fore color
        /// </summary>
        private System.Drawing.Color tbVideoForeColor;
#pragma warning restore CS0649 // Non è possibile assegnare un valore diverso al campo 'CameraForm.tbVideoForeColor'. Il valore predefinito è
        /// <summary>
        /// The tb video back color
        /// </summary>
        private System.Drawing.Color tbVideoBackColor;


        /// <summary>
        /// 
        /// </summary>
        public enum AcquiredObjectTypes : int
        {
            /// <summary>
            /// The null
            /// </summary>
            Null = 0,
            /// <summary>
            /// The image
            /// </summary>
            Image = 1,
            /// <summary>
            /// The video
            /// </summary>
            Video = 2
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ObjectTypesToAcquire : int
        {
            /// <summary>
            /// The image
            /// </summary>
            Image = 1,
            /// <summary>
            /// The video
            /// </summary>
            Video = 2,
            /// <summary>
            /// The image or video
            /// </summary>
            ImageOrVideo = 3
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraForm"/> class.
        /// </summary>
        public CameraForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
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

            CameraImage = await Camera.GetImageAsync();
            RealFileName = Application.MapPath(ApplicationTempPath + @"\" + FileName);
            if (System.IO.Directory.Exists(Application.MapPath(ApplicationTempPath)) == false)
            {
                MessageBox.Show(string.Format("ApplicationTempPath {0} not exist!", ApplicationTempPath), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                AcquiredObjectType = AcquiredObjectTypes.Null;
                IsOk = false;
                Close();
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
                Close();
            }
            catch (Exception)
            {

                AcquiredObjectType = AcquiredObjectTypes.Null;
                IsOk = false;
                Close();
            }

        }

        /// <summary>
        /// Stars the recording video.
        /// </summary>
        private void StarRecordingVideo()
        {

            mStartRecordTime = DateTime.Now;
            Timer.Enabled = true;
            Timer.Start();
            Camera.StartRecording(ContentType, updateInterval: 1000);
        }

        /// <summary>
        /// Handles the Load event of the CameraImagePreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CameraImagePreview_Load(object sender, EventArgs e)
        {
            Camera.Width = Width - Camera.Left * 2;
            txt_MaxRecordTime.Text = MaxRecordTime.ToString();
            tbFacing.Control = cmbFacing;
            tbResolution.Control = cmbResolution;
            tbAudio.Control = chkAudio;
            tbMaxRecordTime.Control = txt_MaxRecordTime;
            tbPhotoForeColor = tbPhoto.ForeColor;
            tbVideoBackColor = tbVideo.ForeColor;
            switch (ObjectTypeToAcquire)
            {
                case ObjectTypesToAcquire.Video:
                    {
                        tbVideo.Visible = true;
                        tbPhoto.Visible = false;
                        tbVideo.PerformClick();
                        break;
                    }

                case ObjectTypesToAcquire.Image:
                    {
                        tbVideo.Visible = false;
                        tbPhoto.Visible = true;
                        tbPhoto.PerformClick();
                        break;
                    }

                case ObjectTypesToAcquire.ImageOrVideo:
                    {
                        tbVideo.Visible = true;
                        tbPhoto.Visible = true;
                        tbPhoto.PerformClick();
                        break;
                    }

                default:
                    {
                        Close();
                        break;
                    }
            }

            Camera.ObjectFit = ObjectFit.Contain;
            string x = Camera.WidthCapture.ToString() + "x" + Camera.HeightCapture.ToString();
            if (cmbResolution.Items.Contains(x) == false)
            {
                cmbResolution.Items.Add(x);
            }

            cmbResolution.SelectedIndex = cmbResolution.Items.IndexOf(x);
            cmbFacing.SelectedIndex = 1;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbFacing control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void cmbFacing_SelectedIndexChanged(object sender, EventArgs e)
        {
            Camera.FacingMode = (Wisej.Web.Ext.Camera.Camera.VideoFacingMode)cmbFacing.SelectedIndex;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cmbResolution control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void cmbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCamera();
        }

        /// <summary>
        /// Sets the camera resolution.
        /// </summary>
        private void SetCameraResolution()
        {
            string[] res;
            string Item = Convert.ToString(cmbResolution.SelectedItem);
            if (!string.IsNullOrEmpty(Item))
            {
                res = Item.Split('x');
                Camera.HeightCapture = Convert.ToInt16(res[1]);
                Camera.WidthCapture = Convert.ToInt16(res[0]);
            }
        }

        /// <summary>
        /// Sets the camera.
        /// </summary>
        /// <param name="audio">if set to <c>true</c> [audio].</param>
        private void SetCamera(bool audio = false)
        {
            string[] res;
            string Item = Convert.ToString(cmbResolution.SelectedItem);
            if (!string.IsNullOrEmpty(Item))
            {
                res = Item.Split('x');
                Camera.HeightCapture = Convert.ToInt16(res[1]);
                Camera.WidthCapture = Convert.ToInt16(res[0]);
                Camera.FacingMode = (Wisej.Web.Ext.Camera.Camera.VideoFacingMode)cmbFacing.SelectedIndex;
                Camera.Audio = audio;
            }
        }

        /// <summary>
        /// Manages the tool bar controls.
        /// </summary>
        /// <param name="Enabled">if set to <c>true</c> [enabled].</param>
        private void ManageToolBarControls(bool Enabled)
        {
            tbAudio.Enabled = Enabled;
            tbFacing.Enabled = Enabled;
            tbPhoto.Enabled = Enabled;
            tbVideo.Enabled = false;
            tbResolution.Enabled = false;
            tbMaxRecordTime.Enabled = Enabled;
            tbCancel.Enabled = Enabled;
        }

        /// <summary>
        /// Handles the Click event of the tbGetFromCamera control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void tbGetFromCamera_Click(object sender, EventArgs e)
        {
            ManageGetFromCameraClick();
        }

        /// <summary>
        /// Manages the get from camera click.
        /// </summary>
        private void ManageGetFromCameraClick()
        {
            if (tbPhoto.Pushed)
            {
                GetImage();
            }

            if (tbVideo.Pushed)
            {
                if (mCameraRecording)
                {
                    Timer.Stop();
                    Timer.Enabled = false;
                    Camera.StopRecording();
                    mCameraRecording = false;
                    tbGetFromCamera.Text = StartRecordVideoText;
                    StatusBar.Text = StatusBarCameraRecordingText;
                    Camera.Audio = false;
                    Camera.Visible = false;
                    ManageToolBarControls(true);
                }
                else
                {
                    ManageToolBarControls(false);
                    tbGetFromCamera.Text = StopRecordVideoText;
                    StatusBar.Text = StatusBarCameraViewModeText;
                    mCameraRecording = true;
                    Camera.FacingMode = (Wisej.Web.Ext.Camera.Camera.VideoFacingMode)cmbFacing.SelectedIndex;
                    SetCameraResolution();
                    Camera.Audio = chkAudio.Checked;
                    Camera.Visible = true;
                    StarRecordingVideo();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the tbPhoto control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void tbPhoto_Click(object sender, EventArgs e)
        {
            ManagePhotoClick();
        }

        /// <summary>
        /// Manages the photo click.
        /// </summary>
        private void ManagePhotoClick()
        {
            tbPhoto.Pushed = true;
            tbVideo.Pushed = false;
            tbPhoto.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            tbVideo.ForeColor = tbVideoForeColor;
            tbAudio.Visible = false;
            tbMaxRecordTime.Visible = false;
            Camera.FacingMode = (Wisej.Web.Ext.Camera.Camera.VideoFacingMode)cmbFacing.SelectedIndex;
            Camera.Audio = false;
            SetCameraResolution();
            Camera.Visible = true;
            AcquiredObjectType = AcquiredObjectTypes.Image;
        }

        /// <summary>
        /// Handles the Click event of the tbVideo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void tbVideo_Click(object sender, EventArgs e)
        {
            ManageVideoClick();
        }

        /// <summary>
        /// Manages the video click.
        /// </summary>
        private void ManageVideoClick()
        {
            Camera.FacingMode = (Wisej.Web.Ext.Camera.Camera.VideoFacingMode)cmbFacing.SelectedIndex;
            Camera.Audio = chkAudio.Checked;
            SetCameraResolution();

            Camera.Visible = true;
            tbAudio.Visible = true;
            tbMaxRecordTime.Visible = true;
            tbPhoto.Pushed = false;
            tbVideo.Pushed = true;
            tbVideo.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            tbPhoto.ForeColor = tbPhotoForeColor;
            tbGetFromCamera.Text = StartRecordVideoText;
            AcquiredObjectType = AcquiredObjectTypes.Video;
        }

        /// <summary>
        /// Handles the FormClosed event of the CameraImagePreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosedEventArgs"/> instance containing the event data.</param>
        private void CameraImagePreview_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (AcquiredObjectType == AcquiredObjectTypes.Video)
            {
                Timer.Stop();
                Timer.Enabled = false;
                Camera.StopRecording();
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

            Camera.Dispose();
        }

        /// <summary>
        /// Handles the Error event of the Camera control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Wisej.Web.Ext.Camera.CameraErrorEventArgs"/> instance containing the event data.</param>
        private void Camera_Error(object sender, Wisej.Web.Ext.Camera.CameraErrorEventArgs e)
        {
            MessageBox.Show("Error on Camera\n" + e.Message);
        }

        /// <summary>
        /// Handles the Uploaded event of the Camera control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UploadedEventArgs"/> instance containing the event data.</param>
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
            Camera.Visible = false;
            chkAudio.Checked = false;
            AcquiredObjectType = AcquiredObjectTypes.Video;
            IsOk = true;
            Close();
        }

        /// <summary>
        /// Updates the recording time.
        /// </summary>
        private void UpdateRecordingTime()
        {
            var diff = DateTime.Now.Subtract(mStartRecordTime);
            if (diff.TotalSeconds > 0d)
            {
                StatusBar.Text = "Recording Time " + string.Format("{0:D2}:{1:D2}:{2:D2}", diff.Hours, diff.Minutes, diff.Seconds);
            }
            else
            {
                StatusBar.Text = "00:00:00";
            }

            if (diff.Minutes >= Convert.ToInt16(txt_MaxRecordTime.Text))
            {
                ManageGetFromCameraClick();
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkAudio control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void chkAudio_CheckedChanged(object sender, EventArgs e)
        {
            Camera.Audio = chkAudio.Checked;
        }

        /// <summary>
        /// Handles the Tick event of the Timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateRecordingTime();
        }

        /// <summary>
        /// Handles the Click event of the tbClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void tbClose_Click(object sender, EventArgs e)
        {
            IsOk = false;
            AcquiredObjectType = AcquiredObjectTypes.Null;
            Close();
        }
    }
}
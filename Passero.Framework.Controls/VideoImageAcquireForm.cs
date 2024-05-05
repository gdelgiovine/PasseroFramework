using System;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
using Wisej.Web;

namespace Passero.Framework.Controls
{


    public partial class VideoImageAcquireForm
    {

        private bool mIsOk = false;
        private AcquiredObjectTypes mFile_AcquiredObjectType = AcquiredObjectTypes.Null;
        private AcquiredObjectTypes mCamera_AcquiredObjectType = AcquiredObjectTypes.Null;
        private AcquiredObjectTypes mWeb_AcquiredObjectType = AcquiredObjectTypes.Null;
        private string mFile_FileName = "";
        private string mFile_RealFileName = "";
        private string mCamera_FileName = "";
        private string mCamera_RealFileName = "";
        private string mWeb_FileName = "";
        private string mWeb_RealFileName = "";
        private string mCamera_ContentType = "";
        private string mFile_ContentType = "";
        private string mWeb_ContentType = "";
        private long mFile_ContentLenght = 0;
        private long mCamera_ContentLenght = 0;
        private long mWeb_ContentLenght = 0;



        public VideoImageAcquireForm()
        {

            // This call is required by the designer.
            InitializeComponent();
            // Add any initialization after the InitializeComponent() call.

        }

        public VideoImageAcquireForm(Form CallerForm)
        {
            // This call is required by the designer.
            InitializeComponent();
            this.CallerForm = CallerForm;
            // Add any initialization after the InitializeComponent() call.
        }
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


        private AcquiredObjectTypes mAcquiredObjectType = AcquiredObjectTypes.Null;
        public AcquiredObjectTypes AcquiredObjectType
        {
            get
            {
                return mAcquiredObjectType;
            }
            set
            {
                mAcquiredObjectType = value;
            }
        }

        private ObjectTypesToAcquire mObjectTypeToAcquire = ObjectTypesToAcquire.ImageOrVideo;
        public ObjectTypesToAcquire ObjectTypeToAcquire
        {
            get
            {
                return mObjectTypeToAcquire;
            }
            set
            {
                mObjectTypeToAcquire = value;
            }
        }


        private Form mCallerForm = null;

        public Form CallerForm
        {
            get
            {
                return mCallerForm;
            }
            set
            {
                mCallerForm = value;
                if (mCallerForm is not null)
                {
                    mCallerForm.Enabled = false;
                }
            }
        }

        private string mApplicationTempPath;
        public string ApplicationTempPath
        {
            get
            {
                return mApplicationTempPath;
            }
            set
            {
                mApplicationTempPath = value;
            }
        }


        private System.Drawing.Imaging.ImageFormat mImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
        public System.Drawing.Imaging.ImageFormat ImageFormat
        {
            get
            {
                return mImageFormat;
            }
            set
            {
                mImageFormat = value;
            }
        }

        private string mAllowedFileType = "";
        public string AllowedFileType
        {
            get
            {
                return mAllowedFileType;
            }
            set
            {
                mAllowedFileType = value;
                if (!string.IsNullOrEmpty(mAllowedFileType))
                {
                    switch (mObjectTypeToAcquire)
                    {
                        case ObjectTypesToAcquire.Image:
                            {
                                Upload1.AllowedFileTypes = "image/*";
                                break;
                            }
                        case ObjectTypesToAcquire.Video:
                            {
                                Upload1.AllowedFileTypes = "video/*";
                                break;
                            }
                        case ObjectTypesToAcquire.ImageOrVideo:
                            {
                                Upload1.AllowedFileTypes = "image/*,video/*";
                                break;
                            }
                    }
                }
            }
        }


        private string mFileName;
        public string FileName
        {
            get
            {
                return mFileName;
            }
            set
            {
                mFileName = value;
            }
        }


        private string mRealFileName;
        public string RealFileName
        {
            get
            {
                return mRealFileName;
            }
            set
            {
                mRealFileName = value;
            }
        }

        private string mVideoSourceURL;
        public string VideoSourceURL
        {
            get
            {
                return mVideoSourceURL;
            }
            set
            {
                mVideoSourceURL = value;
            }
        }
        private string mContentType;
        public string ContentType
        {
            get
            {
                return mContentType;
            }
            set
            {
                mContentType = value;
            }
        }

        private long mContentLenght;
        public long ContentLenght
        {
            get
            {
                return mContentLenght;
            }
            set
            {
                mContentLenght = value;
            }
        }

        public enum Sources : int
        {
            File = 1,
            Camera = 2,
            FileAndCamera = 3,
            Web = 4
        }

        public byte[] ImageToByteArray()
        {
            return SystemDrawingHelper.imageToByteArray(PictureBoxFile.Image);
            
        }

        public System.IO.Stream ImageStream()
        {
            System.IO.Stream stream = null;
            PictureBoxFile.Image.Save(stream, mImageFormat);
            return stream;
        }

        private Sources mSource = (Sources)((int)Sources.Camera + (int)Sources.File);

        public Sources Source
        {
            get
            {
                return mSource;
            }
            set
            {
                mSource = value;
                if (Conversions.ToBoolean(mSource & Sources.File))
                {
                    TabPageFromFile.Hidden = false;
                }
                else
                {
                    TabPageFromFile.Hidden = true;
                }
                if (Conversions.ToBoolean(mSource & Sources.Camera))
                {
                    TabPageFromCamera.Hidden = false;
                }
                else
                {
                    TabPageFromCamera.Hidden = true;
                }

            }
        }

        private bool DownloadFileFromWeb(string URL)
        {

            if (string.IsNullOrEmpty(URL))
            {
                return false;
            }

            mWeb_ContentLenght = 0;
            mWeb_ContentType = "";
            mWeb_RealFileName = "";
            mWeb_FileName = "";

            ContentLenght = 0;
            FileName = "";

            // Dim uriResult As System.Uri = Nothing
            // Dim result = System.Uri.TryCreate(URL, System.UriKind.Absolute, uriResult) AndAlso (uriResult.Scheme = System.Uri.UriSchemeHttp OrElse uriResult.Scheme = System.Uri.UriSchemeHttps)
            System.Net.WebRequest request;

            System.Net.WebResponse response;

            try
            {
                request = System.Net.WebRequest.Create(URL);
                response = request.GetResponse();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text);
                return false;
            }

            // response = TryCast(request.GetResponse(), Net.HttpWebResponse)
            if (response is not null)
            {
                if (response.ContentLength > 0L)
                {
                    mWeb_ContentType = response.ContentType;
                    mWeb_ContentLenght = (int)response.ContentLength;
                    mWeb_FileName = response.ResponseUri.Segments.Last();
                    mWeb_RealFileName = Application.MapPath(mApplicationTempPath) + @"\" + Guid.NewGuid().ToString() + System.IO.Path.GetExtension(mWeb_FileName);
                    try
                    {
                        using (var client = new System.Net.WebClient())
                        {
                            client.DownloadFile(URL, mWeb_RealFileName);
                        }

                        if (mWeb_ContentType.StartsWith("image"))
                        {
                            mWeb_AcquiredObjectType = AcquiredObjectTypes.Image;
                            PictureBoxWeb.Image = SafeImageFromFile(mWeb_RealFileName);
                            PictureBoxWeb.Visible = true;
                        }

                        if (mFile_ContentType.StartsWith("video"))
                        {
                            mWeb_AcquiredObjectType = AcquiredObjectTypes.Video;
                            VideoWeb.SourceURL = ApplicationTempPath + "/" + System.IO.Path.GetFileName(mWeb_RealFileName);
                            VideoWeb.Visible = true;
                        }


                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Text);
                        return false;
                    }
                }
            }

            return false;


        }

        private void ImageAcquire_Load(object sender, EventArgs e)
        {


            if (CallerForm is not null)
            {
                CallerForm.Enabled = false;
            }
            btn_OpenCamera.Location = btn_Upload.Location;
            btn_OpenCamera.Visible = false;
            btn_WebDownload.Location = btn_Upload.Location;
            btn_WebDownload.Visible = false;


            VideoFile.Visible = false;
            VideoFile.Dock = DockStyle.Fill;
            PictureBoxFile.Visible = false;
            PictureBoxFile.Dock = DockStyle.Fill;


            VideoCamera.Visible = false;
            VideoCamera.Dock = DockStyle.Fill;
            PictureBoxCamera.Visible = false;
            PictureBoxCamera.Dock = DockStyle.Fill;

            VideoWeb.Visible = false;
            VideoWeb.Dock = DockStyle.Fill;
            PictureBoxWeb.Visible = false;
            PictureBoxWeb.Dock = DockStyle.Fill;


            TabControl.SelectedIndex = 0;

        }

        private void Upload1_Uploaded(object sender, UploadedEventArgs e)
        {

            PictureBoxFile.Visible = false;
            VideoFile.Visible = false;


            mFile_ContentType = e.Files[0].ContentType;
            mFile_ContentLenght = (int)e.Files[0].ContentLength;
            mFile_FileName = e.Files[0].FileName;
            mFile_RealFileName = Application.MapPath(mApplicationTempPath) + @"\" + Guid.NewGuid().ToString() + System.IO.Path.GetExtension(mFile_FileName);

            e.Files[0].SaveAs(mFile_RealFileName);

            if (mFile_ContentType.StartsWith("image"))
            {
                mFile_AcquiredObjectType = AcquiredObjectTypes.Image;
                PictureBoxFile.Image = System.Drawing.Image.FromStream(e.Files[0].InputStream);
                PictureBoxFile.Dock = DockStyle.Fill;
                PictureBoxFile.Visible = true;
            }

            if (mFile_ContentType.StartsWith("video"))
            {
                mFile_AcquiredObjectType = AcquiredObjectTypes.Video;
                VideoFile.SourceURL = ApplicationTempPath + "/" + System.IO.Path.GetFileName(mFile_RealFileName);
                VideoFile.Visible = true;

            }


        }

        private void btn_Upload_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(AllowedFileType))
            {
                switch (mObjectTypeToAcquire)
                {
                    case ObjectTypesToAcquire.Image:
                        {
                            Upload1.AllowedFileTypes = "image/*";
                            break;
                        }
                    case ObjectTypesToAcquire.Video:
                        {
                            Upload1.AllowedFileTypes = "video/*";
                            break;
                        }
                    case ObjectTypesToAcquire.ImageOrVideo:
                        {
                            Upload1.AllowedFileTypes = "image/*,video/*";
                            break;
                        }
                }
            }

            Upload1.AllowMultipleFiles = false;
            Upload1.UploadFiles();
        }

        private void ImageAcquire_FormClosed(object sender, FormClosedEventArgs e)
        {

            VideoFile.Dispose();
            PictureBoxFile.Image = null;

            if (CallerForm is not null)
            {
                CallerForm.Enabled = true;
                CallerForm.Focus();
            }

            Dispose();

        }

        private void btn_OK_Click(object sender, EventArgs e)
        {

            switch (TabControl.SelectedTab.Name ?? "")
            {

                case var @case when @case == (TabPageFromFile.Name ?? ""):
                    {
                        mAcquiredObjectType = mFile_AcquiredObjectType;
                        mFileName = mFile_FileName;
                        mRealFileName = mFile_RealFileName;
                        mContentType = mFile_ContentType;
                        mContentLenght = mFile_ContentLenght;
                        mVideoSourceURL = VideoFile.SourceURL;
                        break;
                    }
                case var case1 when case1 == (TabPageFromCamera.Name ?? ""):
                    {
                        mAcquiredObjectType = mCamera_AcquiredObjectType;
                        mFileName = mCamera_FileName;
                        mRealFileName = mCamera_RealFileName;
                        mContentType = mCamera_ContentType;
                        mContentLenght = mCamera_ContentLenght;
                        mVideoSourceURL = VideoCamera.SourceURL;
                        break;
                    }
                case var case2 when case2 == (TabPageFromWeb.Name ?? ""):
                    {
                        mAcquiredObjectType = mWeb_AcquiredObjectType;
                        mFileName = mWeb_FileName;
                        mRealFileName = mWeb_RealFileName;
                        mContentType = mWeb_ContentType;
                        mContentLenght = mWeb_ContentLenght;
                        mVideoSourceURL = VideoWeb.SourceURL;
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            switch (mAcquiredObjectType)
            {
                case AcquiredObjectTypes.Video:
                    {
                        mIsOk = true;
                        break;
                    }
                case AcquiredObjectTypes.Image:
                    {
                        mIsOk = true;
                        break;
                    }

                default:
                    {
                        mIsOk = false;
                        break;
                    }
            }

            Close();

        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {

            mIsOk = false;
            Close();

        }



        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {


            switch (TabControl.SelectedTab.Name ?? "")
            {
                case var @case when @case == (TabPageFromFile.Name ?? ""):
                    {
                        btn_OpenCamera.Visible = false;
                        btn_Upload.Visible = true;
                        btn_WebDownload.Visible = false;
                        break;
                    }


                case var case1 when case1 == (TabPageFromCamera.Name ?? ""):
                    {
                        btn_OpenCamera.Visible = true;
                        btn_Upload.Visible = false;
                        btn_WebDownload.Visible = false;
                        break;
                    }

                case var case2 when case2 == (TabPageFromWeb.Name ?? ""):
                    {
                        btn_OpenCamera.Visible = false;
                        btn_Upload.Visible = false;
                        btn_WebDownload.Visible = true;
                        break;
                    }

                default:
                    {
                        break;
                    }

            }

        }
        private void ciao()
        {

        }
        private void btn_OpenCamera_Click(object sender, EventArgs e)
        {
            var CameraForm = new CameraForm();

            if (string.IsNullOrEmpty(mFileName))
            {
                mFileName = Guid.NewGuid().ToString();
            }

            CameraForm.MaxRecordTime = 5;
            CameraForm.FileName = mFileName;
            CameraForm.RealFileName = ""; // Me.mRealFileName
            CameraForm.ObjectTypeToAcquire = (CameraForm.ObjectTypesToAcquire)mObjectTypeToAcquire;


            CameraForm.ShowDialog();

            PictureBoxCamera.Visible = false;

            if (CameraForm.AcquiredObjectType != (int)AcquiredObjectTypes.Null)
            {

                mCamera_FileName = CameraForm.FileName;
                mCamera_RealFileName = CameraForm.RealFileName;
                mCamera_ContentType = CameraForm.ContentType;
                mCamera_ContentLenght = CameraForm.ContentLenght;

                switch (CameraForm.AcquiredObjectType)
                {
                    case CameraForm.AcquiredObjectTypes.Video:
                        {
                            PictureBoxCamera.Visible = false;
                            PictureBoxCamera.Image = null;
                            mCamera_AcquiredObjectType = AcquiredObjectTypes.Video;
                            VideoCamera.SourceURL = CameraForm.VideoSourceURL;
                            VideoCamera.Visible = true;
                            break;
                        }
                    case CameraForm.AcquiredObjectTypes.Image:
                        {
                            VideoCamera.Visible = false;
                            VideoCamera.SourceURL = "";
                            mCamera_AcquiredObjectType = AcquiredObjectTypes.Image;
                            PictureBoxCamera.Image = SafeImageFromFile(CameraForm.RealFileName);
                            PictureBoxCamera.SizeMode = PictureBoxSizeMode.Zoom;
                            PictureBoxCamera.Visible = true;
                            break;
                        }

                    default:
                        {
                            break;
                        }

                }
            }

            else
            {
                VideoCamera.Visible = false;
                mCamera_AcquiredObjectType = AcquiredObjectTypes.Null;
                PictureBoxCamera.Image = null;
                PictureBoxCamera.Visible = false;
                VideoCamera.SourceURL = "";
                mCamera_FileName = "";
                mCamera_ContentLenght = 0;
                mCamera_ContentType = "";
                mCamera_RealFileName = "";
            }

            CameraForm.Dispose();

        }

        private System.Drawing.Image SafeImageFromFile(string path)
        {

            byte[] bytesArr = null;
            if (System.IO.File.Exists(path) == false)
            {
                return null;
            }
            bytesArr = System.IO.File.ReadAllBytes(path);

            var memstr = new System.IO.MemoryStream(bytesArr);
            var img = System.Drawing.Image.FromStream(memstr);
            return img;
        }

        private void CameraAcquire_Closed(object sender, EventArgs e)
        {
            if (mIsOk == false)
            {
                if (!string.IsNullOrEmpty(mRealFileName))
                {
                    if (System.IO.File.Exists(mRealFileName))
                    {
                        System.IO.File.Delete(mRealFileName);
                    }
                    mRealFileName = "";
                }

            }
        }

        private void TextBox1_ToolClick(object sender, ToolClickEventArgs e)
        {
            txt_SourceURL.Text = "";
            PictureBoxWeb.Image = null;
            mWeb_RealFileName = "";
            mWeb_FileName = "";
            mWeb_ContentType = "";
            mWeb_ContentLenght = 0;
            mWeb_AcquiredObjectType = AcquiredObjectTypes.Null;
            VideoWeb.SourceURL = "";

        }

        private void btn_WebDownload_Click(object sender, EventArgs e)
        {

            DownloadFileFromWeb(txt_SourceURL.Text);


        }
    }
}
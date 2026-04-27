using System;
using System.ComponentModel;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    public enum LayoutType
    {
        NavBarOnLeft,
        TopBarOnTop,
    }

    public partial class Layout : Wisej.Web.UserControl
    {
        [Browsable(true)]
        public Wisej.Web.Ext.NavigationBar.NavigationBar NavigationBar
        {
            get
            {
                return this._NavigationBar;
            }
        }
        [Browsable(true)]
        public Wisej.Web.Panel TitleBar
        {
            get
            {
                return this._TitleBar;
            }
        }
        [Browsable(true)]
        public Layout()
        {
            InitializeComponent();
            ResizePage();   
        }

        private LayoutType _LayoutType = LayoutType.TopBarOnTop;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Browsable(true)]
        public LayoutType LayoutType {
            get { return _LayoutType; } 
            set {  
                _LayoutType = value; 
                this.ResizePage();
                OnPropertyChanged(nameof(LayoutType));
            }    
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ResizePage()
        {
            // Resize the controls on the page.
            // This method is called when the page is resized.
            switch (_LayoutType)
            {
                case LayoutType.NavBarOnLeft:
                    this.NavigationBar.Dock= DockStyle.Left;
                    this.TitleBar.Top = 0;
                    this.TitleBar.Left = this.NavigationBar.Width;
                    this.TitleBar.Width = this.Width - this.TitleBar.Left;
                    break;
                case LayoutType.TopBarOnTop:
                    this.TitleBar.Dock = DockStyle.Top;
                    this.NavigationBar.Dock = DockStyle.None;
                    this.NavigationBar.Left = 0;
                    this.NavigationBar.Top = this.TitleBar.Top + this.TitleBar.Height;
                    this.NavigationBar.Height = this.Height - this.NavigationBar.Top;
                    break;
                default:
                    break;
            }
            
            this.ResizeDesktop();

        }

        public void ResizeDesktop()
        {
            if (this.NavigationBar.Visible)
            {
                //this.Desktop.Top = this.navigationBar.Top;
                //this.Desktop.Left = this.navigationBar.Width;
                //this.Desktop.Height = this.navigationBar.Height;
                //this.Desktop.Width = this.Width - this.navigationBar.Width;
            }
            else
            {
                if (this.TitleBar.Visible)
                {
                    //this.Desktop.Top = this.pnlTopBar.Top + this.pnlTopBar.Height;
                    //this.Desktop.Left = 0;
                    //this.Desktop.Height = this.Height - this.Desktop.Top;
                    //this.Desktop.Width = this.Width;
                }
                else
                {
                    //this.Desktop.Top = 0;
                    //this.Desktop.Left = 0;
                    //this.Desktop.Height = this.Height;
                    //this.Desktop.Width = this.Width;
                }

            }
        }

        private void Layout_Resize(object sender, EventArgs e)
        {
           this.ResizePage();
        }

        private void Layout_Load(object sender, EventArgs e)
        {
            this.ResizePage();
        }

        private void _NavigationBar_Resize(object sender, EventArgs e)
        {
            this.ResizeDesktop();
        }
    }
}

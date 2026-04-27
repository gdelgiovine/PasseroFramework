

namespace Passero.Framework.SSRSReports.Extensions.Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (MemoryStream ms = new MemoryStream(SSRSExtensions.GenerateBarcode("QR_CODE", "https://www.google.com")))
            {
                this.pictureBox1.Image = Image.FromStream(ms);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

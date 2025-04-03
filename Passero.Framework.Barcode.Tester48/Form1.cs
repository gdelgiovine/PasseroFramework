using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Passero.Framework.Barcode.Tester48
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string barcodetype= BarcodeFormat.QR_CODE.ToString () ;  
            string barcodevalue = txtBarcodeText.Text;
            //byte[] barcodeBytes = Passero.Framework.Barcode.BarcodeImageFactory.GenerateBarcodeAsByteArray(barcodetype, $"{barcodevalue}");

            string barcodetypespire = BarcodeFormatSpire.QRCODE.ToString();
            byte[] barcodeBytes = Passero.Framework.Barcode.BarcodeImageFactory.GenerateBarcodeAsByteArraySpire(barcodetypespire, $"{barcodevalue}");

            
            using (MemoryStream ms = new MemoryStream(barcodeBytes))
            {
                Bitmap bitmap = new Bitmap(ms);
                this.pictureBox1.Image = bitmap;
            }


            this.txtBarcodeEncodedText.Text = (barcodevalue);
            this.txtBarcodeEncodedTextFont.Font = new Font("BCW_QR", 10);

            this.txtBarcodeEncodedText.Text = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(barcodevalue));
            //this.txtBarcodeEncodedTextFont .Font = new Font("Code EAN13",80);


            this.txtBarcodeEncodedTextFont.Text = this.txtBarcodeEncodedText.Text;
           

        }
    }
    
}

namespace Passero.Framework.Barcode.Tester48
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.txtBarcodeText = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtBarcodeEncodedText = new System.Windows.Forms.TextBox();
            this.txtBarcodeEncodedTextFont = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(375, 35);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 22);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBarcodeText
            // 
            this.txtBarcodeText.Location = new System.Drawing.Point(24, 35);
            this.txtBarcodeText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBarcodeText.Name = "txtBarcodeText";
            this.txtBarcodeText.Size = new System.Drawing.Size(235, 20);
            this.txtBarcodeText.TabIndex = 1;
            this.txtBarcodeText.Text = "019954148850";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(24, 61);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(345, 192);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // txtBarcodeEncodedText
            // 
            this.txtBarcodeEncodedText.Location = new System.Drawing.Point(375, 61);
            this.txtBarcodeEncodedText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBarcodeEncodedText.Name = "txtBarcodeEncodedText";
            this.txtBarcodeEncodedText.Size = new System.Drawing.Size(226, 20);
            this.txtBarcodeEncodedText.TabIndex = 3;
            // 
            // txtBarcodeEncodedTextFont
            // 
            this.txtBarcodeEncodedTextFont.Font = new System.Drawing.Font("Code39AzaleaRegular1", 80.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcodeEncodedTextFont.Location = new System.Drawing.Point(375, 99);
            this.txtBarcodeEncodedTextFont.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBarcodeEncodedTextFont.Name = "txtBarcodeEncodedTextFont";
            this.txtBarcodeEncodedTextFont.Size = new System.Drawing.Size(414, 114);
            this.txtBarcodeEncodedTextFont.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 450);
            this.Controls.Add(this.txtBarcodeEncodedTextFont);
            this.Controls.Add(this.txtBarcodeEncodedText);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtBarcodeText);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtBarcodeText;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtBarcodeEncodedText;
        private System.Windows.Forms.TextBox txtBarcodeEncodedTextFont;
    }
}


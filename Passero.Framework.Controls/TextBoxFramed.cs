using System;
using System.ComponentModel;
using System.Drawing;
using Wisej.Web;


namespace Passero.Framework.Controls
{
    [ToolboxItem(true)]
    public partial class TextBoxFramed : Wisej.Web.TextBox
    {
        private float _charWidth = 0;
        private bool _measuringNeeded = true;
        private int _verticalLineHeight = 100;
        private int leftToolsWidth = 0;
        private int rightToolsWidth = 0;
        private int toolsWidth = 0; 

        [Category("Appearance")]
        [Description("Ottiene o imposta l'altezza della linea verticale (0-100).")]
        public int VerticalLineHeight
        {
            get => _verticalLineHeight;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException(nameof(value), "Il valore deve essere compreso tra 0 e 100.");
                _verticalLineHeight = value;
                Invalidate(); // Richiama il ridisegno del controllo.
            }
        }

        private Color _penColor = Color.LightGray;

        [Category("Appearance")]
        [Description("Specifies the color of the pen used to draw lines or rectangles.")]
        public Color PenColor
        {
            get => _penColor;
            set
            {
                this.SuspendLayout();
                _penColor = value;
                this.ResumeLayout();
                this.Invalidate(); // Forza il ridisegno del controllo
            }
        }

        private float _penSize = 1f;

        [Category("Appearance")]
        [Description("Specifies the thickness of the pen used to draw lines.")]
        public float PenSize
        {
            get => _penSize;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(PenSize), "The value of PenSize must be greater than 0.");
                this.SuspendLayout();
                _penSize = value;
                this.ResumeLayout();
                this.Invalidate(); // Forza il ridisegno del controllo
            }
        }
        public TextBoxFramed()
        {
            InitializeComponent();
            this.Paint += XTextBox_Paint;
            this.TextChanged += XTextBox_TextChanged;
            this.Resize += XTextBox_Resize;
            this.Font = new Font("Courier New", 12); // Imposta un font di default

            // Imposta un valore di default personalizzato per MaxLength
            this.MaxLength = 10;
        }


        private float[] _characterPositions; // Aggiungere come campo alla classe

        // Metodo ottimizzato per precalcolare le posizioni dei caratteri
        private void CalculateCharacterPositions()
        {
            _characterPositions = new float[MaxLength + 1];

            using (var g = this.CreateGraphics())
            {
                string sampleText = new string('M', MaxLength);

                // Calcolo le posizioni per tutti i caratteri in una sola volta
                for (int i = 0; i <= MaxLength; i++)
                {
                    string subString = i > 0 ? sampleText.Substring(0, i) : string.Empty;
                    SizeF size = Wisej.Base.TextUtils.MeasureText(subString, this.Font);
                    _characterPositions[i] = size.Width;
                }
            }
        }



        public override int MaxLength
        {
            get => base.MaxLength;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(MaxLength), "The value of MaxLength cannot be less than 1.");
                this.SuspendLayout();
                base.MaxLength = value;
                this.ResumeLayout();    
                UpdateWidth(); // Aggiorna la larghezza del controllo
            }
        }

        private System.Drawing.Drawing2D.DashStyle _penStyle = System.Drawing.Drawing2D.DashStyle.Solid;

        [Category("Appearance")]
        [Description("Specifies the style of the pen used to draw lines.")]
        public System.Drawing.Drawing2D.DashStyle PenStyle
        {
            get => _penStyle;
            set
            {
                this.SuspendLayout();
                _penStyle = value;
                this.ResumeLayout();
                this.Invalidate(); // Forza il ridisegno del controllo
            }
        }

        private int _VerticalLineStep = 1;

        [Category("Appearance")]
        [Description("Specifies the number of characters after which a vertical line should be drawn.")]

        public int VerticalLineStep
        {
            get => _VerticalLineStep;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(VerticalLineStep), "The value of ShowVline cannot be less than 1.");
                _VerticalLineStep = value;
                this.Invalidate(); // Forza il ridisegno del controllo
            }
        }


        private void UpdateWidth()
        {
            using (var g = this.CreateGraphics())
            {
                // Calculates the width of the string with the maximum length (MaxLength)
                GetToolsWidth();
                string testString = new string('M', this.MaxLength);
                SizeF size = g.MeasureString(testString, this.Font);
                this.Width = (int)Math.Ceiling(size.Width) + 3+toolsWidth ;
            }
        }
        private void XTextBox_TextChanged(object sender, EventArgs e)
        {
            //_measuringNeeded = true;
            this.Invalidate();
        }

        private void XTextBox_Resize(object sender, EventArgs e)
        {
            //_measuringNeeded = true;
            this.Invalidate();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            if (!this.DesignMode)
            {
                UpdateWidth();
            }
            else if (TypeDescriptor.GetProperties(this)["Font"] is PropertyDescriptor fontProperty
                     && fontProperty.ShouldSerializeValue(this))
            {
                //MessageBox.Show("Font property changed at design time.");
                GetToolsWidth();
                // Solo a design time quando la proprietà Font è stata effettivamente modificata
                this.Width = this.MaxLength * (int)Math.Ceiling(this.Font.Size) + 3+toolsWidth ;
            }
        }

        private void XTextBox_Paint(object sender, PaintEventArgs e)
        {
          
            DrawGridLinesFinal(e.Graphics);
           
        }



        private void GetToolsWidth()
        {
            // Verifica se il controllo ha strumenti ComponentTool associati
            if (this.Tools != null && this.Tools.Count > 0)
            {
                foreach (ComponentTool tool in this.Tools)
                {
                    // Determina se lo strumento è a destra
                    // (potrebbe richiedere una logica personalizzata in base all'API)
                    if (tool.Position == LeftRightAlignment.Left)
                    {
                        rightToolsWidth += 40; // Aggiungi la larghezza dello strumento
                    }
                    else
                    {
                        leftToolsWidth += 40; // Aggiungi la larghezza dello strumento
                    }
                }

                toolsWidth = leftToolsWidth + rightToolsWidth;  
            }
        }


        private void DrawGridLinesFinal(Graphics g)
        {
            int xf = 3;
            if (this._VerticalLineStep > 1)
                xf = 0;

            int startY = (this.ClientRectangle.Height * (100 - VerticalLineHeight)) / 100;
            int endY = this.ClientRectangle.Height;

            using (var solidPen = new Pen(this.PenColor, this.PenSize) { DashStyle = this.PenStyle })
            using (var dottedPen = new Pen(this.PenColor, this.PenSize) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot })
            {
                if (_characterPositions == null || _characterPositions.Length != MaxLength + 1)
                    CalculateCharacterPositions();

                // Riferimento per il disegno iniziale (bordo sinistro o destro)
                float initialX = (this.TextAlign == HorizontalAlignment.Left) ? xf : this.Width - xf;

                // Riga verticale iniziale
                g.DrawLine(solidPen, initialX, startY, initialX, endY);
                int xx = 0;

                // Disegna righe da 1 a MaxLength
                for (int i = 1; i <= MaxLength; i++)
                {
                    float position = _characterPositions[Math.Min(i, _characterPositions.Length - xx)];
                    position = position + PenSize;
                    int pixelX;

                    if (this.TextAlign == HorizontalAlignment.Left)
                        pixelX = (int)Math.Round(position) + xf;
                    else
                        pixelX = this.Width - (int)Math.Round(position) - xf;

                    if ((this.TextAlign == HorizontalAlignment.Left && pixelX < this.Width - xx) ||
                        (this.TextAlign != HorizontalAlignment.Left && pixelX > 0))
                    {
                        var penToUse = (i % VerticalLineStep == 0) ? solidPen : dottedPen;
                        g.DrawLine(penToUse, pixelX, startY, pixelX, endY);
                    }
                }

                // Disegna la riga verticale sul bordo finale, sempre a destra
                // Usiamo un valore fisso di offset (1) per stare sicuramente nel controllo
                float finalX = this.Width - 1;

                // Solo se l'allineamento è a destra disegniamo questa riga extra
                if (this.TextAlign != HorizontalAlignment.Left && this.VerticalLineStep >1)
                {
                    g.DrawLine(solidPen, finalX-2, startY, finalX-2, endY);
                }
            }
        }




      


        private void MeasureCharacterWidth(Graphics g)
        {
            // Approccio 1: Usare TextRenderer per una misurazione più precisa (solo su .NET Framework)
            //#if NET48
            try
            {
                using (var bitmap = new Bitmap(1, 1))
                using (var tempG = Graphics.FromImage(bitmap))
                {
                    var size = Wisej.Base.TextUtils.MeasureText("M", this.Font);
                    _charWidth = size.Width;
                    return;
                }
            }
            catch { /* Fallback al metodo standard */ }

        }


        private bool IsMonospaceFont()
        {
            using (var g = this.CreateGraphics())
            {
                float widthI = g.MeasureString("i", this.Font).Width;
                float widthW = g.MeasureString("W", this.Font).Width;
                return Math.Abs(widthI - widthW) < 0.5f;
            }
        }

        private int AverageCharWidth()
        {
            using (var g = this.CreateGraphics())
            {
                string testString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                SizeF size = g.MeasureString(testString, this.Font);
                return (int)(size.Width / testString.Length);
            }
        }
    }
}

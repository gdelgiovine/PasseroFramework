using System;
using System.Collections.Generic;
using System.Drawing;
using Wisej.Web;

namespace Passerow.Framework.ControlScaler  
{
    public enum CenteringType
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2,
        Both = 3
    }

    public enum ControlResizeType
    {
        AllPage = 0,
        Margin = 1,
        Proportional = 2,
    }

    public enum ScalingType
    {
        Auto = 0,           // Scala usando fattori diversi per X e Y
        Proportional = 1,   // Scala mantenendo le proporzioni
        FillPage = 2,       // Scala per occupare tutto lo spazio disponibile (può distorcere)
        None = 3            // Non scala
    }

    public class Scaler
    {
        // Struttura per memorizzare le dimensioni e posizioni originali
        private struct OriginalControlData
        {
            public Size Size;
            public Point Location;
            public Font Font;
        }

        // Dizionario per memorizzare i dati originali di tutti i controlli (FISSI dal designer)
        private readonly Dictionary<Control, OriginalControlData> originalControlsData;
        private readonly Size originalContainerSize; // FISSO dal designer
        private Control containerControl;

        public Scaler(Control container)
        {
            containerControl = container ?? throw new ArgumentNullException(nameof(container));
            originalControlsData = new Dictionary<Control, OriginalControlData>();

            // Cattura le dimensioni del DESIGNER (chiamato subito dopo InitializeComponent)
            originalContainerSize = containerControl.Size;
            StoreOriginalControlData(containerControl);
        }

        private void StoreOriginalControlData(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                // Memorizza i dati originali del controllo (dimensioni FISSE dal designer)
                originalControlsData[control] = new OriginalControlData
                {
                    Size = control.Size,
                    Location = control.Location,
                    Font = control.Font != null ? new Font(control.Font.FontFamily, control.Font.Size, control.Font.Style) : null
                };

                // Ricorsivamente memorizza i dati dei controlli figli
                if (control.Controls.Count > 0)
                {
                    StoreOriginalControlData(control);
                }
            }
        }

        public void ScaleAllControls(float scaleFactorX, float scaleFactorY)
        {
            ScaleControlsRecursive(containerControl, scaleFactorX, scaleFactorY);
        }

        private void ScaleControlsRecursive(Control parent, float scaleFactorX, float scaleFactorY)
        {
            foreach (Control control in parent.Controls)
            {
                if (originalControlsData.ContainsKey(control))
                {
                    var originalData = originalControlsData[control];

                    // SEMPRE basato sulle dimensioni ORIGINALI del designer
                    int newWidth = (int)(originalData.Size.Width * scaleFactorX);
                    int newHeight = (int)(originalData.Size.Height * scaleFactorY);
                    control.Size = new Size(newWidth, newHeight);

                    // Riscala posizione (solo se non è dockato)
                    if (control.Dock == DockStyle.None)
                    {
                        int newX = (int)(originalData.Location.X * scaleFactorX);
                        int newY = (int)(originalData.Location.Y * scaleFactorY);
                        control.Location = new Point(newX, newY);
                    }

                    // Riscala font (se presente)
                    if (originalData.Font != null && control.Font != null)
                    {
                        float newFontSize = originalData.Font.Size * Math.Min(scaleFactorX, scaleFactorY);
                        if (newFontSize >= 6.0f)
                        {
                            try
                            {
                                control.Font = new Font(originalData.Font.FontFamily, newFontSize, originalData.Font.Style);
                            }
                            catch
                            {
                                // Se fallisce la creazione del font, mantieni quello originale
                            }
                        }
                    }
                }

                // Ricorsivamente riscala i controlli figli
                if (control.Controls.Count > 0)
                {
                    ScaleControlsRecursive(control, scaleFactorX, scaleFactorY);
                }
            }
        }

        // METODO SPOSTATO DA Page1.cs - Scala i controlli all'interno di un panel specifico
        public void ScaleControlsInPanel(Control panel, float scaleFactorX, float scaleFactorY)
        {
            foreach (Control control in panel.Controls)
            {
                if (originalControlsData.ContainsKey(control))
                {
                    var originalData = originalControlsData[control];

                    // Riscala dimensioni basandosi su quelle originali
                    int newWidth = (int)(originalData.Size.Width * scaleFactorX);
                    int newHeight = (int)(originalData.Size.Height * scaleFactorY);
                    control.Size = new Size(newWidth, newHeight);

                    // Riscala posizione (solo se non è dockato)
                    if (control.Dock == DockStyle.None)
                    {
                        int newX = (int)(originalData.Location.X * scaleFactorX);
                        int newY = (int)(originalData.Location.Y * scaleFactorY);
                        control.Location = new Point(newX, newY);
                    }

                    // Riscala font usando le dimensioni ORIGINALI dal designer
                    ScaleFontFromOriginal(control, scaleFactorX, scaleFactorY);
                }

                // Ricorsivamente scala i controlli figli
                if (control.Controls.Count > 0)
                {
                    ScaleControlsInPanel(control, scaleFactorX, scaleFactorY);
                }
            }
        }

        // METODO SPOSTATO DA Page1.cs - Scala il font basandosi sulle dimensioni originali
        private void ScaleFontFromOriginal(Control control, float scaleFactorX, float scaleFactorY)
        {
            if (originalControlsData.ContainsKey(control))
            {
                var originalData = originalControlsData[control];
                if (originalData.Font != null)
                {
                    try
                    {
                        // Calcola la nuova dimensione basandosi sul font ORIGINALE
                        float scaleFactor = Math.Min(scaleFactorX, scaleFactorY);
                        float newFontSize = originalData.Font.Size * scaleFactor;

                        // Assicurati che la dimensione del font non sia troppo piccola
                        if (newFontSize >= 6.0f)
                        {
                            control.Font = new Font(originalData.Font.FontFamily, newFontSize, originalData.Font.Style);
                        }
                        else
                        {
                            // Se troppo piccolo, mantieni una dimensione minima
                            control.Font = new Font(originalData.Font.FontFamily, 6.0f, originalData.Font.Style);
                        }
                    }
                    catch
                    {
                        // Se fallisce la creazione del font, mantieni quello originale
                        control.Font = new Font(originalData.Font.FontFamily, originalData.Font.Size, originalData.Font.Style);
                    }
                }
            }
        }

        public void AutoScaleToContainer()
        {
            if (originalContainerSize.Width > 0 && originalContainerSize.Height > 0)
            {
                // SEMPRE basato sulle dimensioni ORIGINALI del designer
                float scaleFactorX = (float)containerControl.ClientSize.Width / originalContainerSize.Width;
                float scaleFactorY = (float)containerControl.ClientSize.Height / originalContainerSize.Height;

                ScaleAllControls(scaleFactorX, scaleFactorY);
            }
        }

        public void ProportionalScaleToContainer()
        {
            if (originalContainerSize.Width > 0 && originalContainerSize.Height > 0)
            {
                // SEMPRE basato sulle dimensioni ORIGINALI del designer
                float scaleFactorX = (float)containerControl.ClientSize.Width / originalContainerSize.Width;
                float scaleFactorY = (float)containerControl.ClientSize.Height / originalContainerSize.Height;

                // Usa il fattore di scala minore per mantenere le proporzioni
                float scaleFactor = Math.Min(scaleFactorX, scaleFactorY);

                ScaleAllControls(scaleFactor, scaleFactor);
            }
        }

        public void FillPageScaleToContainer()
        {
            if (originalContainerSize.Width > 0 && originalContainerSize.Height > 0)
            {
                // SEMPRE basato sulle dimensioni ORIGINALI del designer
                float scaleFactorX = (float)containerControl.ClientSize.Width / originalContainerSize.Width;
                float scaleFactorY = (float)containerControl.ClientSize.Height / originalContainerSize.Height;

                // Usa fattori di scala diversi per X e Y per riempire completamente la pagina
                ScaleAllControls(scaleFactorX, scaleFactorY);

                // Assicurati che i controlli principali partano da (0,0) per riempire completamente
                PositionControlsForFillPage();
            }
        }

        private void PositionControlsForFillPage()
        {
            // Posiziona i controlli principali per riempire completamente la pagina
            foreach (Control control in containerControl.Controls)
            {
                if (control.Dock == DockStyle.None)
                {
                    // Posiziona il controllo a (0,0) e ridimensionalo per occupare tutta la pagina
                    control.Location = new Point(0, 0);
                    control.Size = new Size(containerControl.ClientSize.Width, containerControl.ClientSize.Height);
                }
            }
        }

        public void ResetToOriginal()
        {
            ResetControlsRecursive(containerControl);
        }

        private void ResetControlsRecursive(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (originalControlsData.ContainsKey(control))
                {
                    var originalData = originalControlsData[control];

                    // Ripristina le dimensioni ORIGINALI del designer
                    control.Size = originalData.Size;

                    // Ripristina posizione originale (solo se non è dockato)
                    if (control.Dock == DockStyle.None)
                    {
                        control.Location = originalData.Location;
                    }

                    // Ripristina font originale
                    if (originalData.Font != null)
                    {
                        control.Font = new Font(originalData.Font.FontFamily, originalData.Font.Size, originalData.Font.Style);
                    }
                }

                // Ricorsivamente ripristina i controlli figli
                if (control.Controls.Count > 0)
                {
                    ResetControlsRecursive(control);
                }
            }
        }

        public void CenterControlInParent(Control control, CenteringType centeringType = CenteringType.Both)
        {
            if (control?.Parent == null) return;

            int x = control.Location.X;
            int y = control.Location.Y;

            bool centerHorizontally = (centeringType & CenteringType.Horizontal) == CenteringType.Horizontal;
            bool centerVertically = (centeringType & CenteringType.Vertical) == CenteringType.Vertical;

            if (centerHorizontally)
            {
                x = control.Parent.ClientSize.Width / 2 - control.Size.Width / 2;
            }

            if (centerVertically)
            {
                y = control.Parent.ClientSize.Height / 2 - control.Size.Height / 2;
            }

            control.Location = new Point(x, y);

            // Imposta l'anchor appropriato in base alla centratura
            AnchorStyles anchor = AnchorStyles.None;
            if (centeringType == CenteringType.None)
            {
                anchor = AnchorStyles.Top | AnchorStyles.Left;
            }
            else if (!centerHorizontally)
            {
                anchor = AnchorStyles.Left;
            }
            else if (!centerVertically)
            {
                anchor = AnchorStyles.Top;
            }

            control.Anchor = anchor;
        }

        public void ScaleToContainer(ScalingType scalingType)
        {
            switch (scalingType)
            {
                case ScalingType.Auto:
                    AutoScaleToContainer();
                    break;
                case ScalingType.Proportional:
                    ProportionalScaleToContainer();
                    break;
                case ScalingType.FillPage:
                    FillPageScaleToContainer();
                    break;
                case ScalingType.None:
                    // Non fare nulla
                    break;
            }
        }

        // Metodo alternativo per FillPage che scala solo i contenuti interni
        public void FillPageScaleContentsOnly()
        {
            if (originalContainerSize.Width > 0 && originalContainerSize.Height > 0)
            {
                float scaleFactorX = (float)containerControl.ClientSize.Width / originalContainerSize.Width;
                float scaleFactorY = (float)containerControl.ClientSize.Height / originalContainerSize.Height;

                // Scala solo i controlli figli, non i controlli principali
                foreach (Control control in containerControl.Controls)
                {
                    if (control.Controls.Count > 0)
                    {
                        ScaleControlsRecursive(control, scaleFactorX, scaleFactorY);
                    }
                }
            }
        }

        // Metodo per ottenere le dimensioni originali memorizzate (FISSE dal designer)
        public Size GetOriginalContainerSize()
        {
            return originalContainerSize;
        }

        // Metodo per ottenere i dati originali di un controllo specifico (FISSI dal designer)
        public bool TryGetOriginalControlData(Control control, out Size originalSize, out Point originalLocation)
        {
            if (originalControlsData.ContainsKey(control))
            {
                var data = originalControlsData[control];
                originalSize = data.Size;
                originalLocation = data.Location;
                return true;
            }

            originalSize = Size.Empty;
            originalLocation = Point.Empty;
            return false;
        }

        // Metodo per ottenere il font originale di un controllo specifico
        public bool TryGetOriginalControlFont(Control control, out Font originalFont)
        {
            if (originalControlsData.ContainsKey(control))
            {
                var data = originalControlsData[control];
                originalFont = data.Font;
                return originalFont != null;
            }

            originalFont = null;
            return false;
        }
    }
}
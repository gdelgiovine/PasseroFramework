using System;
using System.Collections.Generic;
using System.Drawing;
using Wisej.Web;

namespace Passero.Framework.Controls
{
    /// <summary>
    /// Utility per scalare i font di una griglia basandosi su incrementi/decrementi e memorizzando le dimensioni originali
    /// </summary>
    public static class GridFontScaler
    {
        // Dizionario per memorizzare le dimensioni originali per ogni griglia
        private static readonly Dictionary<Control, OriginalGridData> originalGridDataStore = new Dictionary<Control, OriginalGridData>();

        /// <summary>
        /// Struttura per memorizzare i dati originali di una griglia
        /// </summary>
        private class OriginalGridData
        {
            public Font GridFont { get; set; }
            public Font DefaultCellStyleFont { get; set; }
            public Font ColumnHeadersFont { get; set; }
            public Font RowHeadersFont { get; set; }
            public Dictionary<string, ColumnOriginalData> ColumnsData { get; set; } = new Dictionary<string, ColumnOriginalData>();
            public Dictionary<string, CellOriginalData> CellsData { get; set; } = new Dictionary<string, CellOriginalData>();
            public Dictionary<Control, ControlOriginalData> ControlsData { get; set; } = new Dictionary<Control, ControlOriginalData>();
        }

        /// <summary>
        /// Struttura per memorizzare i dati originali di una colonna
        /// </summary>
        private class ColumnOriginalData
        {
            public Font DefaultCellStyleFont { get; set; }
            public Font HeaderStyleFont { get; set; }
            public int Width { get; set; }
        }

        /// <summary>
        /// Struttura per memorizzare i dati originali di una cella
        /// </summary>
        private class CellOriginalData
        {
            public Font StyleFont { get; set; }
        }

        /// <summary>
        /// Struttura per memorizzare i dati originali di un controllo
        /// </summary>
        private class ControlOriginalData
        {
            public Font Font { get; set; }
            public Font InnerControlFont { get; set; }
        }

        /// <summary>
        /// Memorizza le dimensioni originali di una griglia per permettere il reset e il scaling incrementale
        /// Include anche le DataGridView contenute all'interno
        /// </summary>
        /// <param name="grid">La griglia di cui memorizzare le dimensioni originali</param>
        public static void StoreOriginalDimensions(Control grid)
        {
            if (grid == null) return;

            if (grid is DataGridView dataGridView)
            {
                var originalData = new OriginalGridData();

                // Memorizza i font della griglia
                originalData.GridFont = dataGridView.Font != null ? new Font(dataGridView.Font.FontFamily, dataGridView.Font.Size, dataGridView.Font.Style) : null;
                originalData.DefaultCellStyleFont = dataGridView.DefaultCellStyle?.Font != null ? new Font(dataGridView.DefaultCellStyle.Font.FontFamily, dataGridView.DefaultCellStyle.Font.Size, dataGridView.DefaultCellStyle.Font.Style) : null;
                originalData.ColumnHeadersFont = dataGridView.ColumnHeadersDefaultCellStyle?.Font != null ? new Font(dataGridView.ColumnHeadersDefaultCellStyle.Font.FontFamily, dataGridView.ColumnHeadersDefaultCellStyle.Font.Size, dataGridView.ColumnHeadersDefaultCellStyle.Font.Style) : null;
                originalData.RowHeadersFont = dataGridView.RowHeadersDefaultCellStyle?.Font != null ? new Font(dataGridView.RowHeadersDefaultCellStyle.Font.FontFamily, dataGridView.RowHeadersDefaultCellStyle.Font.Size, dataGridView.RowHeadersDefaultCellStyle.Font.Style) : null;

                // Memorizza i dati delle colonne
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    var columnData = new ColumnOriginalData
                    {
                        DefaultCellStyleFont = column.DefaultCellStyle?.Font != null ? new Font(column.DefaultCellStyle.Font.FontFamily, column.DefaultCellStyle.Font.Size, column.DefaultCellStyle.Font.Style) : null,
                        HeaderStyleFont = column.HeaderStyle?.Font != null ? new Font(column.HeaderStyle.Font.FontFamily, column.HeaderStyle.Font.Size, column.HeaderStyle.Font.Style) : null,
                        Width = column.Width
                    };
                    originalData.ColumnsData[column.Name] = columnData;
                }

                // Memorizza i dati delle celle e dei controlli
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string cellKey = $"{row.Index}_{cell.ColumnIndex}";
                        
                        // Memorizza il font della cella
                        if (cell.Style?.Font != null)
                        {
                            originalData.CellsData[cellKey] = new CellOriginalData
                            {
                                StyleFont = new Font(cell.Style.Font.FontFamily, cell.Style.Font.Size, cell.Style.Font.Style)
                            };
                        }

                        // Memorizza i dati dei controlli associati alle celle
                        if (cell.Control != null)
                        {
                            StoreControlOriginalData(cell.Control, originalData.ControlsData);
                        }
                    }
                }

                // Salva nel dizionario
                originalGridDataStore[grid] = originalData;
            }

            // **NUOVO: Cerca ricorsivamente altre DataGridView contenute nel controllo**
            FindAndStoreNestedDataGridViews(grid);
        }

        /// <summary>
        /// Cerca ricorsivamente DataGridView annidate e memorizza le loro dimensioni originali
        /// </summary>
        private static void FindAndStoreNestedDataGridViews(Control parentControl)
        {
            if (parentControl == null) return;

            foreach (Control control in parentControl.Controls)
            {
                // Se è una DataGridView e non è già memorizzata, memorizzala
                if (control is DataGridView nestedDataGridView && !originalGridDataStore.ContainsKey(nestedDataGridView))
                {
                    StoreOriginalDimensions(nestedDataGridView);
                }

                // Cerca ricorsivamente nei controlli figli
                if (control.Controls.Count > 0)
                {
                    FindAndStoreNestedDataGridViews(control);
                }
            }
        }

        /// <summary>
        /// Memorizza ricorsivamente i dati originali di un controllo
        /// Include anche le DataGridView contenute nel controllo
        /// </summary>
        private static void StoreControlOriginalData(Control control, Dictionary<Control, ControlOriginalData> controlsData)
        {
            var controlData = new ControlOriginalData();

            // Memorizza il font del controllo principale
            if (control.Font != null)
            {
                controlData.Font = new Font(control.Font.FontFamily, control.Font.Size, control.Font.Style);
            }

            // Gestione specifica per controlli con InnerControl
            var innerControlProperty = control.GetType().GetProperty("InnerControl");
            if (innerControlProperty != null)
            {
                var innerControl = innerControlProperty.GetValue(control) as Control;
                if (innerControl?.Font != null)
                {
                    controlData.InnerControlFont = new Font(innerControl.Font.FontFamily, innerControl.Font.Size, innerControl.Font.Style);
                }
            }

            controlsData[control] = controlData;

            // **NUOVO: Se il controllo è una DataGridView, memorizza anche le sue dimensioni**
            if (control is DataGridView nestedDataGridView && !originalGridDataStore.ContainsKey(nestedDataGridView))
            {
                StoreOriginalDimensions(nestedDataGridView);
            }

            // Memorizza ricorsivamente i controlli figli
            foreach (Control childControl in control.Controls)
            {
                StoreControlOriginalData(childControl, controlsData);
            }
        }

        /// <summary>
        /// Scala una griglia usando un valore di incremento/decremento rispetto alle dimensioni originali
        /// Include anche le DataGridView contenute all'interno
        /// </summary>
        /// <param name="grid">La griglia da scalare</param>
        /// <param name="scaleIncrement">Incremento di scala: 0 = dimensioni originali, +10 = 10% più grande, -20 = 20% più piccolo</param>
        /// <param name="minimumFontSize">Dimensione minima del font (default: 6)</param>
        /// <param name="maximumFontSize">Dimensione massima del font (default: 72)</param>
        public static void ScaleGridFromOriginal(Control grid, float scaleIncrement, float minimumFontSize = 6f, float maximumFontSize = 72f)
        {
            if (grid == null)
                throw new ArgumentNullException(nameof(grid));

            // Se non abbiamo i dati originali, li memorizziamo ora
            if (!originalGridDataStore.ContainsKey(grid))
            {
                StoreOriginalDimensions(grid);
            }

            // Calcola il fattore di scala: 0 = 100%, +10 = 110%, -20 = 80%
            float scalePercentage = 100f + scaleIncrement;
            float scaleFactor = scalePercentage / 100f;

            if (grid is DataGridView dataGridView && originalGridDataStore.ContainsKey(grid))
            {
                ScaleDataGridViewFromOriginal(dataGridView, originalGridDataStore[grid], scaleFactor, minimumFontSize, maximumFontSize);
            }

            // **NUOVO: Scala anche tutte le DataGridView annidate**
            ScaleNestedDataGridViews(grid, scaleIncrement, minimumFontSize, maximumFontSize);
        }

        /// <summary>
        /// Scala ricorsivamente tutte le DataGridView contenute in un controllo
        /// </summary>
        private static void ScaleNestedDataGridViews(Control parentControl, float scaleIncrement, float minimumFontSize, float maximumFontSize)
        {
            if (parentControl == null) return;

            float scalePercentage = 100f + scaleIncrement;
            float scaleFactor = scalePercentage / 100f;

            foreach (Control control in parentControl.Controls)
            {
                // Se è una DataGridView, scalala
                if (control is DataGridView nestedDataGridView && originalGridDataStore.ContainsKey(nestedDataGridView))
                {
                    ScaleDataGridViewFromOriginal(nestedDataGridView, originalGridDataStore[nestedDataGridView], scaleFactor, minimumFontSize, maximumFontSize);
                }

                // Cerca ricorsivamente nei controlli figli
                if (control.Controls.Count > 0)
                {
                    ScaleNestedDataGridViews(control, scaleIncrement, minimumFontSize, maximumFontSize);
                }
            }
        }

        /// <summary>
        /// Scala una DataGridView basandosi sui dati originali memorizzati
        /// </summary>
        private static void ScaleDataGridViewFromOriginal(DataGridView dataGridView, OriginalGridData originalData, float scaleFactor, float minimumFontSize, float maximumFontSize)
        {
            try
            {
                // Scala i font principali della griglia
                if (originalData.GridFont != null)
                {
                    float newSize = CalculateScaledFontSize(originalData.GridFont.Size, scaleFactor, minimumFontSize, maximumFontSize);
                    dataGridView.Font = CreateScaledFont(originalData.GridFont, newSize);
                }

                if (originalData.DefaultCellStyleFont != null)
                {
                    float newSize = CalculateScaledFontSize(originalData.DefaultCellStyleFont.Size, scaleFactor, minimumFontSize, maximumFontSize);
                    dataGridView.DefaultCellStyle.Font = CreateScaledFont(originalData.DefaultCellStyleFont, newSize);
                }

                if (originalData.ColumnHeadersFont != null)
                {
                    float newSize = CalculateScaledFontSize(originalData.ColumnHeadersFont.Size, scaleFactor, minimumFontSize, maximumFontSize);
                    dataGridView.ColumnHeadersDefaultCellStyle.Font = CreateScaledFont(originalData.ColumnHeadersFont, newSize);
                }

                if (originalData.RowHeadersFont != null)
                {
                    float newSize = CalculateScaledFontSize(originalData.RowHeadersFont.Size, scaleFactor, minimumFontSize, maximumFontSize);
                    dataGridView.RowHeadersDefaultCellStyle.Font = CreateScaledFont(originalData.RowHeadersFont, newSize);
                }

                // Scala le colonne
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (originalData.ColumnsData.ContainsKey(column.Name))
                    {
                        var columnData = originalData.ColumnsData[column.Name];

                        // Scala i font della colonna
                        if (columnData.DefaultCellStyleFont != null)
                        {
                            float newSize = CalculateScaledFontSize(columnData.DefaultCellStyleFont.Size, scaleFactor, minimumFontSize, maximumFontSize);
                            column.DefaultCellStyle.Font = CreateScaledFont(columnData.DefaultCellStyleFont, newSize);
                        }

                        if (columnData.HeaderStyleFont != null)
                        {
                            float newSize = CalculateScaledFontSize(columnData.HeaderStyleFont.Size, scaleFactor, minimumFontSize, maximumFontSize);
                            column.HeaderStyle.Font = CreateScaledFont(columnData.HeaderStyleFont, newSize);
                        }

                        // Scala la larghezza della colonna (se non è AutoSize)
                        if (column.AutoSizeMode == DataGridViewAutoSizeColumnMode.None || 
                            column.AutoSizeMode == DataGridViewAutoSizeColumnMode.NotSet)
                        {
                            int newWidth = (int)(columnData.Width * scaleFactor);
                            const int MIN_COLUMN_WIDTH = 10;
                            const int MAX_COLUMN_WIDTH = 1000;
                            newWidth = Math.Max(MIN_COLUMN_WIDTH, Math.Min(MAX_COLUMN_WIDTH, newWidth));
                            column.Width = newWidth;
                        }
                    }
                }

                // Scala le celle e i controlli
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string cellKey = $"{row.Index}_{cell.ColumnIndex}";
                        
                        // Scala il font della cella
                        if (originalData.CellsData.ContainsKey(cellKey) && originalData.CellsData[cellKey].StyleFont != null)
                        {
                            float newSize = CalculateScaledFontSize(originalData.CellsData[cellKey].StyleFont.Size, scaleFactor, minimumFontSize, maximumFontSize);
                            cell.Style.Font = CreateScaledFont(originalData.CellsData[cellKey].StyleFont, newSize);
                        }

                        // Scala i controlli associati alle celle
                        if (cell.Control != null)
                        {
                            ScaleCellControlFromOriginal(cell.Control, originalData.ControlsData, scaleFactor, minimumFontSize, maximumFontSize);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore durante lo scaling dalla dimensione originale: {ex.Message}");
            }
        }

        /// <summary>
        /// Scala un controllo di cella basandosi sui dati originali memorizzati
        /// Include anche le DataGridView contenute nel controllo
        /// </summary>
        private static void ScaleCellControlFromOriginal(Control cellControl, Dictionary<Control, ControlOriginalData> controlsData, float scaleFactor, float minimumFontSize, float maximumFontSize)
        {
            try
            {
                if (controlsData.ContainsKey(cellControl))
                {
                    var controlData = controlsData[cellControl];

                    // Scala il font del controllo principale
                    if (controlData.Font != null)
                    {
                        float newSize = CalculateScaledFontSize(controlData.Font.Size, scaleFactor, minimumFontSize, maximumFontSize);
                        cellControl.Font = CreateScaledFont(controlData.Font, newSize);
                    }

                    // Scala il font dell'InnerControl
                    if (controlData.InnerControlFont != null)
                    {
                        var innerControlProperty = cellControl.GetType().GetProperty("InnerControl");
                        if (innerControlProperty != null)
                        {
                            var innerControl = innerControlProperty.GetValue(cellControl) as Control;
                            if (innerControl != null)
                            {
                                float newSize = CalculateScaledFontSize(controlData.InnerControlFont.Size, scaleFactor, minimumFontSize, maximumFontSize);
                                innerControl.Font = CreateScaledFont(controlData.InnerControlFont, newSize);
                            }
                        }
                    }
                }

                // **NUOVO: Se il controllo è una DataGridView, scalala**
                if (cellControl is DataGridView nestedDataGridView && originalGridDataStore.ContainsKey(nestedDataGridView))
                {
                    ScaleDataGridViewFromOriginal(nestedDataGridView, originalGridDataStore[nestedDataGridView], scaleFactor, minimumFontSize, maximumFontSize);
                }

                // Scala ricorsivamente i controlli figli
                foreach (Control childControl in cellControl.Controls)
                {
                    ScaleCellControlFromOriginal(childControl, controlsData, scaleFactor, minimumFontSize, maximumFontSize);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore durante lo scaling del controllo cella: {ex.Message}");
            }
        }

        /// <summary>
        /// Ripristina una griglia alle sue dimensioni originali
        /// Include anche tutte le DataGridView contenute all'interno
        /// </summary>
        /// <param name="grid">La griglia da ripristinare</param>
        public static void ResetToOriginalDimensions(Control grid)
        {
            ScaleGridFromOriginal(grid, 0f); // 0 = dimensioni originali (100%)
        }

        /// <summary>
        /// Rimuove i dati originali memorizzati per una griglia (per liberare memoria)
        /// Include anche tutte le DataGridView contenute all'interno
        /// </summary>
        /// <param name="grid">La griglia di cui rimuovere i dati memorizzati</param>
        public static void ClearStoredOriginalDimensions(Control grid)
        {
            if (grid != null && originalGridDataStore.ContainsKey(grid))
            {
                originalGridDataStore.Remove(grid);
            }

            // **NUOVO: Rimuovi anche i dati delle DataGridView annidate**
            ClearNestedDataGridViewsData(grid);
        }

        /// <summary>
        /// Rimuove ricorsivamente i dati delle DataGridView annidate
        /// </summary>
        private static void ClearNestedDataGridViewsData(Control parentControl)
        {
            if (parentControl == null) return;

            foreach (Control control in parentControl.Controls)
            {
                // Se è una DataGridView, rimuovi i suoi dati
                if (control is DataGridView nestedDataGridView && originalGridDataStore.ContainsKey(nestedDataGridView))
                {
                    originalGridDataStore.Remove(nestedDataGridView);
                }

                // Cerca ricorsivamente nei controlli figli
                if (control.Controls.Count > 0)
                {
                    ClearNestedDataGridViewsData(control);
                }
            }
        }

        /// <summary>
        /// Scala tutte le DataGridView contenute in un form o controllo contenitore
        /// </summary>
        /// <param name="container">Il contenitore da cui iniziare la ricerca</param>
        /// <param name="scaleIncrement">Incremento di scala</param>
        /// <param name="minimumFontSize">Dimensione minima del font</param>
        /// <param name="maximumFontSize">Dimensione massima del font</param>
        public static void ScaleAllDataGridViewsInContainer(Control container, float scaleIncrement, float minimumFontSize = 6f, float maximumFontSize = 72f)
        {
            if (container == null) return;

            // Trova e scala tutte le DataGridView nel contenitore
            FindAndScaleAllDataGridViews(container, scaleIncrement, minimumFontSize, maximumFontSize);
        }

        /// <summary>
        /// Trova e scala ricorsivamente tutte le DataGridView in un contenitore
        /// </summary>
        private static void FindAndScaleAllDataGridViews(Control parentControl, float scaleIncrement, float minimumFontSize, float maximumFontSize)
        {
            if (parentControl == null) return;

            foreach (Control control in parentControl.Controls)
            {
                // Se è una DataGridView, scalala
                if (control is DataGridView dataGridView)
                {
                    ScaleGridFromOriginal(dataGridView, scaleIncrement, minimumFontSize, maximumFontSize);
                }

                // Cerca ricorsivamente nei controlli figli
                if (control.Controls.Count > 0)
                {
                    FindAndScaleAllDataGridViews(control, scaleIncrement, minimumFontSize, maximumFontSize);
                }
            }
        }

        /// <summary>
        /// Calcola la nuova dimensione del font basandosi sul fattore di scala
        /// </summary>
        private static float CalculateScaledFontSize(float originalFontSize, float scaleFactor, float minimumFontSize, float maximumFontSize)
        {
            float scaledSize = originalFontSize * scaleFactor;
            return Math.Max(minimumFontSize, Math.Min(maximumFontSize, scaledSize));
        }

        /// <summary>
        /// Crea un nuovo font con la dimensione scalata mantenendo famiglia e stile
        /// </summary>
        private static Font CreateScaledFont(Font originalFont, float newSize)
        {
            try
            {
                return new Font(originalFont.FontFamily, newSize, originalFont.Style);
            }
            catch
            {
                return originalFont;
            }
        }

        #region Metodi legacy per compatibilità

        /// <summary>
        /// [LEGACY] Scala tutti i font degli oggetti di una griglia basandosi su una percentuale
        /// </summary>
        [Obsolete("Usa ScaleGridFromOriginal per un controllo migliore dello scaling")]
        public static void ScaleGridFonts(Control grid, float scalePercentage, float minimumFontSize = 6f, float maximumFontSize = 72f)
        {
            // Converte la percentuale in incremento per il nuovo sistema
            float scaleIncrement = scalePercentage - 100f;
            ScaleGridFromOriginal(grid, scaleIncrement, minimumFontSize, maximumFontSize);
        }

        /// <summary>
        /// [LEGACY] Ripristina i font di una griglia alla dimensione standard (100%)
        /// </summary>
        [Obsolete("Usa ResetToOriginalDimensions")]
        public static void ResetGridFontsToStandard(Control grid)
        {
            ResetToOriginalDimensions(grid);
        }

        #endregion
    }
}
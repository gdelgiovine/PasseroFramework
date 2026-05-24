using System;
using System.Drawing;

namespace Passero.Framework.Controls;

internal static class QueryBuilderUIHelper
{
    /// <summary>
    /// Imposta la larghezza del ComboBox in modo che contenga il testo dell'item più lungo.
    /// Usa misurazione in pixel per avere risultati accurati con font correnti.
    /// </summary>
    public static void FitComboToContent(Wisej.Web.ComboBox comboBox, int maxAllowedWidth = 600)
    {
        if (comboBox == null) return;

        try
        {
            using var bmp = new Bitmap(1, 1);
            using var g = Graphics.FromImage(bmp);

            var font = comboBox.Font ?? SystemFonts.DefaultFont;
            int maxWidth = 0;

            foreach (var item in comboBox.Items)
            {
                var text = item?.ToString() ?? string.Empty;
                var sizeF = g.MeasureString(text, font);
                var w = (int)Math.Ceiling(sizeF.Width);
                if (w > maxWidth) maxWidth = w;
            }

            // Aggiunge spazio per padding e freccia dropdown
            var final = maxWidth + 32;
            if (final < 60) final = 60;
            if (final > maxAllowedWidth) final = maxAllowedWidth;

            comboBox.Width = final;
        }
        catch
        {
            // In caso di errore non blocchiamo l'esecuzione; manteniamo la width corrente
        }
    }
}

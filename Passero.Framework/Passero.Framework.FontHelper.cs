using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{
    public static class FontHelper
    {

        public static bool IsMonospaceFont(Font font)
        {
            using (var bitmap = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(bitmap))
            {
                float widthI = g.MeasureString("i", font).Width;
                float widthW = g.MeasureString("W", font).Width;
                return Math.Abs(widthI - widthW) < 0.5f;
            }
        }

        public static int AverageCharWidth(Font font)
        {
            using (var bitmap = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(bitmap))
            {
                string testString = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                SizeF size = g.MeasureString(testString, font);
                return (int)(size.Width / testString.Length);
            }
        }
    }
}

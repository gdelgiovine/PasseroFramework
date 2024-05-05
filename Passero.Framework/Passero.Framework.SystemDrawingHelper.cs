using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{
    public class SystemDrawingHelper
    {

        public static System.Drawing.Image SafeImageFromFile(string path)
        {
            byte[] bytesArr = null;
            if (File.Exists(path) == false)
            {
                return null;
            }

            bytesArr = File.ReadAllBytes(path);
            MemoryStream memstr = new MemoryStream(bytesArr);
            System.Drawing.Image img = System.Drawing.Image.FromStream(memstr);
            return img;
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn, System.Drawing.Imaging.ImageFormat Format = null)
        {
            if (imageIn is object)
            {
                var ms = new MemoryStream();
                if (Format is null)
                {
                    Format = System.Drawing.Imaging.ImageFormat.Jpeg;
                }

                imageIn.Save(ms, Format);
                return ms.ToArray();
            }
            else
            {
                return null;
            }
        }

        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            System.Drawing.Image returnImage = null;
            if (byteArrayIn is object)
            {
                var ms = new MemoryStream(byteArrayIn);
                returnImage = System.Drawing.Image.FromStream(ms);
                return returnImage;
            }

            return returnImage;
        }


    }


}

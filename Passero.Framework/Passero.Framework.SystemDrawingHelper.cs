using System.IO;

namespace Passero.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDrawingHelper
    {

        /// <summary>
        /// Safes the image from file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Images to byte array.
        /// </summary>
        /// <param name="imageIn">The image in.</param>
        /// <param name="Format">The format.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Bytes the array to image.
        /// </summary>
        /// <param name="byteArrayIn">The byte array in.</param>
        /// <returns></returns>
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

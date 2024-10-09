using System;
using System.IO;

namespace Passero.Framework
{

    /// <summary>
    /// 
    /// </summary>
    public static class FileHelper
    {

        /// <summary>
        /// Files to memory stream.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <returns></returns>
        public static MemoryStream FileToMemoryStream(object FileName)
        {
            if (File.Exists(Convert.ToString(FileName)) == false)
            {
                return null;
            }

            byte[] bData;
            BinaryReader br = new BinaryReader(File.OpenRead(Convert.ToString(FileName)));
            bData = br.ReadBytes((int)br.BaseStream.Length);
            MemoryStream ms = new MemoryStream();
            ms.Write(bData, 0, bData.Length);
            return ms;
        }

        /// <summary>
        /// Files to file stream.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <returns></returns>
        public static FileStream FileToFileStream(object FileName)
        {
            if (File.Exists(Convert.ToString(FileName)) == false)
            {
                return null;
            }

            FileStream ms = new FileStream(Convert.ToString(FileName), FileMode.Open);
            return ms;
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="Path">The path.</param>
        /// <returns></returns>
        public static string GetFileName(string Path)
        {
            return System.IO.Path.GetFileName(Path);
        }

        /// <summary>
        /// Gets the name of the unique file.
        /// </summary>
        /// <param name="FileName">Name of the file.</param>
        /// <returns></returns>
        public static string GetUniqueFileName(string FileName)
        {
            string _Path = FileName;
            string _FileName = GetFileNameWithoutExtension(FileName);
            string _FileNameComplete = GetFileName(FileName);
            string _Extension = GetFileExtension(FileName);
            if (FileName.EndsWith(_FileNameComplete))
            {
                _Path = FileName.Substring(0, FileName.Length - _FileNameComplete.Length - 1);
            }

            string _NewFileName = _Path + _FileName + _Extension;
            if (System.IO.File.Exists(_NewFileName))
            {
                int i = 1;
                do
                {
                    string s = "_" + i;
                    _NewFileName = _Path + _FileName + s + _Extension;
                    if (System.IO.File.Exists(_NewFileName) == false)
                    {
                        break;
                    }

                    i = i + 1;
                    if (i > 2000)
                    {
                        break;
                    }
                }
                while (true);
            }

            return _NewFileName;
        }

        /// <summary>
        /// Gets the name of the temporary file.
        /// </summary>
        /// <returns></returns>
        public static string GetTempFileName()
        {
            return System.IO.Path.GetTempPath() + @"\" + System.Guid.NewGuid().ToString();
        }
        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <param name="UserInput">The user input.</param>
        /// <returns></returns>
        public static string GetFileExtension(string UserInput)
        {
            return System.IO.Path.GetExtension(UserInput);
        }

        /// <summary>
        /// Gets the file name without extension.
        /// </summary>
        /// <param name="UserInput">The user input.</param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string UserInput)
        {
            return System.IO.Path.GetFileNameWithoutExtension(UserInput);
        }

        /// <summary>
        /// Gets the name of the safe file.
        /// </summary>
        /// <param name="UserInput">The user input.</param>
        /// <returns></returns>
        public static string GetSafeFileName(string UserInput)
        {
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                UserInput = UserInput.Replace(Convert.ToString(invalidChar), "");
            return UserInput;
        }

    }


}

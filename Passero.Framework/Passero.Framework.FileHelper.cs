using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{

    public static class FileHelper
    {

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

        public static FileStream FileToFileStream(object FileName)
        {
            if (File.Exists(Convert.ToString(FileName)) == false)
            {
                return null;
            }

            FileStream ms = new FileStream(Convert.ToString(FileName), FileMode.Open);
            return ms;
        }

        public static string GetFileName(string Path)
        {
            return System.IO.Path.GetFileName(Path);
        }

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

        public static string GetTempFileName()
        {
            return System.IO.Path.GetTempPath() + @"\" + System.Guid.NewGuid().ToString();
        }
        public static string GetFileExtension(string UserInput)
        {
            return System.IO.Path.GetExtension(UserInput);
        }

        public static string GetFileNameWithoutExtension(string UserInput)
        {
            return System.IO.Path.GetFileNameWithoutExtension(UserInput);
        }

        public static string GetSafeFileName(string UserInput)
        {
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                UserInput = UserInput.Replace(Convert.ToString(invalidChar), "");
            return UserInput;
        }

    }


}

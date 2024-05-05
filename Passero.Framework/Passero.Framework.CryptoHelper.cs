using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{
    public class CryptoHelper
    {
        public enum HashProviders
        {
            MD5 = 0,
            SHA256 = 1,
            SHA384 = 2,
            SHA512 = 3
        }
        public static string GetHashValue(string SourceText, HashProviders HashProvider = HashProviders.MD5)
        {
            var Ue = new UnicodeEncoding();
            var ByteSourceText = Ue.GetBytes(SourceText);
            byte[] ByteHash = null;
            switch (HashProvider)
            {
                case HashProviders.MD5:
                    {
                        System.Security.Cryptography.MD5CryptoServiceProvider Md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        ByteHash = Md5.ComputeHash(ByteSourceText);
                        break;
                    }

                case HashProviders.SHA256:
                    {
                        System.Security.Cryptography.SHA256CryptoServiceProvider SHA256 = new System.Security.Cryptography.SHA256CryptoServiceProvider();
                        ByteHash = SHA256.ComputeHash(ByteSourceText);
                        break;
                    }

                case HashProviders.SHA384:
                    {
                        System.Security.Cryptography.SHA384CryptoServiceProvider SHA384 = new System.Security.Cryptography.SHA384CryptoServiceProvider();
                        ByteHash = SHA384.ComputeHash(ByteSourceText);
                        break;
                    }

                case HashProviders.SHA512:
                    {
                        System.Security.Cryptography.SHA512CryptoServiceProvider SHA512 = new System.Security.Cryptography.SHA512CryptoServiceProvider();
                        ByteHash = SHA512.ComputeHash(ByteSourceText);
                        break;
                    }

                default:
                    {
                        System.Security.Cryptography.MD5CryptoServiceProvider Md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        ByteHash = Md5.ComputeHash(ByteSourceText);
                        break;
                    }
            }

            return Convert.ToBase64String(ByteHash);
        }


    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passero.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public class CryptoHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public enum HashProviders
        {
            /// <summary>
            /// The m d5
            /// </summary>
            MD5 = 0,
            /// <summary>
            /// The sh a256
            /// </summary>
            SHA256 = 1,
            /// <summary>
            /// The sh a384
            /// </summary>
            SHA384 = 2,
            /// <summary>
            /// The sh a512
            /// </summary>
            SHA512 = 3
        }
        /// <summary>
        /// Gets the hash value.
        /// </summary>
        /// <param name="SourceText">The source text.</param>
        /// <param name="HashProvider">The hash provider.</param>
        /// <returns></returns>
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

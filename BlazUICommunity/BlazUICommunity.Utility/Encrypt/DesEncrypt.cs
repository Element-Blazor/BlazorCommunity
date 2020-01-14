using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlazUICommunity.Utility
{
    /// <summary>
    /// DES AES Blowfish
    ///  对称加密算法的优点是速度快，
    ///  缺点是密钥管理不方便，要求共享密钥。
    /// 可逆对称加密  密钥长度8
    /// </summary>
    public class DesEncrypt
    {
        private static byte[] _rgbKey = Encoding.ASCII.GetBytes(DesEncryptExtensions.DesKey.Substring(0, 8));
        private static byte[] _rgbIV = Encoding.ASCII.GetBytes(DesEncryptExtensions.DesKey.Insert(0, "w").Substring(0, 8));

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="text">需要加密的值</param>
        /// <returns>加密后的结果</returns>
        public static string Encrypt(string text)
        {
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            using MemoryStream memStream = new MemoryStream();
            CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateEncryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
            StreamWriter sWriter = new StreamWriter(crypStream);
            sWriter.Write(text);
            sWriter.Flush();
            crypStream.FlushFinalBlock();
            memStream.Flush();
            return Convert.ToBase64String(memStream.GetBuffer(), 0, (int)memStream.Length);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptText"></param>
        /// <returns>解密后的结果</returns>
        public static string Decrypt(string encryptText)
        {
            DESCryptoServiceProvider dsp = new DESCryptoServiceProvider();
            byte[] buffer = Convert.FromBase64String(encryptText);

            using MemoryStream memStream = new MemoryStream();
            CryptoStream crypStream = new CryptoStream(memStream, dsp.CreateDecryptor(_rgbKey, _rgbIV), CryptoStreamMode.Write);
            crypStream.Write(buffer, 0, buffer.Length);
            crypStream.FlushFinalBlock();
            return ASCIIEncoding.UTF8.GetString(memStream.ToArray());
        }
    }
}

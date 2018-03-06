using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;

namespace Helper
{
    public static class HashHelper
    {
        public static string MD5(string word) 
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(word));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
        public static string MD5(int number)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(number.ToString()));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
        public static string Token()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray()) i *= ((int)b + 1);
            return MD5(string.Format("{0:x}", i - DateTime.Now.Ticks));
        }
        public static string DataSecurity(int id)
        {
            // 2) Lo parte en dos
            string token = HashHelper.Token();
            string[] arrToken = new string[2];

            arrToken[0] = token.Substring(0, 7);
            arrToken[1] = token.Substring(16, 6);

            token = arrToken[0] + arrToken[1];

            return arrToken[0] + MD5(id.ToString()) + MD5(token) + arrToken[1];
        }
        public static string DataSecurity(string word)
        {
            // 2) Lo parte en dos
            string token = HashHelper.Token();
            string[] arrToken = new string[2];

            arrToken[0] = token.Substring(0, 7);
            arrToken[1] = token.Substring(16, 6);

            token = arrToken[0] + arrToken[1];

            return arrToken[0] + MD5(word) + MD5(token) + arrToken[1];
        }
        public static string Base64Encode(string word) 
        {
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(word);
            return Convert.ToBase64String(byt);
        }
        public static string Base64Decode(string word)
        {
            byte[] b = Convert.FromBase64String(word);
            return System.Text.Encoding.UTF8.GetString(b);
        }
    }
}
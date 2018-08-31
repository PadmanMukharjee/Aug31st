using System;
using System.Text.RegularExpressions;

namespace M3Pact.Infrastructure.Common
{
    public class HashingUtility
    {
        #region Public Methods

        /// <summary>
        /// Encodes given string to base64 string
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            Byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Verifies if given string is base64 string
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) && Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        /// <summary>
        /// Decodes given base64 string to plain text string
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static string Base64Decode(string base64EncodedData)
        {

            if (!IsBase64String(base64EncodedData))
            {
                return string.Empty;
            }
            Byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        #endregion
    }
}

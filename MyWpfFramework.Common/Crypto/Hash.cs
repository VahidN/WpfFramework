using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace MyWpfFramework.Common.Crypto
{
    /// <summary>
    /// جهت هش کردن کلمه عبور اضافه شده است
    /// </summary>
    public static class Hash
    {
        /// <summary>
        /// هش کردن آرایه‌ای از بایت‌ها
        /// </summary>
        /// <param name="inputBytes">آرایه‌ای از بایت‌ها</param>
        /// <returns>رشته حاوی هش ورودی</returns>
        public static string SHA1Hash(this byte[] inputBytes)
        {
            // step 1, calculate sha1 hash from input
            SHA1 sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2", CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }

        /// <summary>
        /// هش کردن یک رشته
        /// </summary>
        /// <param name="strPass">رشته ورودی</param>
        /// <returns>رشته حاوی هش ورودی</returns>
        public static string SHA1Hash(this string strPass)
        {
            if (string.IsNullOrWhiteSpace(strPass))
                return string.Empty;

            byte[] data = Encoding.UTF8.GetBytes(strPass);
            return SHA1Hash(data);
        }
    }
}
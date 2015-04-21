namespace MyWpfFramework.Common.PersianToolkit
{
    /// <summary>
    /// برای یک دست سازی ی و ک در برنامه بکار می‌رود
    /// </summary>
    public static class YeKe
    {
        /// <summary>
        /// ی عربی
        /// </summary>
        public const char ArabicYeChar = (char)1610;

        /// <summary>
        /// ی فارسی
        /// </summary>
        public const char PersianYeChar = (char)1740;

        /// <summary>
        /// ک عربی
        /// </summary>
        public const char ArabicKeChar = (char)1603;

        /// <summary>
        /// ک فارسی
        /// </summary>
        public const char PersianKeChar = (char)1705;

        /// <summary>
        /// برای یک دست سازی ی و ک در برنامه بکار می‌رود
        /// </summary>
        /// <param name="data">رشته ورودی</param>
        /// <returns>رشته اصلاح شده</returns>
        public static string ApplyCorrectYeKe(this string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return string.Empty;
            return data.Replace(ArabicYeChar, PersianYeChar).Replace(ArabicKeChar, PersianKeChar).Trim();
        }
    }
}
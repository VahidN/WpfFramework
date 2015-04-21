using System;

namespace MyWpfFramework.Common.Config
{
    /// <summary>
    /// خواندن تنظیمات از فایل کانفیگ
    /// </summary>
    public interface IConfigSetGet
    {
        /// <summary>
        /// read settings from app.config file
        /// </summary>
        /// <param name="key">کلید</param>
        /// <returns>مقدار کلید</returns>
        string GetConfigData(string key);

        /// <summary>
        /// ذخیره سازی تنظیمات در فایل کانفیگ برنامه
        /// </summary>
        /// <param name="key">کلید</param>
        /// <param name="data">مقدار</param>
        void SetConfigData(string key, string data);
    }
}
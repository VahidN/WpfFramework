using System;
using System.Configuration;
using MyWpfFramework.Common.MVVM;

namespace MyWpfFramework.Common.Config
{
    /// <summary>
    /// خواندن تنظیمات از فایل کانفیگ
    /// </summary>
    public class ConfigSetGet : IConfigSetGet
    {
        /// <summary>
        /// read settings from app.config file
        /// </summary>
        /// <param name="key">کلید</param>
        /// <returns>مقدار کلید</returns>
        public string GetConfigData(string key)
        {
            //don't load on design time
            if (Designer.IsInDesignModeStatic)
                return "0";

            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var appSettings = configuration.AppSettings;
            string res = appSettings.Settings[key].Value;
            if (res == null) throw new Exception("Undefined: " + key);
            return res;
        }

        /// <summary>
        /// ذخیره سازی تنظیمات در فایل کانفیگ برنامه
        /// </summary>
        /// <param name="key">کلید</param>
        /// <param name="data">مقدار</param>
        public void SetConfigData(string key, string data)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = data;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
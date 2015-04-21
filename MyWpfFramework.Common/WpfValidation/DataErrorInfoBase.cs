using System;
using System.ComponentModel;
using System.Linq;

namespace MyWpfFramework.Common.WpfValidation
{
    /// <summary>
    /// کلاس پایه‌ایی برای یکپارچه سازی اعتبارسنجی مدل‌های ایی اف با دبلیو پی اف
    /// </summary>
    public abstract class DataErrorInfoBase : IDataErrorInfo
    {
        /// <summary>
        /// کلیه خطاهای حاصل از اعتبارسنجی شیء جاری را بر می‌گرداند
        /// </summary>
        public string Error
        {
            get
            {
                var errors = ValidationHelper.GetErrors(this);
                return string.Join(Environment.NewLine, errors.Select(x => x.ErrorMessage));
            }
        }

        /// <summary>
        /// خطاهای حاصل از اعتبارسنجی خاصیتی مشخص را بر می‌گرداند
        /// </summary>
        /// <param name="columnName">نام خاصیت</param>
        /// <returns>خطاهای احتمالی</returns>
        public string this[string columnName]
        {
            get
            {
                var errors = ValidationHelper.ValidateProperty(this, columnName);
                return string.Join(Environment.NewLine, errors.Select(x => x.ErrorMessage));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWpfFramework.Common.WpfValidation
{
    /// <summary>
    /// جهت اعتبار سنجی بر مبنای ویژگی‌های یکپارچه با ایی اف طراحی شده است
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// جهت اعتبار سنجی بر مبنای ویژگی‌های یکپارچه با ایی اف طراحی شده است        
        /// این متد کلیه خطاهای مرتبط با یک شیء را بر می‌گرداند
        /// </summary>
        /// <param name="instance">وهله‌ایی از شیءایی که باید اعتبارسنجی شود</param>
        /// <returns>لیستی از خطاهای احتمالی</returns>
        public static IList<ValidationResult> GetErrors(object instance)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), results, true);
            return results;
        }

        /// <summary>
        /// جهت اعتبار سنجی بر مبنای ویژگی‌های یکپارچه با ایی اف طراحی شده است        
        /// </summary>
        /// <param name="value">وهله‌ایی از شیءایی که باید اعتبارسنجی شود</param>
        /// <param name="propertyName">نام خاصیت مد نظر جهت اعتبار سنجی</param>
        /// <returns>لیستی از خطاهای احتمالی</returns>
        public static IList<ValidationResult> ValidateProperty(object value, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Invalid property name", propertyName);

            var propertyValue = value.GetType().GetProperty(propertyName).GetValue(value, null);
            var results = new List<ValidationResult>();
            var context = new ValidationContext(value, null, null) { MemberName = propertyName };
            Validator.TryValidateProperty(propertyValue, context, results);
            return results;
        }
    }
}
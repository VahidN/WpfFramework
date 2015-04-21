using System;

namespace MyWpfFramework.Common.Security
{
    /// <summary>
    /// وضعیت اعتبار سنجی صفحه را مشخص می‌کند
    /// </summary>
    public enum AuthorizationType
    {
        /// <summary>
        /// همه می‌توانند بدون اعتبار سنجی، دسترسی به این صفحات داشته باشند
        /// </summary>
        AllowAnonymous,

        /// <summary>
        /// کاربران وارد شده به سیستم بدون محدودیت به این صفحات دسترسی خواهند داشت
        /// </summary>
        FreeForAuthenticatedUsers,

        /// <summary>
        /// بر اساس نام نقش‌هایی که مشخص می‌شوند تصمیم گیری خواهد شد
        /// </summary>
        ApplyRequiredRoles
    }

    /// <summary>
    /// ویژگی طراحی شده جهت مشخص سازی وضعیت دسترسی به یک صفحه خاص
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PageAuthorizationAttribute : Attribute
    {
        private readonly AuthorizationType _authorizationType;
        private readonly string _requiredRoles;

        /// <summary>
        /// وضعیت دسترسی به صفحه را بر می‌گرداند
        /// </summary>
        public AuthorizationType AuthorizationType
        {
            get { return _authorizationType; }
        }

        /// <summary>
        /// لیستی از نقش‌های احتمالی مورد نیاز جهت دسترسی به صفحه را بر می‌گرداند
        /// </summary>
        public string[] RequiredRoles
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_requiredRoles))
                    return null;
                return _requiredRoles.Split(',');
            }
        }

        /// <summary>
        /// ویژگی طراحی شده جهت مشخص سازی وضعیت دسترسی به یک صفحه خاص
        /// </summary>
        /// <param name="authorizationType">وضعیت دسترسی به صفحه</param>
        /// <param name="requiredRoles">نقش‌های مورد نیاز جدا شده با کاما از هم</param>
        public PageAuthorizationAttribute(AuthorizationType authorizationType, string requiredRoles = "")
        {
            _authorizationType = authorizationType;
            _requiredRoles = requiredRoles;
        }
    }
}
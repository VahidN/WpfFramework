using System.Collections.ObjectModel;
using MyWpfFramework.Common.Security;
using MyWpfFramework.DomainClasses;

namespace MyWpfFramework.ServiceLayer.Contracts
{
    /// <summary>
    /// اطلاعات سراسری برنامه در مورد کاربر جاری را فراهم می‌کند
    /// </summary>
    public interface IAppContextService
    {
        /// <summary>
        /// جهت بررسی اطلاعات لاگین کاربر استفاده خواهد شد
        /// </summary>
        /// <param name="userName">نام کاربری وارد شده</param>
        /// <param name="password">کلمه عبور وارد شده</param>
        /// <returns>آیا عملیات موفقیت آمیز بوده یا خیر</returns>
        bool LoginCurrentUser(string userName, string password);

        /// <summary>
        /// اطلاعات لاگین کاربر را تخریب می‌کند و سبب خروج او از سیستم خواهد شد
        /// </summary>
        void LogoutCurrentUser();

        /// <summary>
        /// آیا کاربر جاری اعتبار سنجی شده است؟
        /// </summary>
        bool IsCurrentUserAuthenticated { get; }

        /// <summary>
        /// اطلاعات کاربر جاری را باز می‌گرداند
        /// </summary>
        User CurrentUser { get; }

        /// <summary>
        /// آیا کاربر جاری وارد شده به سیستم دسترسی مدیریتی دارد؟
        /// </summary>
        bool IsCurrentUserAdmin { get; }

        /// <summary>
        /// کلیه نقش‌های کاربر وارد شده به سیستم را بر می‌گرداند
        /// </summary>
        string[] CurrentUserRoles { get; }

        /// <summary>
        /// کلیه نقش‌های تعریف شده در سیستم را باز می‌گرداند
        /// </summary>
        string[] AllValidSystemRoles { get; }

        /// <summary>
        /// آیا کاربر جاری نقش‌های مشخص شده را دارا است؟
        /// </summary>
        /// <param name="requiredRoles">لیست یک سری نقش برای بررسی</param>
        /// <returns>آیا کاربر جاری نقش‌های مشخص شده را دارا است؟</returns>
        bool IsCurrentUserInRoles(string[] requiredRoles);

        /// <summary>
        /// آیا کاربر جاری می‌تواند بر اساس نقش‌هایی که دارد صفحه درخواستی را مشاهده کند؟
        /// </summary>
        /// <param name="attributeInstance">وهله‌ای از ویژگی مشخص کننده سطوح دسترسی مورد نیاز یک صفحه</param>
        /// <returns>آیا کاربر جاری نقش‌های مشخص شده را دارا است؟</returns>
        bool CanCurrentUserNavigateTo(PageAuthorizationAttribute attributeInstance);

        /// <summary>
        /// پس از به روز رسانی لیست کاربران شاید نیاز به به روز رسانی وضعیت کاربر جاری باشد
        /// </summary>
        /// <param name="usersList">لیست کاربران</param>
        void UpdateCurrentUser(ObservableCollection<User> usersList);
    }
}
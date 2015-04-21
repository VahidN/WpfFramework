using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MyWpfFramework.Common.Security;
using MyWpfFramework.DomainClasses;
using MyWpfFramework.ServiceLayer.Contracts;

namespace MyWpfFramework.ServiceLayer
{
    /// <summary>
    /// اطلاعات سراسری برنامه در مورد کاربر جاری را فراهم می‌کند
    /// It will be a `Singleton` instance, because it's a single instance/user application.
    /// </summary>
    public class AppContextService : IAppContextService
    {
        readonly IUsersService _usersService;
        /// <summary>
        /// سازنده کلاس
        /// </summary>
        /// <param name="usersService">سرویس اطلاعات کاربران</param>
        public AppContextService(IUsersService usersService)
        {
            _usersService = usersService;
        }

        /// <summary>
        /// آیا کاربر جاری وارد شده به سیستم دسترسی مدیریتی دارد؟
        /// </summary>
        public bool IsCurrentUserAdmin
        {
            get
            {
                return CurrentUser != null && CurrentUser.Role.IsAdmin;
            }
        }

        /// <summary>
        /// جهت بررسی اطلاعات لاگین کاربر استفاده خواهد شد
        /// </summary>
        /// <param name="userName">نام کاربری وارد شده</param>
        /// <param name="password">کلمه عبور وارد شده</param>
        /// <returns>آیا عملیات موفقیت آمیز بوده یا خیر</returns>
        public bool LoginCurrentUser(string userName, string password)
        {
            CurrentUser = _usersService.FindUser(userName, password);
            if (CurrentUser == null || !CurrentUser.IsActive)
            {
                CurrentUser = null;
                return false;
            }
            getRoles();
            return true;
        }

        /// <summary>
        /// اطلاعات لاگین کاربر را تخریب می‌کند و سبب خروج او از سیستم خواهد شد
        /// </summary>
        public void LogoutCurrentUser()
        {
            CurrentUser = null;
            CurrentUserRoles = null;
        }

        /// <summary>
        /// آیا کاربر جاری اعتبار سنجی شده است؟
        /// </summary>
        public bool IsCurrentUserAuthenticated
        {
            get { return CurrentUser != null; }
        }

        /// <summary>
        /// اطلاعات کاربر جاری را باز می‌گرداند
        /// </summary>
        public User CurrentUser { get; private set; }

        /// <summary>
        /// کلیه نقش‌های کاربر وارد شده به سیستم را بر می‌گرداند
        /// </summary>
        public string[] CurrentUserRoles { get; private set; }

        /// <summary>
        /// کلیه نقش‌های تعریف شده در سیستم را باز می‌گرداند
        /// </summary>
        public string[] AllValidSystemRoles { get; private set; }

        private void getRoles()
        {
            var userRoles = new List<string>();
            var allRoles = new List<string>();
            foreach (var property in CurrentUser.Role.GetType().GetProperties())
            {
                var propertyName = property.Name;
                var value = property.GetValue(CurrentUser.Role, null);
                if (!(value is bool)) // کلیه خواص کلاس نقش‌ها در اینجا باید از نوع بولی باشند
                {
                    throw new InvalidOperationException("خاصیت " + propertyName + " از نوع بولی نیست");
                }

                allRoles.Add(propertyName);
                if (!(bool)value)
                    continue;

                userRoles.Add(propertyName);
            }
            CurrentUserRoles = userRoles.ToArray();
            AllValidSystemRoles = allRoles.ToArray();
        }

        /// <summary>
        /// آیا کاربر جاری نقش‌های مشخص شده را دارا است؟
        /// </summary>
        /// <param name="requiredRoles">لیست یک سری نقش برای بررسی</param>
        /// <returns>آیا کاربر جاری نقش‌های مشخص شده را دارا است؟</returns>
        public bool IsCurrentUserInRoles(string[] requiredRoles)
        {
            if (!IsCurrentUserAuthenticated)
                return false;

            if (IsCurrentUserAdmin)
                return true;

            if (CurrentUserRoles == null || !CurrentUserRoles.Any())
            {
                return false;
            }

            foreach (var requiredRole in requiredRoles)
            {
                if (!AllValidSystemRoles.Contains(requiredRole.Trim()))
                {
                    throw new InvalidOperationException(string.Format("نقش {0} در مجموعه نقش‌های تعریف شده سیستم قرار ندارد.", requiredRole.Trim()));
                }
            }

            if (requiredRoles.Any(requiredRole => CurrentUserRoles.Contains(requiredRole.Trim())))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// آیا کاربر جاری می‌تواند بر اساس نقش‌هایی که دارد صفحه درخواستی را مشاهده کند؟
        /// </summary>
        /// <param name="attributeInstance">وهله‌ای از ویژگی مشخص کننده سطوح دسترسی مورد نیاز یک صفحه</param>
        /// <returns>آیا کاربر جاری نقش‌های مشخص شده را دارا است؟</returns>
        public bool CanCurrentUserNavigateTo(PageAuthorizationAttribute attributeInstance)
        {
            switch (attributeInstance.AuthorizationType)
            {
                case AuthorizationType.AllowAnonymous:
                    return true;
                case AuthorizationType.FreeForAuthenticatedUsers:
                    return IsCurrentUserAuthenticated;
                case AuthorizationType.ApplyRequiredRoles:
                    return IsCurrentUserInRoles(attributeInstance.RequiredRoles);
            }

            return false;
        }

        /// <summary>
        /// پس از به روز رسانی لیست کاربران شاید نیاز به به روز رسانی وضعیت کاربر جاری باشد
        /// </summary>
        /// <param name="usersList">لیست کاربران</param>
        public void UpdateCurrentUser(ObservableCollection<User> usersList)
        {
            if (CurrentUser == null)
                return;

            var newUserInfo = usersList.FirstOrDefault(x => x.Id == CurrentUser.Id);
            if (newUserInfo == null)
                return;

            CurrentUser = newUserInfo;
            getRoles();
        }
    }
}
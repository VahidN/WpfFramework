using System.Collections.Generic;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using MyWpfFramework.Common.Config;
using MyWpfFramework.Common.MVVM;
using MyWpfFramework.Common.UI;
using MyWpfFramework.Infrastructure.Core;
using MyWpfFramework.Models;
using MyWpfFramework.ServiceLayer.Contracts;

namespace MyWpfFramework.Infrastructure.ViewModels.Login
{
    /// <summary>
    /// ویوو مدل صفحه لاگین برنامه
    /// </summary>
    public class LoginPageViewModel : BaseViewModel
    {
        /// <summary>
        /// اطلاعات صفحه لاگین جهت نمایش و دریافت
        /// </summary>
        public LoginPageModel LoginPageData { set; get; }

        /// <summary>
        /// رخداد کلیک بر روی دکمه ورود به سیستم را دریافت می‌کند
        /// </summary>
        public RelayCommand DoLogin { set; get; }

        readonly IAppContextService _appContextService;
        readonly IConfigSetGet _configSetGet;

        /// <summary>
        /// ویوو مدل صفحه لاگین برنامه
        /// </summary>
        /// <param name="appContextService">اطلاعات سراسری برنامه در مورد کاربر جاری را فراهم می‌کند</param>
        /// <param name="configSetGet">دسترسی به اطلاعات فایل کانفیگ برنامه</param>
        public LoginPageViewModel(IAppContextService appContextService, IConfigSetGet configSetGet)
        {
            _appContextService = appContextService;
            _configSetGet = configSetGet;

            LoginPageData = new LoginPageModel();
            DoLogin = new RelayCommand(doLogin, canDoLogin);

            initUserFromConfig();
        }

        /// <summary>
        /// نام کاربر وارد شده به سیستم در فایل کانفیگ درج خواهد شد
        /// </summary>
        private void initUserFromConfig()
        {
            var username = _configSetGet.GetConfigData("LastLoginName");
            if (!string.IsNullOrWhiteSpace(username))
            {
                LoginPageData.UserName = username; //update view
            }
        }

        /// <summary>
        /// فعال و غیرفعال سازی دکمه لاگین
        /// </summary>
        /// <returns></returns>
        bool canDoLogin()
        {
            return !string.IsNullOrWhiteSpace(LoginPageData.UserName) &&
                   !string.IsNullOrWhiteSpace(LoginPageData.Password);
        }

        /// <summary>
        /// سعی در ورود به سیستم
        /// </summary>
        void doLogin()
        {
            var result = _appContextService.LoginCurrentUser(LoginPageData.UserName, LoginPageData.Password);
            // آیا کاربر اعتبارسنجی شده است؟
            if (result)
            {
                // ثبت نام کاربری او در فایل کانفیگ برنامه
                _configSetGet.SetConfigData("LastLoginName", LoginPageData.UserName);

                // هدایت به صفحه خوش آمد گویی
                Redirect.ToWelcomePage();
            }
            else
            {
                // نمایش خطایی به کاربر در صورت عدم ورود اطلاعات صحیح یا معتبر
                new SendMsg().ShowMsg(new AlertConfirmBoxModel
                {
                    ErrorTitle = "خطا",
                    Errors = new List<string> { "لطفا مجددا سعی نمائید." },
                    ShowCancel = Visibility.Collapsed,
                    ShowConfirm = Visibility.Visible
                });
            }
        }

        /// <summary>
        /// آیا در حین نمایش صفحه‌ای دیگر باید به کاربر پیغام داد که اطلاعات ذخیره نشده‌ای وجود دارد؟
        /// </summary>
        public override bool ViewModelContextHasChanges
        {
            get { return false; }
        }
    }
}
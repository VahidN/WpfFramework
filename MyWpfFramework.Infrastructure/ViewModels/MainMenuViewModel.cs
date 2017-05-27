using GalaSoft.MvvmLight.CommandWpf;
using MyWpfFramework.Common.MVVM;
using MyWpfFramework.Infrastructure.Core;
using MyWpfFramework.ServiceLayer.Contracts;

namespace MyWpfFramework.Infrastructure.ViewModels
{
    /// <summary>
    /// سیستم راهبری برنامه در اینجا مدیریت می‌شود
    /// </summary>
    public class MainMenuViewModel : BaseViewModel
    {
        readonly IAppContextService _appContextService;

        /// <summary>
        /// سیستم راهبری برنامه در اینجا مدیریت می‌شود
        /// </summary>
        /// <param name="appContextService">اطلاعات سراسری برنامه در مورد کاربر جاری را فراهم می‌کند</param>
        public MainMenuViewModel(IAppContextService appContextService)
        {
            _appContextService = appContextService;
            DoNavigate = new RelayCommand<string>(doNavigate);
        }

        /// <summary>
        /// رخدادهای کلیک بر روی دکمه‌های منوی اصلی برنامه را دریافت می‌کند
        /// </summary>
        public RelayCommand<string> DoNavigate { set; get; }


        /// <summary>
        /// هدایت کاربر به آدرسی خاص
        /// </summary>
        /// <param name="url">آدرس صفحه</param>
        private void doNavigate(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            if (url == "DoLogout") // خروجی از سیستم درخواست شده است
            {
                _appContextService.LogoutCurrentUser();
                Redirect.ToLoginPage();
            }
            else
            {
                Redirect.To(url);
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
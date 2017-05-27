using GalaSoft.MvvmLight.CommandWpf;
using MyWpfFramework.Common.Crypto;
using MyWpfFramework.Common.MVVM;
using MyWpfFramework.DataLayer.Context;
using MyWpfFramework.DomainClasses;
using MyWpfFramework.Models;
using MyWpfFramework.ServiceLayer.Contracts;

namespace MyWpfFramework.Infrastructure.ViewModels.UserInfo
{
    /// <summary>
    /// ویوو مدل تغییر اطلاعات کاربر جاری وارد شده به سیستم
    /// </summary>
    public class ChangeProfileViewModel : BaseViewModel
    {
        readonly IAppContextService _appContextService;
        readonly IUnitOfWork _uow;
        readonly IUsersService _usersService;

        /// <summary>
        /// ویوو مدل تغییر اطلاعات کاربر جاری وارد شده به سیستم
        /// </summary>
        /// <param name="uow">وهله‌ای از زمینه و واحد کاری ایی اف</param>
        /// <param name="appContextService">اطلاعات سراسری برنامه در مورد کاربر جاری را فراهم می‌کند</param>
        /// <param name="usersService">سرویس اطلاعات کاربران</param>
        public ChangeProfileViewModel(IUnitOfWork uow, IAppContextService appContextService, IUsersService usersService)
        {
            _uow = uow;
            _usersService = usersService;
            _appContextService = appContextService;

            ChangeProfileData = new LoginPageModel();
            DoSave = new RelayCommand(doSave, canDoSave);

            ChangeProfileData.UserName = _appContextService.CurrentUser.UserName;
            ChangeProfileData.Password = _appContextService.CurrentUser.Password;
        }

        /// <summary>
        /// رخداد ذخیره سازی اطلاعات را دریافت می‌کند
        /// </summary>
        public RelayCommand DoSave { set; get; }

        /// <summary>
        /// اطلاعات کاربر جاری جهت نمایش در رابط کاربری
        /// </summary>
        public LoginPageModel ChangeProfileData { set; get; }

        /// <summary>
        /// فعال و غیرفعال کردن دکمه ذخیره سازی
        /// </summary>
        private bool canDoSave()
        {
            return ViewModelContextHasChanges;
        }

        /// <summary>
        /// ذخیره سازی اطلاعات تغییر کرده
        /// </summary>
        private void doSave()
        {
            // نکته: از سرویس زمینه برنامه نمی‌توان برای به روز رسانی اطلاعات کاربر استفاده کرد
            // چون پس از لاگین، از کانتکست ایی اف جدا می‌شود

            var user = _usersService.FindUser(_appContextService.CurrentUser.Id);
            user.UserName = ChangeProfileData.UserName;
            if (user.Password != ChangeProfileData.Password)
            {
                user.Password = ChangeProfileData.Password.SHA1Hash();
            }
            _uow.ApplyAllChanges(user.UserName);

            updateAppContext(user);
        }

        /// <summary>
        /// به روز رسانی اطلاعات سراسری برنامه
        /// </summary>
        private void updateAppContext(User user)
        {
            _appContextService.CurrentUser.UserName = user.UserName;
            _appContextService.CurrentUser.Password = user.Password;
        }

        /// <summary>
        /// آیا در حین نمایش صفحه‌ای دیگر باید به کاربر پیغام داد که اطلاعات ذخیره نشده‌ای وجود دارد؟
        /// </summary>
        public override bool ViewModelContextHasChanges
        {
            get
            {
                var user = _appContextService.CurrentUser;
                if (user == null)
                    return false;

                return (user.Password != ChangeProfileData.Password) ||
                       (user.UserName != ChangeProfileData.UserName) ||
                       _uow.ContextHasChanges;
            }
        }
    }
}
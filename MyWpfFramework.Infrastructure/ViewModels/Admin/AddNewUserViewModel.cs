using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using MyWpfFramework.Common.Crypto;
using MyWpfFramework.Common.MVVM;
using MyWpfFramework.DataLayer.Context;
using MyWpfFramework.DomainClasses;
using MyWpfFramework.ServiceLayer.Contracts;

namespace MyWpfFramework.Infrastructure.ViewModels.Admin
{
    /// <summary>
    /// ویوو مدل افزودن و مدیریت کاربران
    /// </summary>
    public class AddNewUserViewModel : BaseViewModel
    {
        readonly IAppContextService _appContextService;
        readonly IUnitOfWork _uow;
        readonly IUsersService _usersService;

        /// <summary>
        /// ویوو مدل افزودن و مدیریت کاربران
        /// </summary>
        /// <param name="uow">وهله‌ای از زمینه و واحد کاری ایی اف</param>
        /// <param name="appContextService">اطلاعات سراسری برنامه در مورد کاربر جاری را فراهم می‌کند</param>
        /// <param name="usersService">سرویس اطلاعات کاربران</param>
        public AddNewUserViewModel(IUnitOfWork uow,
                                   IAppContextService appContextService,
                                   IUsersService usersService)
        {
            _uow = uow;
            _appContextService = appContextService;
            _usersService = usersService;

            UsersList = _usersService.GetSyncedUsersList();

            DoSave = new RelayCommand(doSave, canDoSave);
            DoAddNew = new RelayCommand(doAddNew);
        }

        /// <summary>
        /// رخداد ذخیره سازی اطلاعات را دریافت می‌کند
        /// </summary>
        public RelayCommand DoSave { set; get; }

        /// <summary>
        /// رخداد افزودن یک سطر جدید برای تعریف کاربری جدید را دریافت می‌کند
        /// </summary>
        public RelayCommand DoAddNew { set; get; }

        /// <summary>
        /// لیستی از کاربران برای نمایش در برنامه
        /// </summary>
        public ObservableCollection<User> UsersList { set; get; }

        /// <summary>
        /// ذخیره سازی تغییرات در کلیه ردیف‌ها با هم
        /// </summary>
        private void doSave()
        {
            fixChangedPasswords();
            _uow.ApplyAllChanges(_appContextService.CurrentUser.UserName);

            _appContextService.UpdateCurrentUser(UsersList);
        }

        /// <summary>
        /// پسوردهای جدید باید هش شوند
        /// </summary>
        private void fixChangedPasswords()
        {
            const int sha1Len = 40;
            foreach (var user in UsersList.Where(x => x.Password.Length != sha1Len))
            {
                //پسورد جدید است، بنابراین باید پیش از ذخیره سازی هش شود
                user.Password = user.Password.SHA1Hash();
            }
        }

        /// <summary>
        /// فعال و غیرفعال سازی خودکار دکمه ثبت
        /// این متد به صورت خودکار توسط RelayCommand کنترل می‌شود
        /// </summary>        
        private bool canDoSave()
        {
            // آیا در حین نمایش صفحه‌ای دیگر باید به کاربر پیغام داد که اطلاعات ذخیره نشده‌ای وجود دارد؟
            return ViewModelContextHasChanges;
        }

        /// <summary>
        /// افزودن یک سطر جدید به سیستم ردیابی
        /// </summary>
        private void doAddNew()
        {
            _usersService.AddUser(new User());
        }

        /// <summary>
        /// آیا در حین نمایش صفحه‌ای دیگر باید به کاربر پیغام داد که اطلاعات ذخیره نشده‌ای وجود دارد؟
        /// </summary>
        public override bool ViewModelContextHasChanges
        {
            get { return _uow.ContextHasChanges; }
        }
    }
}
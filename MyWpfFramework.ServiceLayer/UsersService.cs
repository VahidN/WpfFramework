using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using MyWpfFramework.Common.Crypto;
using MyWpfFramework.Common.PersianToolkit;
using MyWpfFramework.DataLayer.Context;
using MyWpfFramework.DomainClasses;
using MyWpfFramework.ServiceLayer.Contracts;

namespace MyWpfFramework.ServiceLayer
{
    /// <summary>
    /// سرویس اطلاعات کاربران
    /// </summary>
    public class UsersService : IUsersService
    {
        readonly IDbSet<User> _users;
        readonly IUnitOfWork _uow;
        /// <summary>
        /// سازنده کلاس
        /// </summary>
        /// <param name="uow">وهله‌ای از الگوی واحد کار یا زمینه ایی اف</param>
        public UsersService(IUnitOfWork uow)
        {
            _uow = uow;
            _users = _uow.Set<User>();
        }

        /// <summary>
        /// یک کاربر را بر اساس اطلاعات لاگین او پیدا می‌کند
        /// </summary>
        /// <param name="username">نام کاربری</param>
        /// <param name="password">کلمه عبور</param>
        /// <returns>کاربر احتمالی یافت شده</returns>
        public User FindUser(string username, string password)
        {
            var hashedPassword = password.SHA1Hash();
            username = username.ApplyCorrectYeKe();
            return _users.FirstOrDefault(x => x.UserName == username && x.Password == hashedPassword);
        }

        /// <summary>
        /// یافتن یک کاربر بر اساس کلید اصلی او
        /// </summary>
        /// <param name="userId">شماره کاربر</param>
        /// <returns>کاربر احتمالی یافت شده</returns>
        public User FindUser(int userId)
        {
            return _users.Find(userId);
        }

        /// <summary>
        /// یک شیء کاربر را به زمینه ایی اف اضافه می‌کند
        /// </summary>
        /// <param name="data">وهله‌ای شیء کاربر</param>
        /// <returns>کاربر اضافه شده به زمینه</returns>
        public User AddUser(User data)
        {
            return _users.Add(data);
        }

        /// <summary>
        /// جهت مقاصد انقیاد داده‌ها در دبلیو پی اف طراحی شده است
        /// لیستی از کاربران سیستم را باز می‌گرداند
        /// </summary>
        /// <param name="count">تعداد کاربر مد نظر</param>
        /// <returns>لیستی از کاربران</returns>
        public ObservableCollection<User> GetSyncedUsersList(int count = 1000)
        {
            _users.OrderBy(x => x.FriendlyName).Take(count)
                  .Load();

            // For Databinding with WPF.
            // Before calling this method you need to fill the context by using `Load()` method.
            return _users.Local;
        }
    }
}
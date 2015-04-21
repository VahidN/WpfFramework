using System.Collections.ObjectModel;
using MyWpfFramework.DomainClasses;

namespace MyWpfFramework.ServiceLayer.Contracts
{
    /// <summary>
    /// سرویس اطلاعات کاربران
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// یک کاربر را بر اساس اطلاعات لاگین او پیدا می‌کند
        /// </summary>
        /// <param name="username">نام کاربری</param>
        /// <param name="password">کلمه عبور</param>
        /// <returns>کاربر احتمالی یافت شده</returns>
        User FindUser(string username, string password);

        /// <summary>
        /// یافتن یک کاربر بر اساس کلید اصلی او
        /// </summary>
        /// <param name="userId">شماره کاربر</param>
        /// <returns>کاربر احتمالی یافت شده</returns>
        User FindUser(int userId);

        /// <summary>
        /// یک شیء کاربر را به زمینه ایی اف اضافه می‌کند
        /// </summary>
        /// <param name="data">وهله‌ای شیء کاربر</param>
        /// <returns>کاربر اضافه شده به زمینه</returns>
        User AddUser(User data);

        /// <summary>
        /// جهت مقاصد انقیاد داده‌ها در دبلیو پی اف طراحی شده است
        /// لیستی از کاربران سیستم را باز می‌گرداند
        /// </summary>
        /// <param name="count">تعداد کاربر مد نظر</param>
        /// <returns>لیستی از کاربران</returns>
        ObservableCollection<User> GetSyncedUsersList(int count = 1000);        
    }
}
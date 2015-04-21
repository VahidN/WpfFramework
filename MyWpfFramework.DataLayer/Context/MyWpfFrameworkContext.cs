using System.Data.Entity;
using MyWpfFramework.DomainClasses;

namespace MyWpfFramework.DataLayer.Context
{
    /// <summary>
    /// زمینه کاری اصلی برنامه
    /// </summary>
    public class MyWpfFrameworkContext : MyDbContextBase
    {
        /// <summary>
        /// ctor.
        /// </summary>
        public MyWpfFrameworkContext()
        {
            // این سازنده خالی اینجا اضافه شده تا بشود روی آن یک برک پوینت قرار داد
            // جهت بررسی تعداد بار وهله سازی کانتکست برنامه در حین تزریق وابستگی‌ها
        }

        /// <summary>
        /// تعریف موجودیت کاربران و در معرض دید قرار دادن آن
        /// </summary>
        public DbSet<User> Users { set; get; }

        //todo: سایر موجودیت‌ها در اینجا اضافه خواهند شد

        /// <summary>
        /// در اینجا سفارشی سازی‌های مورد نیاز تعاریف نگاشت‌ها انجام خواهد شد
        /// </summary>        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<BaseEntity>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
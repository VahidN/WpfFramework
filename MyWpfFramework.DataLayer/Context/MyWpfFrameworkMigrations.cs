using System.Linq;
using System.Data.Entity.Migrations;
using MyWpfFramework.DomainClasses;
using MyWpfFramework.Common.Crypto;
using MyWpfFramework.Common.EntityFrameworkToolkit;

namespace MyWpfFramework.DataLayer.Context
{
    /// <summary>
    /// در صورت تغییر مدل‌های برنامه به صورت خودکار ساختار بانک اطلاعاتی را به روز می‌کند
    /// </summary>
    public class MyWpfFrameworkMigrations : DbMigrationsConfiguration<MyWpfFrameworkContext>
    {
        /// <summary>
        /// سازنده کلاس به روز رسانی ساختار بانک اطلاعاتی
        /// </summary>
        public MyWpfFrameworkMigrations()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        /// <summary>
        /// در اینجا کار تعریف ایندکس‌های منحصربفرد و ذخیره سازی اطلاعات اولیه برنامه انجام می‌شوند
        /// </summary>        
        protected override void Seed(MyWpfFrameworkContext context)
        {
            addRolesAndAdmin(context);
            addUniqueIndex(context);
            base.Seed(context);
        }

        private static void addUniqueIndex(MyWpfFrameworkContext context)
        {
            context.CreateUniqueIndex<User>(x => x.UserName);
            context.CreateUniqueIndex<User>(x => x.FriendlyName);
        }

        /// <summary>
        /// در اولین باری که دیتابیس ایجاد می‌شود، یک کاربر ادمین جهت ورود به سیستم نیز افزوده خواهد شد
        /// </summary>        
        private static void addRolesAndAdmin(MyWpfFrameworkContext context)
        {
            var user1 = new User
            {
                FriendlyName = "Admin",
                Password = "123456".SHA1Hash(),                
                UserName = "Admin",
                IsActive = true,
                Role = new SystemRole
                {
                    IsAdmin = true,
                    CanAddNewUser = true
                }
            };
            if (!context.Users.Any())
            {
                context.Users.Add(user1);
                context.ApplyAllChanges(string.Empty); //forces using instantiated base auditFields
            }
        }
    }
}
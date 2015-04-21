using System.Data.Entity;

namespace MyWpfFramework.DataLayer.Context
{
    /// <summary>
    /// کار آغاز تنظیمات بانک اطلاعاتی در اینجا رخ خواهد داد
    /// </summary>
    public static class StartDb
    {
        /// <summary>
        /// کار به روز رسانی ساختار بانک اطلاعاتی در صورت نیاز و سپس آغاز نگاشت‌ها
        /// </summary>
        public static void InitDb()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyWpfFrameworkContext, MyWpfFrameworkMigrations>());
            // Forces initialization of database on model changes.
            using (var context = new MyWpfFrameworkContext())
            {
                context.Database.Initialize(force: true);
            }
        }
    }
}
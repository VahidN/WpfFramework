using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using MyWpfFramework.Common.PersianToolkit;
using MyWpfFramework.Common.ReflectionToolkit;

namespace MyWpfFramework.Common.EntityFrameworkToolkit
{
    /// <summary>
    /// جهت یکسان سازی ی و ک در برنامه استفاده می‌شود
    /// </summary>
    public static class ApplyYeKe
    {
        /// <summary>
        /// این متد موجودیت‌های تغییر کرده در ایی اف را یافته و سپس ی و ک آن‌ها را یک دست می‌کند
        /// </summary>
        /// <param name="dbContext">زمینه کاری ایی اف</param>
        public static void ApplyCorrectYeKe(this DbContext dbContext)
        {
            if (dbContext == null) return;

            //پیدا کردن موجودیت‌های تغییر کرده  
            var changedEntities = dbContext.ChangeTracker
                                      .Entries()
                                      .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var item in changedEntities)
            {
                if (item.Entity == null) continue;

                //یافتن خواص قابل تنظیم و رشته‌ای این موجودیت‌ها  
                var propertyInfos = item.Entity.GetType().GetProperties(
                    BindingFlags.Public | BindingFlags.Instance
                    ).Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                var pr = new PropertyReflector();

                //اعمال یکپارچگی نهایی  
                foreach (var propertyInfo in propertyInfos)
                {
                    var propName = propertyInfo.Name;
                    var val = pr.GetValue(item.Entity, propName);
                    if (val != null)
                    {
                        var newVal = val.ToString().ApplyCorrectYeKe();
                        if (newVal == val.ToString()) continue;
                        pr.SetValue(item.Entity, propName, newVal);
                    }
                }
            }
        }
    }
}
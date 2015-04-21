using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MyWpfFramework.Common.Security
{
    /// <summary>
    /// یافتن کلیه صفحاتی که دارای اعتبار سنجی هستند و سپس کش کردن نتیجه آن
    /// فرض بر این است که این صفحات در اسمبلی اصلی برنامه قرار دارند
    /// </summary>
    public static class PageAuthorizationScanner
    {
        /// <summary>
        /// لیست صفحات دارای اعتبارسنجی
        /// </summary>
        public static IList<Type> TypesWithPageAuthorizationAttributeCache { get; private set; }

        static PageAuthorizationScanner()
        {
            var asm = Assembly.GetEntryAssembly(); //اسمبلی است که ویووهای برنامه در آن قرار دارند
            TypesWithPageAuthorizationAttributeCache = asm.GetTypes()
                    .Where(x => x.GetCustomAttributes(typeof(PageAuthorizationAttribute), true).Any())
                    .ToList();
        }

        /// <summary>
        /// وضعیت اعتبار سنجی صفحه را دریافت می‌کند
        /// </summary>
        /// <param name="url">آدرس صفحه</param>
        /// <returns>وهله‌ای از ویژگی مرتبط با آن</returns>
        public static PageAuthorizationAttribute GetPageAuthorizationAttribute(Uri url)
        {
            var className = Path.GetFileNameWithoutExtension(url.ToString());
            var classType = TypesWithPageAuthorizationAttributeCache.FirstOrDefault(x => x.Name == className);
            if (classType == null)
            {
                throw new NotImplementedException("وضعیت دسترسی به صفحه " + url.ToString() + " نامشخص است.");
            }
            return classType.GetCustomAttributes(typeof(PageAuthorizationAttribute), true).First() as PageAuthorizationAttribute;
        }
    }
}
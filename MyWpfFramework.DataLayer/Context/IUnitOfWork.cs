using System.Data.Entity;
using System.Collections.Generic;

namespace MyWpfFramework.DataLayer.Context
{
    /// <summary>
    /// پیاده سازی الگوی واحد کار
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// دسترسی به اعمال قابل انجام با یک موجودیت را فراهم می‌کند
        /// </summary>
        /// <typeparam name="TEntity">نوع موجودیت</typeparam>        
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// ذخیره سازی کلیه تغییرات انجام شده در تمامی رکوردهای تحت نظر در حافظه
        /// </summary>
        /// <param name="userName">نام کاربر جاری</param>
        /// <param name="updateAuditFields">آیا فیلدهای ویرایش کننده اطلاعات نیز مقدار دهی شوند؟</param>
        /// <returns>تعداد رکوردهای تغییر کرده</returns>
        int ApplyAllChanges(string userName, bool updateAuditFields = true);

        /// <summary>
        /// بازگردانی تغییرات انجام شده در رکوردهای تحت نظر در حافظه به حالت اول
        /// </summary>
        void RejectChanges();

        /// <summary>
        /// برای ذخیره سازی تعداد زیادی رکورد با هم کاربرد دارد. در غیراینصورت از آن استفاده نکنید
        /// </summary>
        void DisableChangeTracking();

        /// <summary>
        /// آیا در رکوردهای تحت نظر در حافظه تغییری حاصل شده است؟
        /// </summary>
        bool ContextHasChanges { get; }
    }
}
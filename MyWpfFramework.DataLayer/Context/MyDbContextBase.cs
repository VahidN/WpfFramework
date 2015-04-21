using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using MyWpfFramework.Common.EntityFrameworkToolkit;
using MyWpfFramework.Common.PersianToolkit;
using MyWpfFramework.Common.UI;
using MyWpfFramework.DomainClasses;

namespace MyWpfFramework.DataLayer.Context
{
    /// <summary>
    /// پیاده سازی الگوی واحد کار
    /// </summary>
    public class MyDbContextBase : DbContext, IUnitOfWork
    {
        /// <summary>
        /// بازگردانی تغییرات انجام شده در رکوردهای تحت نظر در حافظه به حالت اول
        /// </summary>
        public void RejectChanges()
        {
            foreach (var entry in this.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;

                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        private void auditFields(string auditUser)
        {
            foreach (var entry in this.ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        setCreatedOn(auditUser, entry);
                        setModifiedOn(auditUser, entry);
                        break;

                    case EntityState.Modified:
                        setModifiedOn(auditUser, entry);
                        break;
                }
            }
        }

        private static void setModifiedOn(string auditUser, DbEntityEntry<BaseEntity> entry)
        {
            entry.Entity.ModifiedOn = DateTime.Now;
            entry.Entity.ModifiedBy = auditUser;
        }

        private static void setCreatedOn(string auditUser, DbEntityEntry<BaseEntity> entry)
        {
            entry.Entity.CreatedOn = DateTime.Now;
            entry.Entity.CreatedBy = auditUser;
            entry.Entity.CreatedOnPersian = PersianDate.CurrentSystemShamsiDate("/", true);
        }

        /// <summary>
        /// دسترسی به اعمال قابل انجام با یک موجودیت را فراهم می‌کند
        /// </summary>
        /// <typeparam name="TEntity">نوع موجودیت</typeparam>        
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// ذخیره سازی کلیه تغییرات انجام شده در تمامی رکوردهای تحت نظر در حافظه
        /// </summary>
        /// <param name="userName">نام کاربر جاری</param>
        /// <param name="updateAuditFields">آیا فیلدهای ویرایش کننده اطلاعات نیز مقدار دهی شوند؟</param>
        /// <returns>تعداد رکوردهای تغییر کرده</returns>
        public int ApplyAllChanges(string userName, bool updateAuditFields = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    userName = "Program";

                if (this.Configuration.AutoDetectChangesEnabled)
                    this.ApplyCorrectYeKe();

                if (updateAuditFields)
                {
                    auditFields(userName);
                }

                if (hasValidationErrors())
                    return 0;

                return base.SaveChanges();
            }
            catch (DbEntityValidationException validationException)
            {
                var errors = new List<string>();
                foreach (var error in validationException.EntityValidationErrors)
                {
                    var entry = error.Entry;
                    errors.Add(entry.Entity.GetType().Name + ": " + entry.State);
                    foreach (var err in error.ValidationErrors)
                    {
                        errors.Add(err.PropertyName + " " + err.ErrorMessage);
                    }
                }

                new SendMsg().ShowMsg(
                    new AlertConfirmBoxModel
                    {
                        ErrorTitle = "خطای اعتبار سنجی",
                        Errors = errors,
                    }, validationException);
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                var errors = "مقادیر در سمت بانک اطلاعاتی تغییر کرده‌اند. لطفا صفحه را ریفرش کنید" + ": "
                    + concurrencyException.Entries.First().Entity.GetType().Name;

                new SendMsg().ShowMsg(
                    new AlertConfirmBoxModel
                    {
                        ErrorTitle = "خطای همزمانی",
                        Errors = new List<string> { errors },
                    }, concurrencyException);
            }
            catch (DbUpdateException updateException)
            {
                var errors = new List<string> { updateException.Message };
                if (updateException.InnerException != null)
                    errors.Add(updateException.InnerException.Message);

                foreach (var entry in updateException.Entries)
                {
                    errors.Add("Related Entity: " + entry.Entity);
                }

                new SendMsg().ShowMsg(
                    new AlertConfirmBoxModel
                    {
                        ErrorTitle = "خطای به روز رسانی",
                        Errors = errors,
                    }, updateException);
            }

            return 0;
        }

        private bool hasValidationErrors()
        {
            var validationErrors = this.GetValidationErrors();
            if (validationErrors == null || !validationErrors.Any())
                return false;

            var results = new List<string>();
            foreach (var item in validationErrors)
            {
                results.Add(string.Join(Environment.NewLine, item.ValidationErrors.Select(x => x.ErrorMessage)));
            }

            //نمایش خطاهای اعتبار سنجی به کاربر
            new SendMsg().ShowMsg(
                new AlertConfirmBoxModel
                {
                    ErrorTitle = "لطفا خطاهای زیر را پیش از ثبت نهایی برطرف کنید:",
                    Errors = results
                });
            return true;
        }

        /// <summary>
        /// برای ذخیره سازی تعداد زیادی رکورد با هم کاربرد دارد. در غیراینصورت از آن استفاده نکنید
        /// </summary>
        public void DisableChangeTracking()
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
        }

        /// <summary>
        /// آیا در رکوردهای تحت نظر در حافظه تغییری حاصل شده است؟
        /// </summary>
        public bool ContextHasChanges
        {
            get
            {
                return this.ChangeTracker
                           .Entries()
                           .Any(x => x.State == EntityState.Added ||
                                     x.State == EntityState.Deleted ||
                                     x.State == EntityState.Modified);
            }
        }
    }
}
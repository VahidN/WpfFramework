using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using MyWpfFramework.Common.ReflectionToolkit;

namespace MyWpfFramework.Common.EntityFrameworkToolkit
{
    /// <summary>
    /// یک سری متد کمکی طراحی شده برای ایی اف
    /// </summary>
    public static class ContextExtensions
    {
        /// <summary>
        /// یافتن نام جدول واقعی مورد استفاده در زمینه جاری ایی اف کد فرست
        /// </summary>
        /// <typeparam name="T">نام کلاس</typeparam>
        /// <param name="context">زمینه</param>
        /// <returns>رشته حاوی نام جدول</returns>
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext.GetTableName<T>();
        }

        /// <summary>
        /// یافتن نام جدول واقعی مورد استفاده در زمینه جاری ایی اف دیتابیس فرست
        /// </summary>
        /// <typeparam name="T">نام کلاس</typeparam>
        /// <param name="context">زمینه</param>
        /// <returns>رشته حاوی نام جدول</returns>
        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            var sql = context.CreateObjectSet<T>().ToTraceString();
            var regex = new Regex("FROM (?<table>.*) AS");
            var match = regex.Match(sql);
            string table = match.Groups["table"].Value;
            return table                     
                     .Replace("`", string.Empty)
                     .Replace("[", string.Empty)
                     .Replace("]", string.Empty)
					 .Replace("dbo.", string.Empty)
                     .Trim();
        }

        private static bool hasUniqueIndex(this DbContext context, string tableName, string indexName)
        {
            var sql = "SELECT count(*) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS where table_name = '"
                      + tableName + "' and CONSTRAINT_NAME = '" + indexName + "'";
            var result = context.Database.SqlQuery<int>(sql).FirstOrDefault();
            return result > 0;
        }

        private static void createUniqueIndex(this DbContext context, string tableName, string fieldName)
        {
            var indexName = "IX_Unique_" + tableName + "_" + fieldName;
            if (hasUniqueIndex(context, tableName, indexName))
                return;

            try
            {
                var sql = "ALTER TABLE [" + tableName + "] ADD CONSTRAINT [" + indexName + "] UNIQUE ([" + fieldName + "])";
                context.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception ex)
            {
                if (!isDuplicateIndex(indexName, ex))
                    throw;
            }
        }

        private static bool isDuplicateIndex(string indexName, Exception ex)
        {
            return ex.Message.Contains("exists") && ex.Message.Contains(indexName);
        }

        /// <summary>
        /// ایجاد فیلد منحصربفرد در بانک اطلاعاتی
        /// </summary>
        /// <typeparam name="TEntity">نام کلاس موجودیت مد نظر</typeparam>
        /// <param name="context">زمینه ایی اف</param>
        /// <param name="fieldName">نام خاصیت مورد نظر</param>
        public static void CreateUniqueIndex<TEntity>(this DbContext context, Expression<Func<TEntity, object>> fieldName) where TEntity : class
        {
            createUniqueIndex(context, context.GetTableName<TEntity>(), PropertyHelper.Name(fieldName));
        }

        /// <summary>
        /// آیا خاصیت مورد نظر به صورت فیلد منحصربفرد علامتگذاری شده‌است؟
        /// </summary>
        /// <typeparam name="TEntity">نام کلاس موجودیت مد نظر</typeparam>
        /// <param name="context">زمینه ایی اف</param>
        /// <param name="fieldName">نام خاصیت مورد نظر</param>
        public static void HasUniqueIndex<TEntity>(this DbContext context, Expression<Func<TEntity, object>> fieldName) where TEntity : class
        {
            var tableName = context.GetTableName<TEntity>();
            var indexName = "IX_Unique_" + tableName + "_" + PropertyHelper.Name(fieldName);
            hasUniqueIndex(context, tableName, indexName);
        }
    }
}
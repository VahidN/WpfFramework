using System;
using System.ComponentModel;
using System.Linq.Expressions;
using MyWpfFramework.Common.WpfValidation;

namespace MyWpfFramework.Common.MVVM
{
    /// <summary>
    /// کلاس پایه ویوو مدل‌های برنامه که جهت علامتگذاری آن‌ها برای سیم کشی‌های تزریق وابستگی‌های برنامه نیز استفاده می‌شود
    /// </summary>
    public abstract class BaseViewModel : DataErrorInfoBase, INotifyPropertyChanged, IViewModel
    {
        /// <summary>
        /// جهت اطلاع رسانی در مورد تغییر مقدار یک خاصیت بکار می‌رود
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// تغییر مقدار یک خاصیت را اطلاع رسانی خواهد کرد
        /// </summary>
        /// <param name="propertyName">نام خاصیت</param>
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// تغییر مقدار یک خاصیت را اطلاع رسانی خواهد کرد
        /// </summary>
        /// <param name="expression">نام خاصیت مورد نظر</param>
        public void NotifyPropertyChanged(Expression<Func<object>> expression)
        {
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
                throw new ArgumentException("Not a property or field", "expression");

            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }

        /// <summary>
        /// آیا در حین نمایش صفحه‌ای دیگر باید به کاربر پیغام داد که اطلاعات ذخیره نشده‌ای وجود دارد؟
        /// </summary>
        public abstract bool ViewModelContextHasChanges { get; }

        public object QueryStringData { get; set; }
    }
}
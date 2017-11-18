namespace MyWpfFramework.Common.MVVM
{
    /// <summary>
    /// از این اینترفیس خالی برای یافتن و علامتگذاری ویوو مدل‌ها استفاده می‌کنیم
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// هر ویوو مدل برنامه باید مشخص کند که در صورت مراجعه به صفحه‌ای دیگر
        /// آیا اطلاعاتی ذخیره نشده دارد یا خیر و از این مقدار جهت نمایش اخطاری به کاربر
        /// استفاده خواهد شد
        /// </summary>
        bool ViewModelContextHasChanges { get; }

        /// <summary>
        /// براي پارامتر از يك صفحه به صفحه‌ي ديگر
        /// </summary>
        object QueryStringData { get; set; }
    }
}
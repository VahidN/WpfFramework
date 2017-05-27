using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MyWpfFramework.Common.WpfValidation;

namespace MyWpfFramework.DomainClasses
{
    /// <summary>
    /// تمام کلاس‌های دومین برنامه از این کلاس پایه مشتق خواهند شد
    /// </summary>
    // AOP: All classes that have INotifyPropertyChanged will have notification code injected into property sets.
    public abstract class BaseEntity : DataErrorInfoBase, INotifyPropertyChanged //پیاده سازی خودکار سیستم اعتبارسنجی یکپارچه
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        [Required(ErrorMessage = "تاریخ ایجاد خالی است")]
        public DateTime CreatedOn { set; get; }

        [Required(ErrorMessage = "تاریخ ایجاد خالی است")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "تاریخ باید حداکثر 50 کاراکتر باشد.")]
        public string CreatedOnPersian { set; get; }

        [Required(ErrorMessage = "ثبت کننده رکورد خالی است")]
        [StringLength(maximumLength: 450, MinimumLength = 1, ErrorMessage = "نام کاربری باید بین 1 تا 450 کاراکتر باشد.")]
        public string CreatedBy { set; get; }

        [Required(ErrorMessage = "آخرین تاریخ به روز رسانی خالی است")]
        public DateTime ModifiedOn { set; get; }

        [Required(ErrorMessage = "ویرایش کننده رکورد خالی است")]
        [StringLength(maximumLength: 450, MinimumLength = 1, ErrorMessage = "نام کاربری باید بین 1 تا 450 کاراکتر باشد.")]
        public string ModifiedBy { set; get; }
    }
}
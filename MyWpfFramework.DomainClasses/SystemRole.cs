using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace MyWpfFramework.DomainClasses
{
    [ImplementPropertyChanged] // AOP
    [ComplexType]
    public class SystemRole
    {
        public bool IsAdmin { set; get; }
        public bool CanAddNewUser { set; get; }

        //todo: سایر نقش‌های مورد نیاز در اینجا به صورت یک خاصیت بولی اضافه خواهند شد
    }
}
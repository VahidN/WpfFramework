using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWpfFramework.DomainClasses
{
    // AOP: All classes that have INotifyPropertyChanged will have notification code injected into property sets.
    [ComplexType]
    public class SystemRole : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsAdmin { set; get; }
        public bool CanAddNewUser { set; get; }

        //todo: سایر نقش‌های مورد نیاز در اینجا به صورت یک خاصیت بولی اضافه خواهند شد
    }
}
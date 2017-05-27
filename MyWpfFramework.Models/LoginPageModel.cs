using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MyWpfFramework.Common.WpfValidation;

namespace MyWpfFramework.Models
{
    // AOP: All classes that have INotifyPropertyChanged will have notification code injected into property sets.
    public class LoginPageModel : DataErrorInfoBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [Required(ErrorMessage = "لطفا نام کاربری را تکمیل نمائید")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "لطفا کلمه عبور را تکمیل نمائید")]
        public string Password { get; set; }
    }
}
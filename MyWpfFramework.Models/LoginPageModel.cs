using System.ComponentModel.DataAnnotations;
using MyWpfFramework.Common.WpfValidation;
using PropertyChanged;

namespace MyWpfFramework.Models
{
    [ImplementPropertyChanged] // AOP
    public class LoginPageModel : DataErrorInfoBase
    {
        [Required(ErrorMessage = "لطفا نام کاربری را تکمیل نمائید")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "لطفا کلمه عبور را تکمیل نمائید")]
        public string Password { get; set; }
    }
}
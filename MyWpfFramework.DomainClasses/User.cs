using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWpfFramework.DomainClasses
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "نام مستعار خالی است.")]
        [StringLength(maximumLength: 450, MinimumLength = 3, ErrorMessage = "نام مستعار باید بین سه تا 450 کاراکتر باشد.")]
        public string FriendlyName { get; set; }

        [Required(ErrorMessage = "نام کاربری خالی است.")]
        [StringLength(maximumLength: 450, MinimumLength = 3, ErrorMessage = "نام کاربری باید بین سه تا 450 کاراکتر باشد.")]
        public string UserName { set; get; }

        [Required(ErrorMessage = "کلمه عبور خالی است")]
        [StringLength(maximumLength: 50, ErrorMessage = "حداکثر طول کلمه عبور 50 کاراکتر است")]
        public string Password { set; get; }

        public bool IsActive { set; get; }

        public SystemRole Role { set; get; }

        public User()
        {
            Role = new SystemRole();
        }
    }
}
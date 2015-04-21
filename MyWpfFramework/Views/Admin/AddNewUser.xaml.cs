using MyWpfFramework.Common.Security;

namespace MyWpfFramework.Views.Admin
{
    /// <summary>
    /// افزودن و مدیریت کاربران سیستم
    /// </summary>
    [PageAuthorization(AuthorizationType.ApplyRequiredRoles, "IsAdmin, CanAddNewUser")]
    public partial class AddNewUser
    {
        /// <summary>
        /// سازنده افزودن و مدیریت کاربران سیستم
        /// </summary>
        public AddNewUser()
        {
            InitializeComponent();            
        }
    }
}
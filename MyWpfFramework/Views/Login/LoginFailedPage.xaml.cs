using MyWpfFramework.Common.Security;

namespace MyWpfFramework.Views.Login
{
    /// <summary>
    /// لاگین موفقیت آمیز نبوده است
    /// </summary>
    [PageAuthorization(AuthorizationType.AllowAnonymous)]
    public partial class LoginFailedPage
    {
        /// <summary>
        /// لاگین موفقیت آمیز نبوده است
        /// </summary>
        public LoginFailedPage()
        {
            InitializeComponent();
        }
    }
}
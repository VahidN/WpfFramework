using MyWpfFramework.Common.Security;

namespace MyWpfFramework.Views.Login
{
    /// <summary>
    /// ورود به سیستم
    /// </summary>
    [PageAuthorization(AuthorizationType.AllowAnonymous)]
    public partial class LoginPage
    {
        /// <summary>
        /// ورود به سیستم
        /// </summary>
        public LoginPage()
        {
            InitializeComponent();
        }
    }
}
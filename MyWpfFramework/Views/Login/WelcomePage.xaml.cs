using MyWpfFramework.Common.Security;

namespace MyWpfFramework.Views.Login
{
    /// <summary>
    /// صفحه خوش آمدید
    /// </summary>
    [PageAuthorization(AuthorizationType.AllowAnonymous)]
    public partial class WelcomePage
    {
        /// <summary>
        /// صفحه خوش آمدید
        /// </summary>
        public WelcomePage()
        {
            InitializeComponent();
        }
    }
}
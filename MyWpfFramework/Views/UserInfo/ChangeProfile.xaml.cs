using MyWpfFramework.Common.Security;

namespace MyWpfFramework.Views.UserInfo
{
    /// <summary>
    /// تغییر مشخصات کاربر جاری
    /// </summary>
    [PageAuthorization(AuthorizationType.FreeForAuthenticatedUsers)]
    public partial class ChangeProfile
    {
        /// <summary>
        /// تغییر مشخصات کاربر جاری
        /// </summary>
        public ChangeProfile()
        {
            InitializeComponent();
        }
    }
}
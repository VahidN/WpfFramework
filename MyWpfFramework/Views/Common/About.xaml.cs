using MyWpfFramework.Common.Security;

namespace MyWpfFramework.Views.Common
{
    /// <summary>
    /// نمایش صفحه درباره برنامه
    /// </summary>
    [PageAuthorization(AuthorizationType.AllowAnonymous)]
    public partial class About
    {
        /// <summary>
        /// نمایش صفحه درباره برنامه
        /// </summary>
        public About()
        {
            InitializeComponent();            
        }
    }
}
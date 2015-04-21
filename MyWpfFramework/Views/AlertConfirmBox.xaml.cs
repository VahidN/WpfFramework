using MyWpfFramework.Common.UI;

namespace MyWpfFramework.Views
{
    /// <summary>
    /// نمایش اطلاعات به کاربر
    /// </summary>
    public partial class AlertConfirmBox
    {
        /// <summary>
        /// نمایش اطلاعات به کاربر
        /// </summary>
        public AlertConfirmBox()
        {
            InitializeComponent();
            this.DataContext = new AlertConfirmBoxViewModel();
        }
    }
}
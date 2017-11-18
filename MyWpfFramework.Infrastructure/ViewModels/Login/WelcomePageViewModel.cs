using MyWpfFramework.Common.MVVM;

namespace MyWpfFramework.Infrastructure.ViewModels.Login
{
    public class WelcomePageViewModel : BaseViewModel
    {
        public string Message => this.QueryStringData.ToString(); // روش دريافت كوئري استرينگ ارسالي از صفحه‌ي قبل

        public override bool ViewModelContextHasChanges => false;
    }
}

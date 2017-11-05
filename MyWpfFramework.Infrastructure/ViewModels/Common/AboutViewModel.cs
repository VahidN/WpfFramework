using MyWpfFramework.Common.MVVM;

namespace MyWpfFramework.Infrastructure.ViewModels.Common
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {

        }

        public override bool ViewModelContextHasChanges => false;
    }
}

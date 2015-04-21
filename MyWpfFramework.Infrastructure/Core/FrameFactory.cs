using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using MyWpfFramework.Common.Logger;
using MyWpfFramework.Common.MVVM;
using MyWpfFramework.Common.Security;
using MyWpfFramework.Common.UI;
using MyWpfFramework.ServiceLayer.Contracts;
using StructureMap;

namespace MyWpfFramework.Infrastructure.Core
{
    /// <summary>
    /// ایجاد یک کنترل فریم سفارشی که قابلیت تزریق وابستگی‌ها را به صورت خودکار دارد
    /// به همراه اعمال مسایل راهبری برنامه که از منوی اصلی دریافت می‌شوند
    /// </summary>
    public class FrameFactory : Frame
    {
        /// <summary>
        /// زمان محو شدن فریم جاری و نمایش فریم بعدی
        /// </summary>
        public static readonly DependencyProperty FadeDurationProperty =
            DependencyProperty.Register("FadeDuration", typeof(Duration),
            typeof(FrameFactory), new FrameworkPropertyMetadata(new Duration(TimeSpan.FromMilliseconds(100))));

        /// <summary>
        /// اطلاعات سراسری برنامه در مورد کاربر جاری را فراهم می‌کند
        /// </summary>
        public IAppContextService AppContextService { set; get; }

        /// <summary>       
        /// FadeDuration will be used as the duration for Fade Out and Fade In animations        
        /// </summary>       
        public Duration FadeDuration
        {
            get { return (Duration)GetValue(FadeDurationProperty); }
            set { SetValue(FadeDurationProperty, value); }
        }

        /// <summary>
        /// سازنده کلاس فریم سفارشی ما
        /// </summary>
        public FrameFactory()
        {
            this.NavigationFailed += onNavigationFailed;
            this.Navigated += onNavigated;
            this.Navigating += onNavigating;

            registerMessenger();

            SmObjectFactory.Container.BuildUp(this);

            this.Loaded += onLoaded;
        }

        private void onLoaded(object sender, RoutedEventArgs e)
        {
            wireUpAllUserControlsOnTheWindow();
            showLoginPageOnStartup();
        }

        private void wireUpAllUserControlsOnTheWindow()
        {
            var parent = this.FindParent<Window>();
            if (parent == null)
                throw new InvalidOperationException("پنجره دربرگیرنده فریم برنامه یافت نشد");

            foreach (var userControl in parent.FindVisualChildren<UserControl>())
            {
                new ViewModelFactory(userControl, SmObjectFactory.Container).WireUp();
            }
        }

        private void showLoginPageOnStartup()
        {
            if (!AppContextService.IsCurrentUserAuthenticated)
            {
                Redirect.ToLoginPage();
            }
        }

        private void onNavigating(object sender, NavigatingCancelEventArgs e)
        {
            fadeOut(e);
            checkPermissions(e);
        }

        private void checkPermissions(NavigatingCancelEventArgs e)
        {
            //اینجا بهترین مکان برای اعمال مباحث اعتبار سنجی ورود به صفحات است
            //چون قبل از بارگذاری صفحه اعمال می‌شود            
            var attribute = PageAuthorizationScanner.GetPageAuthorizationAttribute(e.Uri);
            if (!AppContextService.CanCurrentUserNavigateTo(attribute))
            {
                e.Cancel = true; //صفحه نمایش داده نشود
                Redirect.ToLoginFailedPage();
            }
        }

        private void fadeOut(NavigatingCancelEventArgs e)
        {
            // if we did not internally initiate the navigation:           
            //   1. cancel the navigation,            
            //   2. cache the target,            
            //   3. disable hittesting during the fade, and            
            //   4. fade out the current content   
            if (Content != null && !_allowDirectNavigation && _contentPresenter != null)
            {
                e.Cancel = true;
                _navArgs = e;
                _contentPresenter.IsHitTestVisible = false;
                var da = new DoubleAnimation(0.0d, FadeDuration) { DecelerationRatio = 1.0d };
                da.Completed += fadeOutCompleted;
                _contentPresenter.BeginAnimation(OpacityProperty, da);
            }
            _allowDirectNavigation = false;
        }

        private void fadeOutCompleted(object sender, EventArgs e)
        {
            // after the fade out          
            //   1. re-enable hittesting        
            //   2. initiate the delayed navigation       
            //   3. invoke the FadeIn animation at Loaded priority          
            var animationClock = sender as AnimationClock;
            if (animationClock != null)
            {
                animationClock.Completed -= fadeOutCompleted;
            }
            if (_contentPresenter == null)
                return;

            _contentPresenter.IsHitTestVisible = true;
            _allowDirectNavigation = true;
            switch (_navArgs.NavigationMode)
            {
                case NavigationMode.New:
                    if (_navArgs.Uri == null)
                    {
                        NavigationService.Navigate(_navArgs.Content);
                    }
                    else
                    {
                        NavigationService.Navigate(_navArgs.Uri);
                    }
                    break;
                case NavigationMode.Back:
                    NavigationService.GoBack();
                    break;
                case NavigationMode.Forward:
                    NavigationService.GoForward();
                    break;
                case NavigationMode.Refresh:
                    NavigationService.Refresh();
                    break;
            }

            Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (ThreadStart)delegate
                {
                    var da = new DoubleAnimation(1.0d, FadeDuration) { AccelerationRatio = 1.0d };
                    _contentPresenter.BeginAnimation(OpacityProperty, da);
                });
        }

        private bool _allowDirectNavigation;
        private ContentPresenter _contentPresenter;
        private NavigatingCancelEventArgs _navArgs;

        private void onNavigated(object sender, NavigationEventArgs e)
        {
            //برای ارسال کوئری استرینگ به شکل پیغام مناسب است
            if (_queryString == null) return;
            Messenger.Default.Send(_queryString, "QueryStringData"); //QueryStringData متفاوت است
            _queryString = null;
        }

        private void onNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            //نمایش خطا به یوزر
            new SendMsg().ShowMsg(new AlertConfirmBoxModel
            {
                ErrorTitle = "خطای نمایش صفحه",
                Errors = ExceptionLogger.GetExceptionMessages(e.Exception),
                ShowCancel = Visibility.Collapsed,
                ShowConfirm = Visibility.Visible
            }, e.Exception);
            e.Handled = true;
        }

        //it will receive urls from MenuViewModel
        private void registerMessenger() //todo: NavigateTo(string pageName, object arguments)
        {
            Messenger.Default.Register<string>(this, "MyNavigationService", doNavigate);
            Messenger.Default.Register<object>(this, "QueryString", doloadData);//QueryString متفاوت است
        }

        object _queryString;
        private void doloadData(object data)
        {
            _queryString = data;
        }

        private void doNavigate(string uri)
        {
            //اگر اطلاعاتی برای ذخیره شدن وجود دارد ابتدا به کاربر پیغام داده خواهد شد
            if (currentViewModelInstance != null && currentViewModelInstance.ViewModelContextHasChanges)
            {
                manageUnSavedData(uri);
            }
            else
            {
                //هدایت کاربر به صفحه درخواستی
                this.Navigate(new Uri(uri, UriKind.Relative));
            }
        }

        private readonly SendMsg _sendMsg = new SendMsg(); // do not garbage collect our messengers!
        private void manageUnSavedData(string uri)
        {
            _sendMsg.ShowMsg(new AlertConfirmBoxModel
            {
                ErrorTitle = "تائید هدایت به صفحه‌ای دیگر",
                Errors = new List<string> 
                    {
                         "در صفحه جاری اطلاعات ذخیره نشده‌ای وجود دارند.",
                         " آیا مایل هستید ابتدا آنها را ذخیره کنید؟" 
                    },
                ShowConfirm = Visibility.Visible,
                ShowCancel = Visibility.Visible
            },
            ex: null,
            confirmed: input => stayOnThePage(input),
            cancelled: input => navigateFromThisPage(input, uri));
        }

        private void navigateFromThisPage(AlertConfirmBoxModel input, string uri)
        {
            // خیر. بنابراین به صفحه درخواستی او را هدایت خواهیم کرد
            this.Navigate(new Uri(uri, UriKind.Relative));
        }

        private void stayOnThePage(AlertConfirmBoxModel input)
        {
            // بله. پس در همین صفحه باقی خواهیم ماند
            return;
        }

        private ViewModelFactory _currentViewModelFactory;
        /// <summary>
        /// به این وهله جهت بررسی وجود تغییرات ذخیره نشده نیاز داریم
        /// </summary>
        private IViewModel currentViewModelInstance
        {
            get
            {
                if (_currentViewModelFactory == null)
                    return null;

                return _currentViewModelFactory.ViewModelInstance;
            }
        }

        /// <summary>
        /// در اینجا می‌شود به وهله‌ای از صفحه‌ای که قرار است اضافه گردد دسترسی یافت
        /// </summary>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            var newPage = newContent as FrameworkElement;
            if (newPage == null)
                return;

            _currentViewModelFactory = new ViewModelFactory(newPage, SmObjectFactory.Container);
            _currentViewModelFactory.WireUp(); //کار تزریق وابستگی‌ها و وهله سازی ویوو مدل مرتبط انجام خواهد شد
        }

        /// <summary>
        /// get a reference to the frame's content presenter      
        /// this is the element we will fade in and out    
        /// </summary>
        public override void OnApplyTemplate()
        {
            _contentPresenter = GetTemplateChild("PART_FrameCP") as ContentPresenter;
            base.OnApplyTemplate();
        }
    }
}
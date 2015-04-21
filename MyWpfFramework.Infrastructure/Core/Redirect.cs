using GalaSoft.MvvmLight.Messaging;

namespace MyWpfFramework.Infrastructure.Core
{
    /// <summary>
    /// کلاس راهبری برنامه
    /// </summary>
    public static class Redirect
    {
        const string Token = "MyNavigationService";

        /// <summary>
        /// هدایت کاربر به صفحه لاگین
        /// </summary>
        public static void ToLoginPage()
        {
            Messenger.Default.Send(@"\Views\Login\LoginPage.xaml", Token);
        }

        /// <summary>
        /// هدایت کاربر به صفحه موفقیت آمیز نبودن لاگین
        /// </summary>
        public static void ToLoginFailedPage()
        {
            Messenger.Default.Send(@"\Views\Login\LoginFailedPage.xaml", Token);
        }

        /// <summary>
        /// هدایت کاربر به صفحه خوش آمد گویی
        /// </summary>
        public static void ToWelcomePage()
        {
            Messenger.Default.Send(@"\Views\Login\WelcomePage.xaml", Token);
        }

        /// <summary>
        /// هدایت کاربر به صفحه‌ای مشخص
        /// </summary>
        /// <param name="url">آدرس صفحه</param>
        public static void To(string url)
        {
            Messenger.Default.Send(url, Token);
        }
    }
}
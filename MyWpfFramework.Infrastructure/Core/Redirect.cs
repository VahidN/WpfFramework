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
        /// <param name="queryStringData">ارسال اطلاعات اضافي به صفحه‌ي بعدي</param>
        public static void ToLoginPage(object queryStringData = null)
        {
            To(@"\Views\Login\LoginPage.xaml", queryStringData);
        }

        /// <summary>
        /// هدایت کاربر به صفحه موفقیت آمیز نبودن لاگین
        /// </summary>
        /// <param name="queryStringData">ارسال اطلاعات اضافي به صفحه‌ي بعدي</param>
        public static void ToLoginFailedPage(object queryStringData = null)
        {
            To(@"\Views\Login\LoginFailedPage.xaml", queryStringData);
        }

        /// <summary>
        /// هدایت کاربر به صفحه خوش آمد گویی
        /// </summary>
        /// <param name="queryStringData">ارسال اطلاعات اضافي به صفحه‌ي بعدي</param>
        public static void ToWelcomePage(object queryStringData = null)
        {
            To(@"\Views\Login\WelcomePage.xaml", queryStringData);
        }

        /// <summary>
        /// هدایت کاربر به صفحه‌ای مشخص
        /// </summary>
        /// <param name="url">آدرس صفحه</param>
        /// <param name="queryStringData">ارسال اطلاعات اضافي به صفحه‌ي بعدي</param>
        public static void To(string url, object queryStringData = null)
        {
            Messenger.Default.Send(message: queryStringData, token: FrameFactory.QueryString);
            Messenger.Default.Send(url, Token);
        }
    }
}
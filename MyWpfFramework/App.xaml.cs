using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using MyWpfFramework.Common.Logger;
using MyWpfFramework.Common.Toolkit;
using MyWpfFramework.Common.UI;
using MyWpfFramework.DataLayer.Context;
using MyWpfFramework.Infrastructure.Core;

namespace MyWpfFramework
{
    /// <summary>
    /// آغاز کننده برنامه
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// آغاز کننده برنامه
        /// </summary>
        public App()
        {
            checkSingleInstanceApplication();
            wireUpEvents();
            StartDb.InitDb();
            setCultureInfo();
        }

        private static void setCultureInfo()
        {
            //برای نمایش اعداد به صورت فارسی
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fa-IR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fa-IR");
        }

        private void wireUpEvents()
        {
            this.Exit += appExit;
            this.Deactivated += appDeactivated;
            this.Startup += appStartup;
            this.DispatcherUnhandledException += appDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += currentDomainUnhandledException;
        }

        private void checkSingleInstanceApplication()
        {
            var process = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (process.Length > 1)
            {
                MessageBox.Show("MyWpfFramework is already running ...", "MyWpfFramework", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Shutdown();
            }
        }

        static void appDeactivated(object sender, EventArgs e)
        {
            Memory.ReEvaluateWorkingSet();
        }

        static void appStartup(object sender, StartupEventArgs e)
        {
            reducingCpuConsumptionForAnimations();
        }

        static void reducingCpuConsumptionForAnimations()
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
                 typeof(Timeline),
                 new FrameworkPropertyMetadata { DefaultValue = 20 }
                 );
        }

        static void currentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //این نوع خطاها عموما موارد مدیریت نشده در تردها هستند که بهتر است برنامه اینجا تمام شود
            var exception = (Exception)e.ExceptionObject;
            new SendMsg().ShowMsg(
                    new AlertConfirmBoxModel
                    {
                        ErrorTitle = "خطای مدیریت نشده:",
                        Errors = ExceptionLogger.GetExceptionMessages(exception),
                        ShowCancel = Visibility.Collapsed,
                        ShowConfirm = Visibility.Visible
                    }, exception);
        }

        static void appExit(object sender, ExitEventArgs e)
        {
        }

        private static void appDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            new SendMsg().ShowMsg(
                    new AlertConfirmBoxModel
                    {
                        ErrorTitle = "خطای عمومی:",
                        Errors = ExceptionLogger.GetExceptionMessages(e.Exception),
                        ShowCancel = Visibility.Collapsed,
                        ShowConfirm = Visibility.Visible
                    }, e.Exception);

            e.Handled = true;
        }
    }
}
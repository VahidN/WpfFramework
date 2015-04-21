using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyWpfFramework.Common.Logger
{
    /// <summary>
    /// کلاسی برای ثبت وقایع یک برنامه دسکتاپ
    /// </summary>
    public static class ExceptionLogger
    {
        private static TimeSpan getSystemUpTime()
        {
            using (var upTime = new PerformanceCounter("System", "System Up Time"))
            {
                upTime.NextValue();
                return TimeSpan.FromSeconds(upTime.NextValue());
            }
        }

        private static string getExceptionTypeStack(Exception e)
        {
            if (e.InnerException != null)
            {
                var message = new StringBuilder();
                message.AppendLine(getExceptionTypeStack(e.InnerException));
                message.AppendLine("   " + e.GetType());
                return (message.ToString());
            }
            return "   " + e.GetType();
        }

        private static string getExceptionMessageStack(Exception e)
        {
            if (e.InnerException != null)
            {
                var message = new StringBuilder();
                message.AppendLine(getExceptionMessageStack(e.InnerException));
                message.AppendLine("   " + e.Message);
                return (message.ToString());
            }
            return "   " + e.Message;
        }
        
        private static string getExceptionCallStack(Exception e)
        {
            if (e.InnerException != null)
            {
                var message = new StringBuilder();
                message.AppendLine(getExceptionCallStack(e.InnerException));
                message.AppendLine("--- Next Call Stack:");
                message.AppendLine(e.StackTrace);
                return (message.ToString());
            }
            return e.StackTrace;
        }

        /// <summary>
        /// لیست اطلاعات یک استثنای دریافتی را باز می‌گرداند
        /// </summary>
        /// <param name="exception">استثناء</param>
        /// <returns>لیست اطلاعات</returns>
        public static List<string> GetExceptionMessages(Exception exception)
        {
            var res = new List<string> { "Exception messages: " };

            var msg = getExceptionMessageStack(exception);
            res.Add(msg);

            if (msg.ToLower().Contains("duplicate"))
                res.Add("اطلاعات وارد شده تکراری است.");

            if (msg.ToLower().Contains("a foreign key value cannot be inserted"))
                res.Add("لطفا تمامی فیلدهای ورودی را پیش از ثبت نهایی تکمیل نمائید.");

            return res;
        }

        /// <summary>
        /// اطلاعات یک شیء استثنای دریافتی را در فایل ذخیره می‌کند
        /// </summary>
        /// <param name="exception">شیء استثناء</param>
        public static void LogExceptionToFile(object exception)
        {
            try
            {
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                var errs = GetDetailedException(exception as Exception);
                var res = errs.Aggregate(string.Empty, (current, line) => current + line + Environment.NewLine);
                File.AppendAllText(
                    appPath + @"\ErrosLog.Log",
                    string.Format(@"+-------------------------------------------------------------------+{0}{1}",
                    Environment.NewLine, res));
                //todo: send e-mail
            }
            catch
            {
                /*کاری نمی‌شود کرد. بدترین حالت ممکن است*/
            }
        }

        /// <summary>
        /// لیست اطلاعات یک استثنای دریافتی را باز می‌گرداند
        /// </summary>
        /// <param name="exception">استثنای دریافتی</param>
        /// <returns>لیست اطلاعات</returns>
        public static List<string> GetDetailedException(Exception exception)
        {
            var res = new List<string>();

            var computerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();

            res.Add("Application:       " + Application.ProductName);
            res.Add("Path:              " + Application.ExecutablePath);
            res.Add("Version:           " + Application.ProductVersion);
            res.Add("Date:              " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            res.Add("Computer name:     " + SystemInformation.ComputerName);
            res.Add("CPU Cores:         " + ProcessorInformation.GetCpuCores());
            res.Add("CPU Manufacturer:  " + ProcessorInformation.GetCpuManufacturer());
            res.Add("CPU Clock Speed:   " + ProcessorInformation.GetCpuClockSpeed());
            res.Add("CPU Data Width:    " + ProcessorInformation.GetCpuDataWidth());
            res.Add("User name:         " + SystemInformation.UserName);
            res.Add("OSVersion:         " + computerInfo.OSVersion);
            res.Add("OSPlatform:        " + (Environment.Is64BitOperatingSystem ? "X64" : "X86"));
            res.Add("OSFullName:        " + computerInfo.OSFullName);
            res.Add("Culture:           " + CultureInfo.CurrentCulture.Name);
            res.Add("Resolution:        " + SystemInformation.PrimaryMonitorSize);
            res.Add("System up time:    " + getSystemUpTime());
            res.Add("App up time:       " + (DateTime.Now - Process.GetCurrentProcess().StartTime));
            res.Add("Total memory:      " + computerInfo.TotalPhysicalMemory / (1024 * 1024) + "Mb");
            res.Add("Available memory:  " + computerInfo.AvailablePhysicalMemory / (1024 * 1024) + "Mb");
            //Getting Available Drive Space
            var currentDrive = DriveInfo.GetDrives()
                .Where(x => x.Name.ToLower() == Application.ExecutablePath.Substring(0, 3).ToLower())
                .FirstOrDefault();
            if (currentDrive != null)
            {
                res.Add(string.Format("Drive {0}", currentDrive.Name));
                res.Add(string.Format("Volume label: {0}", currentDrive.VolumeLabel));
                res.Add(string.Format("File system: {0}", currentDrive.DriveFormat));
                res.Add(string.Format("Available space to current user: {0} MB", currentDrive.AvailableFreeSpace / (1024 * 1024)));
                res.Add(string.Format("Total available space: {0} MB", currentDrive.TotalFreeSpace / (1024 * 1024)));
                res.Add(string.Format("Total size of drive: {0} MB ", currentDrive.TotalSize / (1024 * 1024)));
            }

            //Get callerInfo
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(2); //caller of LogExceptionToFile
            var methodBase = stackFrame.GetMethod();
            var callingType = methodBase.DeclaringType;
            res.Add(string.Format("Url: {0} -> {1}", callingType.Assembly.Location, callingType.Assembly.FullName));
            res.Add(string.Format("Caller: {0} -> {1}", callingType.FullName, methodBase.Name));


            res.Add("Exception classes: ");
            res.Add(getExceptionTypeStack(exception));
            res.Add("Exception messages: ");
            res.Add(getExceptionMessageStack(exception));
            res.Add("Stack Traces:");
            res.Add(getExceptionCallStack(exception));
            return res;
        }
    }
}
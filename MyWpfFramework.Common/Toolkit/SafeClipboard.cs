using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace MyWpfFramework.Common.Toolkit
{
    /// <summary>
    /// کپی اطلاعات در تخته برش ویندوز
    /// </summary>
    public static class SafeClipboard
    {
        /// <summary>
        /// این متد 15 بار سعی می‌کند تا اطلاعات مورد نظر را در تخته برش ویندوز کپی کند
        /// </summary>
        /// <param name="text">متن مورد نظر</param>
        public static void ClipboardSetText(this string text)
        {
            for (var i = 0; i < 15; i++)
            {
                try
                {
                    Clipboard.SetText(text);
                    break;
                }
                catch (COMException)
                {
                    Thread.Sleep(10);
                }
            }
        }
    }
}
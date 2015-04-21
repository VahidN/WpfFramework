using System;
using System.Globalization;

namespace MyWpfFramework.Common.PersianToolkit
{
    /// <summary>
    /// دریافت تاریخ به شمسی
    /// </summary>
    public static class PersianDate
    {
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// </summary>
        public static string ShamsiDateTime(this DateTime info, string separator = "/", bool includeHourMinute = true)
        {
            int ym = info.Year;
            int mm = info.Month;
            int dm = info.Day;
            PersianCalendar sss = new PersianCalendar();
            int ys = sss.GetYear(new DateTime(ym, mm, dm, new GregorianCalendar()));
            int ms = sss.GetMonth(new DateTime(ym, mm, dm, new GregorianCalendar()));
            int ds = sss.GetDayOfMonth(new DateTime(ym, mm, dm, new GregorianCalendar()));
            if (includeHourMinute)
                return ys + separator + ms.ToString("00", CultureInfo.InvariantCulture) + separator + ds.ToString("00", CultureInfo.InvariantCulture) + " " + info.Hour.ToString("00") + ":" + info.Minute.ToString("00");
            else
                return ys + separator + ms.ToString("00", CultureInfo.InvariantCulture) + separator + ds.ToString("00", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// گرفتن تاریخ شمسی جاری سیستم
        /// </summary>
        public static string CurrentSystemShamsiDate(string separator = "/", bool includeHourMinute = false)
        {
            var info = DateTime.Now;
            int ym = info.Year;
            int mm = info.Month;
            int dm = info.Day;

            PersianCalendar sss = new PersianCalendar();
            int ys = sss.GetYear(new DateTime(ym, mm, dm, new GregorianCalendar()));
            int ms = sss.GetMonth(new DateTime(ym, mm, dm, new GregorianCalendar()));
            int ds = sss.GetDayOfMonth(new DateTime(ym, mm, dm, new GregorianCalendar()));
            if (!includeHourMinute)
            {
                return ys + separator + ms.ToString("00", CultureInfo.InvariantCulture) + separator + ds.ToString("00", CultureInfo.InvariantCulture);
            }
            else
            {
                return ys + separator + ms.ToString("00", CultureInfo.InvariantCulture) + separator + ds.ToString("00", CultureInfo.InvariantCulture) + " " + info.Hour + ":" + info.Minute;
            }
        }
    }
}
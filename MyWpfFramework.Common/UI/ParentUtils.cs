using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MyWpfFramework.Common.UI
{
    /// <summary>
    /// کار یافتن والد یک شیء یا فرزندان او را در دبلیو پی اف تسهیل می‌کند
    /// </summary>
    public static class ParentUtils
    {
        /// <summary>
        /// کار یافتن فرزندانی از جنس مشخص را انجام می‌دهد
        /// </summary>
        /// <typeparam name="T">نوع فرزند مد نظر</typeparam>
        /// <param name="depObj">شیء والد</param>
        /// <returns>لیست فرزندان</returns>
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// والد یک شیء را بر اساس نوعی مشخص می‌یابد
        /// </summary>
        /// <typeparam name="T">نوع والد مد نظر</typeparam>
        /// <param name="element">شیء جاری</param>
        /// <returns>والد احتمالی یافت شده</returns>
        public static T FindParent<T>(this FrameworkElement element) where T : FrameworkElement
        {
            var parent = LogicalTreeHelper.GetParent(element) as FrameworkElement;
            while (parent != null)
            {
                var correctlyTyped = parent as T;
                return correctlyTyped ?? FindParent<T>(parent);
            }

            return null;
        }

        /// <summary>
        ///     Walks up the templated parent tree looking for a parent type.
        /// </summary>
        public static T FindTemplatedParent<T>(this FrameworkElement element) where T : FrameworkElement
        {
            var parent = element.TemplatedParent as FrameworkElement;

            while (parent != null)
            {
                var correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = parent.TemplatedParent as FrameworkElement;
            }

            return null;
        }

        /// <summary>
        /// یافتن والد یک شیء
        /// </summary>
        public static T FindVisualParent<T>(this UIElement element) where T : UIElement
        {
            var parent = element;
            while (parent != null)
            {
                var correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }
    }
}
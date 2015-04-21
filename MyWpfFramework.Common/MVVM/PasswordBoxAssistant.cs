using System.Windows;
using System.Windows.Controls;

namespace MyWpfFramework.Common.MVVM
{
    //from : http://blog.functionalfun.net/2008/06/wpf-passwordbox-and-data-binding.html
    /// <summary>
    /// پسورد باکس در دبلیو پی اف برای مباحث انقیاد داده‌ها مناسب نیست به همین جهت نیاز به راه کار زیر وجود دارد
    /// </summary>
    public static class PasswordBoxAssistant
    {
        #region Fields (3)

        /// <summary>
        /// آیا انقیاد به پسورد باکس فعال شود؟
        /// </summary>
        public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached(
            "BindPassword", typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(false, onBindPasswordChanged));

        /// <summary>
        /// پسورد وارد شده را باز می‌گرداند و جهت انقیاد داده‌ها در معرض استفاده قرار می‌دهد
        /// </summary>
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxAssistant), new FrameworkPropertyMetadata(string.Empty, onBoundPasswordChanged));
        
        private static readonly DependencyProperty UpdatingPassword =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxAssistant));

        #endregion Fields

        #region Methods (9)

        // Public Methods (4) 

        /// <summary>
        /// آیا انقیاد به پسورد باکس فعال شود؟
        /// </summary>        
        public static bool GetBindPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(BindPassword);
        }

        /// <summary>
        /// پسورد وارد شده را باز می‌گرداند و جهت انقیاد داده‌ها در معرض استفاده قرار می‌دهد
        /// </summary>
        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BoundPassword);
        }

        /// <summary>
        /// آیا انقیاد به پسورد باکس فعال شود؟
        /// </summary>
        public static void SetBindPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(BindPassword, value);
        }

        /// <summary>
        /// پسورد وارد شده را باز می‌گرداند و جهت انقیاد داده‌ها در معرض استفاده قرار می‌دهد
        /// </summary>
        public static void SetBoundPassword(DependencyObject dp, string value)
        {
            dp.SetValue(BoundPassword, value);
        }
        // Private Methods (5) 

        private static bool getUpdatingPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdatingPassword);
        }

        private static void handlePasswordChanged(object sender, RoutedEventArgs e)
        {
            var box = sender as PasswordBox;
            if (box == null || !box.IsFocused) return;

            // set a flag to indicate that we're updating the password
            setUpdatingPassword(box, true);
            // push the new password into the BoundPassword property
            SetBoundPassword(box, box.Password);
            setUpdatingPassword(box, false);
        }

        private static void onBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            // when the BindPassword attached property is set on a PasswordBox,
            // start listening to its PasswordChanged event

            var box = dp as PasswordBox;

            if (box == null)
            {
                return;
            }

            var wasBound = (bool)(e.OldValue);
            var needToBind = (bool)(e.NewValue);

            if (wasBound)
            {
                box.PasswordChanged -= handlePasswordChanged;
            }

            if (needToBind)
            {
                box.PasswordChanged += handlePasswordChanged;                
            }
        }

        private static void onBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as PasswordBox;
            if (box == null) return;

            // only handle this event when the property is attached to a PasswordBox
            // and when the BindPassword attached property has been set to true
            if (d == null || !GetBindPassword(d))
            {
                return;
            }

            // avoid recursive updating by ignoring the box's changed event
            box.PasswordChanged -= handlePasswordChanged;

            var newPassword = (string)e.NewValue;

            if (!getUpdatingPassword(box))
            {
                box.Password = newPassword;
            }

            box.PasswordChanged += handlePasswordChanged;
        }

        private static void setUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPassword, value);
        }

        #endregion Methods
    }
}
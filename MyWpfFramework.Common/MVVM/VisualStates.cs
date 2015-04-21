using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace MyWpfFramework.Common.MVVM
{
    /// <summary>
    /// جهت فراهم آوردن امکانات انقیاد داده‌های مخصوص VisualStateManager طراحی شده است
    /// </summary>
    public class VisualStates : VisualStateManager
    {
        #region Fields (3)

        static string _lastState = string.Empty;
        static VisualState _visualState;
        /// <summary>
        /// انقیاد اطلاعات به وضعیت جاری VisualStateManager
        /// </summary>
        public static readonly DependencyProperty CurrentStateProperty =
            DependencyProperty.RegisterAttached(
            "CurrentState",
            typeof(String),
            typeof(VisualStates),
            new PropertyMetadata(transitionToState));

        #endregion Fields

        #region Methods (5)

        // Public Methods (3) 

        /// <summary>
        /// انقیاد اطلاعات به وضعیت جاری VisualStateManager
        /// </summary>
        public static string GetCurrentState(DependencyObject obj)
        {
            return (string)obj.GetValue(CurrentStateProperty);
        }

        /// <summary>
        /// انقیاد اطلاعات به وضعیت جاری VisualStateManager
        /// </summary>
        public static void SetCurrentState(DependencyObject obj, string value)
        {
            obj.SetValue(CurrentStateProperty, value);
        }

        /// <summary>
        /// دریافت وضعیت فعلی VisualStateManager
        /// </summary>
        public static VisualState TryGetVisualState(string stateName, FrameworkElement ctrl)
        {
            var groups = GetVisualStateGroups(ctrl);
            if (groups.Count == 0)
            {
                var templateControl = ctrl as Control;
                if (templateControl != null)
                    groups = GetVisualStateGroups(templateControl.GetType()
                        .GetProperty("TemplateChild", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(templateControl, null) as FrameworkElement);
            }
            return groups.OfType<VisualStateGroup>().SelectMany(g => g.States.OfType<VisualState>()).FirstOrDefault(s => s.Name == stateName);
        }
        // Private Methods (2) 

        static void storyboardCompleted(object sender, EventArgs e)
        {
            Messenger.Default.Send(_lastState, "StoryboardCompleted");
            _visualState.Storyboard.Completed -= storyboardCompleted;
        }

        private static void transitionToState(object sender, DependencyPropertyChangedEventArgs args)
        {
            var c = sender as FrameworkElement;
            if (c == null)
            {
                throw new ArgumentException("CurrentState is only supported on the FrameworkElement type");
            }

            _lastState = args.NewValue.ToString();

            _visualState = TryGetVisualState(_lastState, c);
            if (_visualState != null && _visualState.Storyboard != null)
            {
                _visualState.Storyboard.Completed += storyboardCompleted;
            }

            GoToElementState(c, _lastState, false);
        }

        #endregion Methods
    }
}
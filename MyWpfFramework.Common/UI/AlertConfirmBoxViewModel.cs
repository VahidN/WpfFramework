using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MyWpfFramework.Common.MVVM;
using MyWpfFramework.Common.Toolkit;

namespace MyWpfFramework.Common.UI
{
    /// <summary>
    /// ویوو مدل مسیج باکس سفارشی برنامه
    /// </summary>
    public class AlertConfirmBoxViewModel : BaseViewModel
    {
        #region Fields (2)

        AlertConfirmBoxModel _alertConfirmBoxModel;
        readonly List<AlertConfirmBoxModel> _localCache;

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// سازنده کلاس آلرت باکس سفارشی برنامه
        /// </summary>
        public AlertConfirmBoxViewModel()
        {
            _localCache = new List<AlertConfirmBoxModel>();
            setupMessengers();
            AlertConfirmBoxModel = new AlertConfirmBoxModel
            {
                IsVisible = Visibility.Collapsed,
                ShowConfirm = Visibility.Collapsed,
                ShowCancel = Visibility.Collapsed,
                CurrentState = "Normal"
            };
            setupCommands();
        }

        #endregion Constructors

        #region Properties (5)

        /// <summary>
        /// اطلاعات مورد نیاز جهت نمایش در آلرت باکس
        /// </summary>
        public AlertConfirmBoxModel AlertConfirmBoxModel
        {
            set
            {
                _alertConfirmBoxModel = value;
                NotifyPropertyChanged("AlertConfirmBoxModel");
            }
            get { return _alertConfirmBoxModel; }
        }

        /// <summary>
        /// رخداد کلیک بر روی دکمه لغو را دریافت می‌کند
        /// </summary>
        public RelayCommand CancelCommand { set; get; }

        /// <summary>
        /// رخداد کلیک بر روی دکمه تائید را دریافت می‌کند
        /// </summary>
        public RelayCommand ConfirmCommand { set; get; }

        /// <summary>
        /// رخداد کلیک بر روی دکمه کپی را دریافت می‌کند
        /// </summary>
        public RelayCommand CopyCommand { set; get; }

        /// <summary>
        /// رخداد کلیک بر روی دکمه ذخیره را دریافت می‌کند
        /// </summary>
        public RelayCommand SaveCommand { set; get; }

        #endregion Properties

        #region Methods (10)

        // Private Methods (10) 

        void cancelCommand()
        {
            Messenger.Default.Send("Cancelled", "AlertSysResult");
            AlertConfirmBoxModel.CurrentState = "HideAnim"; //animation            
            startHide();
        }

        void confirmCommand()
        {
            Messenger.Default.Send("Confirmed", "AlertSysResult");
            AlertConfirmBoxModel.CurrentState = "HideAnim"; //animation            
            startHide();
        }

        void copyCommand()
        {
            if (AlertConfirmBoxModel.Errors == null || !AlertConfirmBoxModel.Errors.Any()) return;
            var res = AlertConfirmBoxModel.Errors.Aggregate(string.Empty, (current, line) => current + line + Environment.NewLine);
            res.ClipboardSetText();
        }

        void doUpdateModel(AlertConfirmBoxModel input)
        {
            if (input != null)
            {
                _localCache.Add(input);
            }

            if (AlertConfirmBoxModel.IsVisible == Visibility.Visible) return;

            if (!_localCache.Any())
                return;

            //show 1st cached item.
            AlertConfirmBoxModel.CurrentState = "FlashAnim"; //animation
            AlertConfirmBoxModel.Errors = _localCache[0].Errors;
            AlertConfirmBoxModel.ErrorTitle = _localCache[0].ErrorTitle;
            AlertConfirmBoxModel.IsVisible = _localCache[0].IsVisible;
            AlertConfirmBoxModel.ShowCancel = _localCache[0].ShowCancel;
            AlertConfirmBoxModel.ShowConfirm = _localCache[0].ShowConfirm;

            _localCache.RemoveAt(0);
        }

        void hideIt()
        {
            AlertConfirmBoxModel.Errors = new List<string>();
            AlertConfirmBoxModel.ErrorTitle = string.Empty;
            AlertConfirmBoxModel.IsVisible = Visibility.Collapsed;
        }

        void saveCommand()
        {
            //todo: ...
        }

        private void setupCommands()
        {
            ConfirmCommand = new RelayCommand(confirmCommand);
            CancelCommand = new RelayCommand(cancelCommand);
            CopyCommand = new RelayCommand(copyCommand);
            SaveCommand = new RelayCommand(saveCommand);
        }

        private void setupMessengers()
        {
            Messenger.Default.Register<string>(this, "StoryboardCompleted", storyboardCompleted);
            Messenger.Default.Register<AlertConfirmBoxModel>(this, "MyMessengerService", doUpdateModel);
        }

        private void startHide()
        {
            AlertConfirmBoxModel.ShowCancel = Visibility.Collapsed;
            AlertConfirmBoxModel.ShowConfirm = Visibility.Collapsed;
        }

        void storyboardCompleted(string state)
        {
            switch (state)
            {
                case "HideAnim":
                    {
                        hideIt();
                        //show next cached item
                        doUpdateModel(null);
                    }
                    break;
                case "FlashAnim":
                    break;
            }
        }

        #endregion Methods

        /// <summary>
        /// آیا در حین نمایش صفحه‌ای دیگر باید به کاربر پیغام داد که اطلاعات ذخیره نشده‌ای وجود دارد؟
        /// </summary>
        public override bool ViewModelContextHasChanges
        {
            get { return false; }
        }
    }
}
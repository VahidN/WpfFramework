using System;
using System.Threading;
using GalaSoft.MvvmLight.Messaging;
using MyWpfFramework.Common.Logger;

namespace MyWpfFramework.Common.UI
{
    /// <summary>
    /// جهت نمایش آلرت باکس برنامه طراحی شده است
    /// </summary>
    public class SendMsg
    {
        #region Fields (3)

        Action<AlertConfirmBoxModel> _cancelled;
        Action<AlertConfirmBoxModel> _confirmed;
        AlertConfirmBoxModel _input;

        #endregion Fields

        #region Methods (2)

        // Public Methods (1) 

        /// <summary>
        /// نمایش یک پیغام به کاربر
        /// </summary>
        /// <param name="input">اطلاعاتی جهت نمایش به کاربر</param>
        /// <param name="ex">شیء استثناء در صورت وجود</param>
        /// <param name="confirmed">اگر تائید شد پس از آن چه رخ دهد؟</param>
        /// <param name="cancelled">اگر رد شد پس از آن چه رخ دهد؟</param>
        public void ShowMsg(AlertConfirmBoxModel input,
                            Exception ex = null,
                            Action<AlertConfirmBoxModel> confirmed = null,
                            Action<AlertConfirmBoxModel> cancelled = null)
        {
            _cancelled = cancelled;
            _confirmed = confirmed;
            _input = input;
            Messenger.Default.Register<string>(this, "AlertSysResult", alertSysResult);
            Messenger.Default.Send(input, "MyMessengerService");

            if (ex != null)
            {
                //don't block UI...
                new Thread(ExceptionLogger.LogExceptionToFile).Start(ex);
            }
        }
        // Private Methods (1) 

        void alertSysResult(string res)
        {
            switch (res)
            {
                case "Confirmed":
                    if (_confirmed != null)
                        _confirmed.Invoke(_input);

                    Messenger.Default.Unregister<string>(this);
                    break;
                case "Cancelled":
                    if (_cancelled != null)
                        _cancelled.Invoke(_input);

                    Messenger.Default.Unregister<string>(this);
                    break;
                default:
                    break;
            }
        }

        #endregion Methods
    }
}
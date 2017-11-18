using System.Windows;
using StructureMap;

namespace MyWpfFramework.Common.MVVM
{
    /// <summary>
    /// Stitches together a view and its view-model
    /// </summary>
    public class ViewModelFactory
    {
        private readonly FrameworkElement _control;
        private readonly IContainer _container;
        private readonly object _queryStringData;

        /// <summary>
        /// سازنده کلاس تزریق وابستگی‌ها به ویوو مدل و وهله سازی آن
        /// </summary>
        /// <param name="control">وهله‌ای از شیءایی که باید کار تزریق وابستگی‌ها در آن انجام شود</param>
        /// <param name="container">SM Container</param>
        /// <param name="queryStringData">Query String Data</param>
        public ViewModelFactory(FrameworkElement control, IContainer container, object queryStringData)
        {
            _control = control;
            _container = container;
            _queryStringData = queryStringData;
        }

        /// <summary>
        /// وهله متناظر با ویوو مدل
        /// </summary>
        public IViewModel ViewModelInstance { get; private set; }

        /// <summary>
        /// کار تزریق خودکار وابستگی‌ها و وهله سازی ویوو مدل مرتبط انجام خواهد شد
        /// </summary>
        public void WireUp()
        {
            var viewName = _control.GetType().Name;
            var viewModelName = string.Concat(viewName, "ViewModel"); //قرار داد نامگذاری ما است

            if (!_control.IsLoaded)
            {
                _control.Loaded += (s, e) =>
                {
                    setDataContext(viewModelName);
                };
            }
            else
            {
                setDataContext(viewModelName);
            }
        }

        private void setDataContext(string viewModelName)
        {
            //کار تزریق خودکار وابستگی‌ها و وهله سازی ویوو مدل مرتبط انجام خواهد شد
            ViewModelInstance = _container.TryGetInstance<IViewModel>(viewModelName);
            if (ViewModelInstance == null) // این صفحه ویوو مدل ندارد
                return;

            ViewModelInstance.QueryStringData = _queryStringData;
            _control.DataContext = ViewModelInstance;
        }
    }
}
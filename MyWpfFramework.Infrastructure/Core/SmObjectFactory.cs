using MyWpfFramework.Common.Config;
using MyWpfFramework.Common.MVVM;
using MyWpfFramework.DataLayer.Context;
using MyWpfFramework.ServiceLayer;
using MyWpfFramework.ServiceLayer.Contracts;
using StructureMap;
using System;
using System.Threading;

namespace MyWpfFramework.Infrastructure.Core
{
    /// <summary>
    /// تنظيمات تزريق وابستگي‌هاي برنامه در اينجا انجام مي‌شوند
    /// </summary>
    public static class SmObjectFactory
    {
        private static readonly Lazy<Container> _containerBuilder =
            new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// SM Container.
        /// </summary>
        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        /// <summary>
        /// اعمال تنظيمات تزريق وابستگي‌ها
        /// </summary>ummary>
        private static Container defaultContainer()
        {
            return new Container(cfg =>
            {
                // نکته: در برنامه‌های دسکتاپ نیازی به استفاده از حالت
                // HybridHttpOrThreadLocalScoped
                // نیست چون سبب عدم رها شدن منابع می‌گردد و در این حالت کل صفحات برنامه با یک کانتکست کار خواهد کرد
                // مگر اینکه مانند برنامه‌های وب در آخر هر درخواست، کار رها سازی منابع به صورت دستی انجام شود
                cfg.For<IUnitOfWork>().Use(() => new MyWpfFrameworkContext());

                //علت سینگلتون تعریف شدن وهله در اینجا:
                //هربار فقط یک کاربر در برنامه دسکتاپ وارد می‌شود
                //همچنین نیاز داریم اطلاعات کاربر لاگین شده را به صورت سراسری
                //جهت اعتبارسنجی‌های ویژه صفحات مختلف نگه داری کنیم
                cfg.For<IAppContextService>().Singleton().Use<AppContextService>();

                cfg.Policies.SetAllProperties(properties =>
                {
                    properties.OfType<IAppContextService>();
                });

                //todo: اگر نیاز است سایر تنظیمات در اینجا اضافه خواهند شد

                cfg.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssemblyContainingType<IUsersService>();
                    scan.AssemblyContainingType<IConfigSetGet>();

                    // Add all types that implement IView into the container,
                    // and name each specific type by the short type name.
                    scan.With(new SingletonConvention<IViewModel>()); // The lifetime of added ViewModels is Transient. Use this method to change them.
                    scan.AddAllTypesOf<IViewModel>().NameBy(type => type.Name); // with default lifecycle => (Transient)

                    // این نکته حجم زیادی از کدهای تکراری تعاریف اولیه را کاهش می‌دهد
                    // Wire up all I`Test` interfaces with `Test` classes.
                    scan.WithDefaultConventions();
                });

            });
        }
    }
}
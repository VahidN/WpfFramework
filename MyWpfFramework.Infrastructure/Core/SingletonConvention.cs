using System.Linq;
using MyWpfFramework.Infrastructure.ViewModels.Common;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;
using StructureMap.TypeRules;

namespace MyWpfFramework.Infrastructure.Core
{
    public class SingletonConvention<TPluginFamily> : IRegistrationConvention
    {
        public void ScanTypes(TypeSet types, Registry registry)
        {
            foreach(var type in types.AllTypes())
            {
                if (!type.IsConcrete() ||
                    !type.CanBeCreated() ||
                    !type.AllInterfaces().Contains(typeof(TPluginFamily)))
                {
                    continue;
                }

                if (type == typeof(AboutViewModel)) // Just a sample. Remove this check, if you want all of your ViewModels become Singleton.
                {
                    // How to mark a view model as Singleton
                    registry.For(typeof(TPluginFamily)).Singleton().Use(type).Named(type.Name);
                }
            }
        }
    }
}
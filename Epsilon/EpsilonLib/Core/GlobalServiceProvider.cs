using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace EpsilonLib.Core
{
    public class GlobalServiceProvider
    {
        private static CompositionContainer _container;

        public static void Initialize(CompositionContainer container)
        {
            _container = container;
        }

        public static T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public static object GetService(Type type)
        {
            string contract = AttributedModelServices.GetContractName(type);
            return _container.GetExports<object>(contract).SingleOrDefault()?.Value ?? default;
        }
    }
}

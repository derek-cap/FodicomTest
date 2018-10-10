using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest
{
    class ServiceLocator
    {
        private static IContainer _container;
        /// <summary>
        /// Container to resolve components/service.
        /// </summary>
        public static IContainer Container => _container;

        private static ServiceLocator _instance;
        public static ServiceLocator Instance
        {
            get { return _instance ?? (_instance = new ServiceLocator()); }
        }

        private ServiceLocator()
        {
            Register();
        }

        /// <summary>
        /// Get the instance of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        private void Register()
        {
            // Create the builder with which components/services are registered.
            var builder = new ContainerBuilder();

            _container = builder.Build();
        }
    }
}

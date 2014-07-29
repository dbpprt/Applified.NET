using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;

namespace Applified.Common.OwinDependencyInjection
{

    public class UnityDependencyResolver : IDependencyResolver, IServiceProvider, IUnityServiceProvider
    {
        private readonly IUnityContainer _container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        public IDependencyScope BeginScope()
        {
            return new UnityDependencyResolver(_container.CreateChildContainer());
        }

        public IUnityContainer GetUnderlayingContainer()
        {
            return _container;
        }
    }
}


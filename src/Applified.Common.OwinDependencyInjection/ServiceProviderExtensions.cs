using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

namespace Applified.Common.OwinDependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static T Resolve<T>(this IServiceProvider provider)
        {
            return (T) provider.GetService(typeof (T));
        }

        public static IEnumerable<T> ResolveAll<T>(this IDependencyResolver provider)
        {
            return provider.GetServices(typeof(T)).OfType<T>();
        }

        public static T Resolve<T>(this IDependencyResolver provider)
        {
            return (T)provider.GetService(typeof(T));
        }

        public static T Resolve<T>(this IDependencyScope provider)
        {
            return (T)provider.GetService(typeof(T));
        }

        public static IEnumerable<T> ResolveAll<T>(this IDependencyScope provider)
        {
            return provider.GetServices(typeof(T)).OfType<T>();
        }
    }
}

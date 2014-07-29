using System;
using Microsoft.Practices.Unity;

namespace Applified.Common.Unity
{
    public static class UnityExtensions
    {
        public static IUnityContainer RegisterModule<TModule>(this IUnityContainer container) where TModule : IUnityModule
        {
            var module = Activator.CreateInstance<TModule>();
            module.RegisterDependencies(container);

            return container;
        }
    }
}

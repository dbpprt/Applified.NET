using System;
using Microsoft.Practices.Unity;

namespace Applified.Common.OwinDependencyInjection
{
    public interface IUnityServiceProvider : IDisposable
    {
        IUnityContainer GetUnderlayingContainer();
    }
}
using System;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Microsoft.Practices.Unity;
using Owin;

namespace Applified.Core.Extensibility.Contracts
{
    public interface IApplicationEventHandler
    {
        Task OnStartup(IUnityContainer container, IDependencyScope scope);

        void OnShutdown();
    }
}

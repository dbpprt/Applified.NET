using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Applified.Common;
using Applified.Common.Unity;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Owin;

namespace Applified.Core.Extensibility
{
    public abstract class FeatureBase : IUnityModule
    {
        public abstract Guid FeatureId { get; }

        public abstract int ExecutionOrderKey { get; }

        public virtual void RegisterDependencies(IUnityContainer container) { }

        public virtual void OnStartup(IAppBuilder app) { }

        public virtual void OnShutdown() { }

        public abstract Task<OwinMiddleware> UseAsync(Guid applicationId, OwinMiddleware next, IAppBuilder appBuilder, IDependencyScope scope);
    }
}

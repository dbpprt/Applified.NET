using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public virtual void RegisterDependencies(IUnityContainer container) { }

        public virtual void OnStartup(IAppBuilder app) { }

        public virtual void OnShutdown() { }

        public abstract OwinMiddleware GetTenantMiddleware(
            Guid applicationId, 
            OwinMiddleware next,
            IAppBuilder appBuilder);
    }
}

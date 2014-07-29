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
    public abstract class FeatureBase : OwinMiddleware, IUnityModule
    {
        public abstract Guid FeatureId { get; }

        public Dictionary<string, string> GlobalSettings { get; protected set; } 

        protected FeatureBase(OwinMiddleware next) 
            : base(next)
        {
            GlobalSettings = new Dictionary<string, string>();
        }

        protected FeatureBase() : base(new NoopMiddleware())
        {
            GlobalSettings = new Dictionary<string, string>();
        }

        public virtual void RegisterDependencies(IUnityContainer container) { }

        public virtual void OnStartup(IAppBuilder app) { }

        public virtual void OnShutdown() { }
    }
}

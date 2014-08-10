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

    /// <summary>
    /// Base class for features
    /// TODO: This class is far from being perfect... A lot of refactoring is required to make this more usable
    /// </summary>
    public abstract class FeatureBase : IUnityModule
    {
        public abstract SettingsBase GetSettings(Dictionary<string, string> dictionary);

        public abstract Guid FeatureId { get; }

        public abstract int ExecutionOrderKey { get; }

        public abstract string AssemblyName { get; }

        public virtual void RegisterDependencies(IUnityContainer container) { }

        public virtual void OnStartup(IAppBuilder app) { }

        public virtual void OnShutdown() { }

        public virtual List<AvaliableSetting> GetAvaliableSettings()
        {
            return GetSettings(null).GetAvaliableSettings();
        }

        public abstract Task<OwinMiddleware> UseAsync(Guid applicationId, OwinMiddleware next, IAppBuilder appBuilder, IDependencyScope scope);
    }
}

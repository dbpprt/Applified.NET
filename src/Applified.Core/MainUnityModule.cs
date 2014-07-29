using Applified.Common.OwinDependencyInjection;
using Applified.Common.Unity;
using Applified.Core.Extensibility.Contracts;
using Applified.Core.Handlers;
using Applified.Core.ServiceContracts;
using Applified.Core.Services;
using Applified.Core.Services.Services;
using Microsoft.Practices.Unity;

namespace Applified.Core
{
    class MainUnityModule : UnityModule
    {
        public override void RegisterDependencies(IUnityContainer container)
        {
            container
                .RegisterModule<ServiceUnityModule>()

                .RegisterType<ICurrentApplication, MultiTenancyProvider>(new HierarchicalLifetimeManager())
                .RegisterType<IDynamicRegistrationDelegate, MultiTenancyProvider>("MultiTenancyProvider",
                    new HierarchicalLifetimeManager())

                .RegisterType<IUrlBuilderService, UrlBuilderService>(new HierarchicalLifetimeManager())
                .RegisterType<IStorageService, DatabaseStorageService>(new HierarchicalLifetimeManager())
                .RegisterType<IDeploymentService, DeploymentService>(new HierarchicalLifetimeManager())
                .RegisterType<IFeatureService, FeatureService>(new HierarchicalLifetimeManager())
                
                
                .RegisterType<IApplicationEventHandler, FeatureSynchronizationHandler>(
                    "FeatureSynchronizationHandler", 
                    new HierarchicalLifetimeManager());
        }
    }
}

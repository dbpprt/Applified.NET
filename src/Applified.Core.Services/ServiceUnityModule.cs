using System.Collections.Generic;
using Applified.Common.Unity;
using Applified.Core.DataAccess;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.Extensibility.Contracts;
using Applified.Core.Services.Contracts;
using Applified.Core.Services.Repositories;
using Microsoft.Practices.Unity;

namespace Applified.Core.Services
{
    public class ServiceUnityModule : UnityModule
    {
        public override void RegisterDependencies(IUnityContainer container)
        {
            container
                .RegisterType<IDbContext, EntityContext>(new ExternallyControlledLifetimeManager())
                .RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager())

                .RegisterType<IRepository<Application>, Repository<Application>>(new HierarchicalLifetimeManager())
                .RegisterType<IRepository<WellKnownApplication>, Repository<WellKnownApplication>>(new HierarchicalLifetimeManager())
                .RegisterType<IRepository<Feature>, Repository<Feature>>(new HierarchicalLifetimeManager())
                .RegisterType<IRepository<GlobalFeatureSetting>, Repository<GlobalFeatureSetting>>(new HierarchicalLifetimeManager())

                .RegisterType(typeof(IRepository<>), typeof(ApplicationDependantRepository<>), new HierarchicalLifetimeManager())
                .RegisterType(typeof(INativeRepository<>), typeof(NativeRepository<>), new HierarchicalLifetimeManager())

                //.RegisterType<IRepository<Post>, PostRepository>(new HierarchicalLifetimeManager())

                .RegisterType<IEnumerable<IPostApplicationCreationStep>, IPostApplicationCreationStep[]>();
        }
    }
}

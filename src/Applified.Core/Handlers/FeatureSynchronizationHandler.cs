using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.Extensibility.Contracts;
using Applified.Core.ServiceContracts;
using Microsoft.Practices.Unity;

namespace Applified.Core.Handlers
{
    class FeatureSynchronizationHandler : IApplicationEventHandler
    {
        public async Task OnStartup(IUnityContainer container, IDependencyScope scope)
        {
            var featureService = scope.Resolve<IFeatureService>();
            await featureService.SynchronizeIntegratedFeaturesWithDatabaseAsync(
                AppDomain.CurrentDomain.BaseDirectory
                ).ConfigureAwait(false);
        }

        public void OnShutdown() { }
    }
}

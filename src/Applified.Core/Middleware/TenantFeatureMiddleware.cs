using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.ServiceContracts;
using Microsoft.Owin;
using Owin;

namespace Applified.Core.Middleware
{
    public class TenantFeatureMiddleware : OwinMiddleware
    {
        private readonly IAppBuilder _appBuilder;
        private static ConcurrentDictionary<Guid, OwinMiddleware> _tenantOwinPipe;
        private static SemaphoreSlim _tenantPipeLock;
        private static OwinMiddleware _coreMiddlewares;

        public TenantFeatureMiddleware(
            OwinMiddleware next,
            IAppBuilder appBuilder)
            : base(next)
        {
            _appBuilder = appBuilder;
            _tenantOwinPipe = new ConcurrentDictionary<Guid, OwinMiddleware>();
            _tenantPipeLock = new SemaphoreSlim(1);
        }

        public override async Task Invoke(IOwinContext context)
        {
            var scope = context.GetRequestContainer();
            var current = scope.Resolve<ICurrentContext>();

            OwinMiddleware pipe;
            if (_tenantOwinPipe.TryGetValue(current.ApplicationId, out pipe))
            {
                await pipe.Invoke(context);
                return;
            }

            var featureService = scope.Resolve<IFeatureService>();
            var registeredFeatures = await featureService.GetFeatureInstancesAsync();

            registeredFeatures = registeredFeatures
                .OrderByDescending(feature => feature.ExecutionOrderKey)
                .ToList();

            var count = registeredFeatures.Count;
            var lastMiddleware = Next;

            for (var i = count - 1; i >= 0; i--)
            {
                var provider = registeredFeatures[i];

                var currentMiddleware = await provider.UseAsync(
                    current.ApplicationId,
                    lastMiddleware,
                    _appBuilder,
                    scope
                    );

                if (currentMiddleware != null || lastMiddleware == null)
                {
                    lastMiddleware = currentMiddleware;
                }
            }

            if (lastMiddleware != null)
            {
                await SetPipe(current.ApplicationId, lastMiddleware);
                await lastMiddleware.Invoke(context);
            }
            else
            {
                await SetPipe(current.ApplicationId, Next);
                await Next.Invoke(context);
            }
        }

        private static async Task SetPipe(Guid application, OwinMiddleware middleware)
        {
            await _tenantPipeLock.WaitAsync();

            try
            {
                if (_tenantOwinPipe == null)
                {
                    _tenantOwinPipe = new ConcurrentDictionary<Guid, OwinMiddleware>();
                }

                _tenantOwinPipe.TryAdd(application, middleware);
            }
            finally
            {
                _tenantPipeLock.Release();
            }
        }
    }
}
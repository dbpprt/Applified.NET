using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Applified.Common;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.Extensibility.Contracts;
using Applified.Core.ServiceContracts;
using Microsoft.Owin;
using Microsoft.Practices.Unity;

namespace Applified.Core.Handlers
{
    /// <summary>
    /// This class represents the CurrentContext in a tenant aware manner.
    /// All code gets executed per request => performance critical
    /// TODO: It should contain some better loading logic without loading all tenants
    /// TODO: some applicationinitialization mechanism
    /// </summary>
    [UsedImplicitly]
    public class MultiTenancyContextHandler :
        AbstractNotificationSubscriber,
        IDynamicRegistrationDelegate,
        ICurrentContext,
        IDisposable,
        IUnprotectedContext
    {
        private string _host;
        private string _accessToken;

        private IServiceProvider _provider;

        private static ConcurrentDictionary<string, Application> _applicationBindings = null;
        private static readonly SemaphoreSlim ApplicationBindingsLock = new SemaphoreSlim(1);

        private IServerEnvironment _serverEnvironment;

        public bool IsAdmin { get; set; }

        public Guid ApplicationId { get; set; }

        public Guid? DeploymentId { get; set; }

        public string BaseDirectory { get; set; }

        /// <summary>
        /// This method invalidates the caches when a new deployment "arrives"
        /// TODO: we should only delete the affected application because its not required to rebuild all caches
        /// </summary>
        /// <param name="deploymentId"></param>
        /// <returns></returns>
        public override async Task OnNewDeployment(Guid deploymentId)
        {
            await ApplicationBindingsLock.WaitAsync();

            try
            {
                _applicationBindings = null;

            }
            finally
            {
                ApplicationBindingsLock.Release();
            }
        }

        public async Task InterceptRequestScope(IUnityServiceProvider provider, IOwinContext context)
        {
            _provider = provider as IServiceProvider;

            var scope = provider.GetUnderlayingContainer();

            _serverEnvironment = scope.Resolve<IServerEnvironment>();

            // we are able to register (override) dependecies per request
            // so we are building the request context ondemand and per request

            scope.RegisterInstance<INotificationSubscriber>("MultiTenancyContextHandler", this);

            scope.RegisterInstance<ICurrentContext>(this, new HierarchicalLifetimeManager());

            scope.RegisterInstance<IUnprotectedContext>(this, new HierarchicalLifetimeManager());

            await Initialize(scope);

            _host = context.Request.Host.Value;

            Application currentApplication;
            if (_applicationBindings.TryGetValue(_host, out currentApplication))
            {
                // no need for locking here.. its called per request...
                DeploymentId = currentApplication.ActiveDeploymentId;
                ApplicationId = currentApplication.Id;

                if (DeploymentId.HasValue)
                {
                    BaseDirectory = _serverEnvironment.GetDeploymentDirectory(DeploymentId.Value);
                }
            }


            await AdminAuthorize(context);
        }

        /// <summary>
        /// Checks for an access token in header params and tries to authorize the user.
        /// This implementation needs some refactoring.. And some better code structure
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task AdminAuthorize(IOwinContext context)
        {
            string[] tokens;
            _accessToken = context.Request.Headers.TryGetValue("AccessToken", out tokens)
                ? tokens.FirstOrDefault()
                : null;

            if (!string.IsNullOrEmpty(_accessToken))
            {
                var applications = _provider.Resolve<IRepository<Application>>();
                var desiredApplication = await applications.Query()
                    .FirstOrDefaultAsync(application => application.AccessToken == _accessToken);

                if (desiredApplication == null)
                {
                    throw new UnauthorizedAccessException();
                }

                IsAdmin = true;

                // this tricks the service/dataaccess architecture to allow admins to 
                // edit their applications without accessing a valid binding
                ApplicationId = desiredApplication.Id;

                // not quire sure wether this is required :/
                DeploymentId = desiredApplication.ActiveDeploymentId;
            }
        }

        /// <summary>
        /// This initializes the application bindings cache to avoid database queries on each request
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        private static async Task Initialize(IUnityContainer scope)
        {
            if (_applicationBindings == null)
            {
                await ApplicationBindingsLock.WaitAsync();

                try
                {
                    if (_applicationBindings == null)
                    {
                        var applications = scope.Resolve<IRepository<Application>>();

                        _applicationBindings = new ConcurrentDictionary<string, Application>();

                        var results = await applications.Query()
                            .Include(entity => entity.Bindings)
                            .ToListAsync();

                        var bindings = results
                            .SelectMany(result => result.Bindings)
                            .Select(
                                binding =>
                                {
                                    binding.Application =
                                        results.First(entity => entity.Id == binding.ApplicationId);
                                    return binding;
                                })
                            .ToList();

                        foreach (var binding in bindings)
                        {
                            _applicationBindings.TryAdd(binding.Hostname, binding.Application);
                        }
                    }
                }
                finally
                {
                    ApplicationBindingsLock.Release();
                }
            }
        }

        public void Dispose()
        {

        }
    }
}

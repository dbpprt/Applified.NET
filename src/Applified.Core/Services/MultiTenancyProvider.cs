using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Linq;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.Extensibility.Contracts;
using Applified.Core.ServiceContracts;
using Microsoft.Owin;
using Microsoft.Practices.Unity;

namespace Applified.Core.Services
{
    public class MultiTenancyProvider : AbstractNotificationSubscriber, IDynamicRegistrationDelegate, ICurrentApplication, IDisposable, IUnprotectedContext
    {
        private string _host;
        private string _accessToken;

        private IServiceProvider _provider;

        private static ConcurrentDictionary<string, Application> _applicationBindings = null;
        private static object _applicationBindingsLock = new object();

        public bool IsAdmin { get; private set; }
        private Application _application;
        private Guid? _deploymentToServe;

        public MultiTenancyProvider()
        {
            _application = null;
            IsAdmin = false;
        }

        public override void OnNewDeployment(Guid deploymentId)
        {
            lock (_applicationBindingsLock)
            {
                _applicationBindings = new ConcurrentDictionary<string, Application>();
                _deploymentToServe = null;
                _application = null;
            }

            base.OnNewDeployment(deploymentId);
        }

        public void InterceptRequestScope(IUnityServiceProvider provider, IOwinContext context)
        {
            _provider = provider as IServiceProvider;

            var scope = provider.GetUnderlayingContainer();

            scope.RegisterInstance<INotificationSubscriber>("MultiTenancyProvider", this);

            scope.RegisterInstance<ICurrentApplication>(this, new HierarchicalLifetimeManager());

            scope.RegisterInstance<IUnprotectedContext>(this, new HierarchicalLifetimeManager());

            if (_applicationBindings == null)
            {
                var applications = scope.Resolve<IRepository<Application>>();

                lock (_applicationBindingsLock)
                {
                    _applicationBindings = new ConcurrentDictionary<string, Application>();

                    var results = applications.Query()
                        .Include(entity => entity.Bindings)
                        .ToList();

                    var bindings = results
                        .SelectMany(result => result.Bindings)
                        .Select(
                            binding =>
                            {
                                binding.Application =
                                    results.First(application => application.Id == binding.ApplicationId);
                                return binding;
                            })
                        .ToList();

                    foreach (var binding in bindings)
                    {
                        _applicationBindings.TryAdd(binding.Hostname, binding.Application);
                    }
                }
            }

            _host = context.Request.Host.Value;

            string[] tokens;
            _accessToken = context.Request.Headers.TryGetValue("AccessToken", out tokens) 
                ? tokens.FirstOrDefault() 
                : null;

            if (!string.IsNullOrEmpty(_accessToken))
            {
                var applications = _provider.Resolve<IRepository<Application>>();
                var desiredApplication = applications.Query()
                    .FirstOrDefault(application => application.AccessToken == _accessToken);

                if (desiredApplication == null)
                {
                    throw new UnauthorizedAccessException();
                }

                IsAdmin = true;
                _application = desiredApplication;
            }
        }

        public Application Application
        {
            get
            {
                if (_application != null)
                    return _application;

                lock (_applicationBindingsLock)
                {
                    Application application;
                    if (_applicationBindings.TryGetValue(_host, out application))
                    {
                        return application;
                    }

                    throw new InvalidOperationException("You're trying to do something bad!");
                }
            }
        }

        public Guid? DeploymentToServe {
            get
            {
                if (_deploymentToServe != null)
                    return _deploymentToServe;

                if (_application != null)
                    return _application.ActiveDeploymentId;

                return Application.ActiveDeploymentId;
            }
        }

        public void Dispose()
        {

        }

        public void SetDeploymentToServe(Guid? guid)
        {
            _deploymentToServe = guid;
        }

        public void SetIsAdmin(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }

        public void SetCurrentApplication(Application application)
        {
            _application = application;
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.Extensibility.Contracts;
using Applified.Core.ServiceContracts;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.StaticFiles.Infrastructure;
using Microsoft.Practices.Unity;

namespace Applified.Core.Middleware
{
    public class MultiTenantFileServer : AbstractNotificationSubscriber, IDynamicRegistrationDelegate
    {
        private readonly Func<IDictionary<string, object>, Task> _nextFunc;
        private readonly ConcurrentDictionary<Guid, StaticFileMiddleware> _tenantHandlers;
        private readonly object _tenantHandlersLock = new object();

        private readonly string _workingDirectory;

        public MultiTenantFileServer(Func<IDictionary<string, object>, Task> nextFunc, SharedOptions sharedOptions, string workingDirectory)
        {
            _nextFunc = nextFunc;
            _workingDirectory = workingDirectory;

            lock (_tenantHandlersLock)
            {
                _tenantHandlers = new ConcurrentDictionary<Guid, StaticFileMiddleware>();
            }
        }

        public override void OnNewDeployment(Guid deploymentId)
        {
            lock (_tenantHandlersLock)
            {
                StaticFileMiddleware tenantHandler;
                _tenantHandlers.TryRemove(deploymentId, out tenantHandler);
            }

            base.OnNewDeployment(deploymentId);
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);
            var scope = (IServiceProvider)context.Environment.GetRequestContainer();
            var currentApplication = scope.Resolve<ICurrentApplication>();

            var activeDeploymentId = currentApplication.DeploymentToServe;

            if (!activeDeploymentId.HasValue)
            {
                throw new Exception("TODO: Shared error page!");
            }

            var desiredPath = Path.Combine(_workingDirectory, activeDeploymentId.ToString());

            if (!Directory.Exists(desiredPath))
            {
                await DownloadDeployment(scope, _workingDirectory, desiredPath, activeDeploymentId.Value);
            }

            StaticFileMiddleware tenantHandler;

            if (_tenantHandlers.TryGetValue(activeDeploymentId.Value, out tenantHandler))
            {
                await tenantHandler.Invoke(environment);
                return;
            }
            else
            {
                lock (_tenantHandlersLock)
                {
                    tenantHandler = new StaticFileMiddleware(_nextFunc, new StaticFileOptions
                    {
                        ServeUnknownFileTypes = true,
                        FileSystem = new PhysicalFileSystem(desiredPath)
                    });

                    _tenantHandlers.TryAdd(activeDeploymentId.Value, tenantHandler);
                }

                await tenantHandler.Invoke(environment);
                return;
            }

            await _nextFunc(environment);
        }

        private async Task DownloadDeployment(IServiceProvider scope, string workingDirectory, string targetDirectory, Guid deploymentId)
        {
            var deploymentService = scope.Resolve<IDeploymentService>();

            var obj = await deploymentService
                .GetDeploymentPayloadInternalAsync(deploymentId)
                .ConfigureAwait(false);

            var workingName = Guid.NewGuid().ToString();
            var workingPath = Path.Combine(workingDirectory, workingName);

            File.WriteAllBytes(workingPath, obj.Data);
            ZipFile.ExtractToDirectory(workingPath, targetDirectory);
        }

        public void InterceptRequestScope(IUnityServiceProvider provider, IOwinContext context)
        {
            var container = provider.GetUnderlayingContainer();

            if (container != null)
            {
                container.RegisterInstance<INotificationSubscriber>("MultiTenantFileServer", this);
            }
        }
    }
}
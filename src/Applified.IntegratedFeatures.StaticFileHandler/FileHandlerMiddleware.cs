using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.Extensibility.Contracts;
using Applified.Core.ServiceContracts;
using Applified.IntegratedFeatures.StaticFileHandler.Wrappers;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Practices.Unity;
using Owin;

namespace Applified.IntegratedFeatures.StaticFileHandler
{
    class FileHandlerMiddleware : OwinMiddleware //, INotificationSubscriber, IDynamicRegistrationDelegate
    {
        private readonly IDependencyScope _scope;
        private readonly Settings _settings;
        private OwinMiddleware _underlayingMiddleware;

        // TODO: this instance needs to be rebuild when a new deployment arrives!

        public FileHandlerMiddleware(
            OwinMiddleware next,
            IDependencyScope scope,
            Settings settings
            )
            : base(next)
        {
            _scope = scope;
            _settings = settings;

            InitializeUnderlayingMiddleware();
        }

        private void InitializeUnderlayingMiddleware()
        {
            var context = _scope.Resolve<ICurrentContext>();
            var requestPath = new PathString(_settings.GetValue<string>(Settings.RequestPath));
            var fileSystem = new PhysicalFileSystem(context.BaseDirectory);

            OwinMiddleware middleware = new StaticFileMiddlewareWrapper(Next, new StaticFileOptions
            {
                RequestPath = requestPath,
                ServeUnknownFileTypes = _settings.GetValue<bool>(Settings.ServeUnknownFileTypes),
                FileSystem = fileSystem
            });
            middleware = new SendFileMiddlewareWrapper(middleware);

            if (_settings.GetValue<bool>(Settings.EnableDirectoryBrowsing))
            {
                middleware = new DirectoryBrowserMiddlewareWrapper(middleware, new DirectoryBrowserOptions
                {
                    RequestPath = requestPath,
                    FileSystem = fileSystem
                });
            }

            if (_settings.GetValue<bool>(Settings.EnableDefaultFiles))
            {
                middleware = new DefaultFilesMiddlewareWrapper(middleware, new DefaultFilesOptions
                {
                    DefaultFileNames = _settings.GetValue<string>(Settings.DefaultFiles).Split(';'),
                    RequestPath = requestPath
                });
            }

            _underlayingMiddleware = middleware;
        }

        public override Task Invoke(IOwinContext context)
        {
            return _underlayingMiddleware.Invoke(context);
        }

        // TODO: the tenantfeature handler should recreate the middlewares on changes! 

        //public Task OnNewDeployment(Guid deploymentId)
        //{
        //    InitializeUnderlayingMiddleware();


        //}

        ///// <summary>
        ///// TODO: is it necessary to register the notificationsubscriber on each request?
        ///// could be really time expensive with huge amount of tenants
        ///// </summary>
        ///// <param name="provider"></param>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public Task InterceptRequestScope(IUnityServiceProvider provider, IOwinContext context)
        //{
        //    provider.GetUnderlayingContainer().RegisterInstance<INotificationSubscriber>(this);

        //    return Task.FromResult(0);
        //}
    }
}

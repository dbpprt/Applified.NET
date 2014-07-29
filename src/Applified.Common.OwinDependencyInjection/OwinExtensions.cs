using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Owin;

namespace Applified.Common.OwinDependencyInjection
{
    public static class OwinExtensions
    {
        public static IAppBuilder UseContainer(this IAppBuilder app, IDependencyResolver appContainer)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            if (appContainer == null)
            {
                throw new ArgumentNullException("appContainer");
            }

            SetApplicationContainer(app, appContainer);

            return app.Use(new Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>(nextApp => new ContainerMiddleware(nextApp, app).Invoke));
        }

        public static HttpServer PrepareWebapiAdapter(this IAppBuilder app, HttpConfiguration configuration)
        {
            var appContainer = GetApplicationContainer(app);
            configuration.DependencyResolver = appContainer;
            var httpServer = new OwinDependencyScopeHttpServerAdapter(configuration);
            return httpServer;
        }

        public static IAppBuilder SetApplicationContainer(this IAppBuilder app, IDependencyResolver container)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            app.Properties[Constants.OwinApplicationContainerKey] = container;

            return app;
        }

        public static IDependencyResolver GetApplicationContainer(this IAppBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            return app.Properties[Constants.OwinApplicationContainerKey] as IDependencyResolver;
        }
    }
}
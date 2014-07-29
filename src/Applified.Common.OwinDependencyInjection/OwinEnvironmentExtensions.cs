using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Http.Dependencies;
using Microsoft.Owin;
using Owin;

namespace Applified.Common.OwinDependencyInjection
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class OwinEnvironmentExtensions
    {
        public static IUnityServiceProvider SetRequestContainer(this IDictionary<string, object> environment, IAppBuilder app)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            if (app == null)
            {
                throw new ArgumentNullException("app");
            }

            var appContainer = OwinExtensions.GetApplicationContainer(app);
            if (appContainer == null)
            {
                throw new InvalidOperationException("There is no application container registered to resolve a request container");
            }

            var requestContainer = appContainer.BeginScope();

            environment[Constants.OwinRequestContainerEnvironmentKey] = requestContainer;

            return requestContainer as IUnityServiceProvider;
        }

        public static IDependencyScope GetRequestContainer(
            this IDictionary<string, object> environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            var unityScope =
                environment[Constants.OwinRequestContainerEnvironmentKey] as IDependencyScope;

            return unityScope;
        }

        public static IDependencyScope GetRequestContainer(
            this IOwinContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var environment = context.Environment;

            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            var unityScope =
                environment[Constants.OwinRequestContainerEnvironmentKey] as IDependencyScope;

            return unityScope;
        }
    }
}

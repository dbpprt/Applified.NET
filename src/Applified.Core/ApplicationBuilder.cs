using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Http;
using Applified.Common.OwinDependencyInjection;
using Applified.Common.Unity;
using Applified.Core.Identity;
using Applified.Core.Middleware;
using Applified.Core.ServiceContracts;
using Microsoft.Owin.Diagnostics;
using Microsoft.Owin.Extensions;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Applified.Core
{
    public static class ApplicationBuilder
    {
        public static void Build(IAppBuilder app)
        {
            app.UseStageMarker(PipelineStage.MapHandler);

            app.UseErrorPage(new ErrorPageOptions
            {
                ShowCookies = true,
                ShowEnvironment = true,
                ShowExceptionDetails = true,
                ShowHeaders = true,
                ShowQuery = true,
                ShowSourceCode = true,
                SourceCodeLineCount = 20
            });

            var container = new UnityContainer()
                .RegisterModule<MainUnityModule>();

            app.UseContainer(new UnityDependencyResolver(container));

            app.Use<ApplicationEventMiddleware>(container);

            app.Use<ApplicationDeploymentProviderMiddleware>();

            app.Use<TenantFeatureMiddleware>(app);

            app.Use<DeploymentMiddleware>();

            //app.Use<ManagementMiddleware>();

            //app.Use<MetaWeblogService>();

            //app.Use<SimpleUrlRoutingMiddleware>(StaticRouteConfiguration());

            //app.UseWebApi(
            //    app.PrepareWebapiAdapter(ApiHttpConfiguration())
            //    );

            //app.Use<MultiTenantFileServerMiddleware>(null, "C:\\Deployments");
        }


        private static HttpConfiguration ApiHttpConfiguration()
        {
            var config = new HttpConfiguration();
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            return config;
        }

        private static HttpRouteCollection StaticRouteConfiguration()
        {
            var config = new HttpRouteCollection();

            config.IgnoreRoute("Webapi", "api/{*data}");

            config.MapHttpRoute("Windows live writer manifest",
                "wlwmanifest.xml",
                new { rewrite = "/assets/wlwmanifest.xml" }
            );

            config.MapHttpRoute("Angular HTML5 Navigation",
                "{*data}",
                new { rewrite = "/index.html" },
                new { data = @".*?$(?<!\.js|.css|.eot)" }
            );

            return config;
        }
    }
}
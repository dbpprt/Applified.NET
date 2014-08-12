#region Copyright (C) 2014 Applified.NET 
// Copyright (C) 2014 Applified.NET
// http://www.applified.net

// This file is part of Applified.NET.

// Applified.NET is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.

// You should have received a copy of the GNU Affero General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Web.Http;
using Applified.Common;
using Applified.Common.Logging;
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

            var container = new UnityContainer()
                .RegisterModule<MainUnityModule>();

            app.UseContainer(new UnityDependencyResolver(container));

            app.Use<ApplicationEventMiddleware>(app, container);

            app.Use<DeploymentMiddleware>();

            app.Use<ApplicationDeploymentProviderMiddleware>();

            app.Use<TenantFeatureMiddleware>(app);

            //app.Use<ManagementMiddleware>();

            //app.Use<MetaWeblogService>();

            //app.UseWebApi(
            //    app.PrepareWebapiAdapter(ApiHttpConfiguration())
            //    );

            container.Resolve<ILog>()
                .Write("Server started successfully and is ready to receive requests!")
                .IsVerbose()
                .Save();
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

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
using System.IO;
using System.Web.Http;
using Applified.Common.Logging;
using Applified.Common.OwinDependencyInjection;
using Applified.Common.Unity;
using Applified.Core.Middleware;
using Applified.Core.ServiceContracts;
using Microsoft.Owin.Extensions;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Applified.Core
{
    public static class ApplicationBuilder
    {
        public const string ConfigurationFileName = "server.json";

        public static void Build(IAppBuilder app)
        {
            app.UseStageMarker(PipelineStage.MapHandler);

            var container = new UnityContainer()
                .RegisterModule<MainUnityModule>();

            InitializeEnvironment(container);

            app.UseContainer(new UnityDependencyResolver(container));

            app.Use<ApplicationEventMiddleware>(app, container);

            InitializeFarmFeatures(app, container);



            app.Use<DeploymentMiddleware>();

            app.Use<ApplicationDeploymentProviderMiddleware>();

            app.Use<TenantFeatureMiddleware>(app);

            

            container.Resolve<ILog>()
                .Write("Server started successfully and is ready to receive requests!")
                .IsVerbose()
                .Save();
        }

        public static void InitializeFarmFeatures(IAppBuilder app, IUnityContainer container)
        {
            using (var scope = container.CreateChildContainer())
            {
                var featureService = scope.Resolve<IFeatureService>();

                var farmFeatures = featureService.GetActivatedFarmFeatures();

                foreach (var farmFeature in farmFeatures)
                {
                    var instance = featureService.InstantiateFeature(farmFeature.Id);

                    if (instance != null)
                    {
                        instance.RegisterDependencies(container);
                        instance.Build(app);
                    }
                }
            }
        }

        public static void InitializeEnvironment(IUnityContainer container)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationFileName);
            var settings = new Settings(filePath);

            if (!File.Exists(filePath))
            {
                try
                {
                    settings.Save();

                    container.Resolve<ILog>()
                        .Write("No server configuration file found at " + filePath + ". Using default settings!")
                        .IsError()
                        .Save();
                }
                catch (Exception exception)
                {
                    exception
                        .ToEvent()
                        .SetMessage(
                            "Unable to find a server configuration and unable to create a default server configuration!")
                        .IsCritical()
                        .Save(container.Resolve<ILog>());
                }
            }

            var serverEnvironment = new ServerEnvironment(settings);
            container.RegisterInstance<IServerEnvironment>(serverEnvironment);
        }
    }
}

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
using System.Diagnostics;
using Applified.Common.OwinDependencyInjection;
using Applified.Common.Unity;
using Applified.Core.Extensibility.Contracts;
using Applified.Core.Handlers;
using Applified.Core.ServiceContracts;
using Applified.Core.Services;
using Applified.Core.Services.Services;
using Microsoft.Practices.Unity;

namespace Applified.Core
{
    public class MainUnityModule : UnityModule
    {
        public override void RegisterDependencies(IUnityContainer container)
        {
            container
                .RegisterModule<ServiceUnityModule>()

                .RegisterType<ICurrentContext, MultiTenancyContextHandler>(new HierarchicalLifetimeManager())
                .RegisterType<IDynamicRegistrationDelegate, MultiTenancyContextHandler>("MultiTenancyContextHandler",
                    new HierarchicalLifetimeManager())

                .RegisterType<IUrlBuilderService, UrlBuilderService>(new HierarchicalLifetimeManager())
                .RegisterType<IStorageService, DatabaseStorageService>(new HierarchicalLifetimeManager())
                .RegisterType<IDeploymentService, DeploymentService>(new HierarchicalLifetimeManager())
                .RegisterType<IFeatureService, FeatureService>(new HierarchicalLifetimeManager())
                .RegisterType<IApplicationService, ApplicationService>(new HierarchicalLifetimeManager())
                .RegisterType<ISetupService, SetupService>(new HierarchicalLifetimeManager())

                .RegisterType<IServerEnvironment, DevelopmentServerEnvironment>(new HierarchicalLifetimeManager())

                .RegisterNamed<IApplicationEventHandler, RegisterGlobalFeatureDependenciesHandler>(
                    new HierarchicalLifetimeManager())
                .RegisterNamed<IApplicationEventHandler, FeatureSynchronizationHandler>(
                    new HierarchicalLifetimeManager());


        }
    }
}

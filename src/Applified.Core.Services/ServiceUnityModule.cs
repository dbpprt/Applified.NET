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

using System.Collections.Generic;
using Applified.Common.Unity;
using Applified.Core.DataAccess;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.Extensibility.Contracts;
using Applified.Core.Services.Contracts;
using Applified.Core.Services.Repositories;
using Microsoft.Practices.Unity;

namespace Applified.Core.Services
{
    public class ServiceUnityModule : UnityModule
    {
        public override void RegisterDependencies(IUnityContainer container)
        {
            container
                .RegisterType<IDbContext, EntityContext>(new HierarchicalLifetimeManager())
                .RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager())

                .RegisterType<IRepository<Application>, ApplicationRepository>(new HierarchicalLifetimeManager())
                .RegisterType<IRepository<WellKnownApplication>, Repository<WellKnownApplication>>(new HierarchicalLifetimeManager())
                .RegisterType<IRepository<Feature>, Repository<Feature>>(new HierarchicalLifetimeManager())
                .RegisterType<IRepository<GlobalFeatureSetting>, Repository<GlobalFeatureSetting>>(new HierarchicalLifetimeManager())

                .RegisterType(typeof(IRepository<>), typeof(ApplicationDependantRepository<>), new HierarchicalLifetimeManager())
                .RegisterType(typeof(INativeRepository<>), typeof(NativeRepository<>), new HierarchicalLifetimeManager())

                //.RegisterType<IRepository<Post>, PostRepository>(new HierarchicalLifetimeManager())

                .RegisterType<IEnumerable<IPostApplicationCreationStep>, IPostApplicationCreationStep[]>();
        }
    }
}

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
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Applified.Common;
using Applified.Common.Configuration;
using Applified.Common.Unity;
using Applified.Core.Entities.Infrastructure;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Owin;

namespace Applified.Core.Extensibility
{

    /// <summary>
    /// Base class for features
    /// TODO: This class is far from being perfect... A lot of refactoring is required to make this more usable
    /// </summary>
    public abstract class FeatureBase : IUnityModule
    {
        public abstract SettingsBase GetSettings(Dictionary<string, string> dictionary);

        public abstract Guid FeatureId { get; }

        public abstract int ExecutionOrderKey { get; }

        public abstract string AssemblyName { get; }

        public virtual FeatureScope Scope
        {
            get
            {
                return FeatureScope.Application;
            }
        }

        public virtual void RegisterDependencies(IUnityContainer container) { }

        public virtual void Build(IAppBuilder app) { }

        public virtual void OnStartup(IAppBuilder app) { }

        public virtual void OnShutdown() { }

        public virtual List<AvaliableSetting> GetAvaliableSettings()
        {
            return GetSettings(null).GetAvaliableSettings();
        }

        public abstract Task<OwinMiddleware> UseAsync(Guid applicationId, OwinMiddleware next, IAppBuilder appBuilder, IDependencyScope scope);

        public virtual DbMigrationsConfiguration GetMigrationsConfiguration()
        {
            return null;
        } 
    }
}

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
using System.ComponentModel.Composition;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Applified.Common;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.Extensibility;
using Applified.Core.ServiceContracts;
using Microsoft.Owin;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace Applified.IntegratedFeatures.StaticFileHandler
{
    [Export(typeof(IntegratedFeatureBase))]
    public class StaticFileHandlerFeature : IntegratedFeatureBase
    {
        public override SettingsBase GetSettings(Dictionary<string, string> dictionary)
        {
            return new Settings(dictionary);
        }

        public override Guid FeatureId
        {
            get { return new Guid("31EF6DF6-978C-4001-9E93-56C183772AF7"); }
        }

        public override int ExecutionOrderKey
        {
            // execute as last middleware because it sends 404 if it dont find a file
            get { return 0; }
        }

        public override async Task<OwinMiddleware> UseAsync(
            Guid applicationId, 
            OwinMiddleware next, 
            IAppBuilder builder,
            IDependencyScope scope)
        {
            var featureService = scope.Resolve<IFeatureService>();
            var settings = await featureService.GetSettingsAsync(FeatureId);

            return new FileHandlerMiddleware(next, scope, new Settings(settings));
        }

        public override string Name
        {
            get { return "static-file-handler"; }
        }

        public override string Description
        {
            get { return "Provider for static files like images, html, js, etc files"; }
        }

        public override string Version
        {
            get { return "0.0.1-alpha-1"; }
        }

        public override string Author
        {
            get { return "Dennis Bappert"; }
        }

        public override string AssemblyName
        {
            get { return Assembly.GetExecutingAssembly().FullName; }
        }
    }
}

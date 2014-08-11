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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Applified.Common;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.Extensibility;
using Applified.Core.ServiceContracts;
using Microsoft.Owin;
using Owin;

namespace Applified.IntegratedFeatures.AngularHtml5NavigationRewrite
{
    [Export(typeof(IntegratedFeatureBase))]
    public class AngularHtml5NavigationRewriteFeature : IntegratedFeatureBase
    {
        public override SettingsBase GetSettings(Dictionary<string, string> dictionary)
        {
            return new Settings(dictionary);
        }

        public override Guid FeatureId
        {
            get { return new Guid("31EF6DF6-978C-4001-9E15-56C183772AF7"); }
        }

        public override int ExecutionOrderKey
        {
            get { return 5000; }
        }

        public override async Task<OwinMiddleware> UseAsync(
            Guid applicationId,
            OwinMiddleware next,
            IAppBuilder appBuilder,
            IDependencyScope scope)
        {
            var routes = new HttpRouteCollection();

            var featureService = scope.Resolve<IFeatureService>();
            var dictionary = await featureService.GetSettingsAsync(FeatureId);
            var settings = new Settings(dictionary);

           routes.MapHttpRoute(
                settings.GetValue<string>(Settings.RouteName),
                settings.GetValue<string>(Settings.MatchRoute),
                new { rewrite = settings.GetValue<string>(Settings.RewriteTo) },
                new { data = settings.GetValue<string>(Settings.Constraint) }
            );

            return new UrlRewriteMiddleware(next, routes);
        }

        public override string Name
        {
            get { return "angular-html5-navigation-rewrite"; }
        }

        public override string Description
        {
            get { return "This is a simple module which rewrites all request to index.html except for static resources"; }
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

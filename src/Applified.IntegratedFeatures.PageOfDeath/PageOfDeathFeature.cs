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
using System.Web.Http.Dependencies;
using Applified.Common;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.Extensibility;
using Applified.Core.ServiceContracts;
using Applified.IntegratedFeatures.PageOfDeath.Wrappers;
using Microsoft.Owin;
using Microsoft.Owin.Diagnostics;
using Owin;

namespace Applified.IntegratedFeatures.PageOfDeath
{
    [Export(typeof(IntegratedFeatureBase))]
    public class PageOfDeathFeature : IntegratedFeatureBase
    {
        public override SettingsBase GetSettings(Dictionary<string, string> dictionary)
        {
            return new Settings(dictionary);
        }

        public override Guid FeatureId
        {
            get { return new Guid("12EF8DF6-978C-4001-9E93-56C183776BF7"); }
        }

        public override int ExecutionOrderKey
        {
            get { return int.MaxValue; }
        }

        public override async Task<OwinMiddleware> UseAsync(
            Guid applicationId,
            OwinMiddleware next, 
            IAppBuilder appBuilder, 
            IDependencyScope scope)
        {
            var featureService = scope.Resolve<IFeatureService>();
            var dictionary = await featureService.GetSettingsAsync(FeatureId);
            var settings = new Settings(dictionary);

            var middleware = new ErrorPageMiddlewareWrapper(
                next,
                new ErrorPageOptions
                {
                    ShowCookies = settings.GetValue<bool>(Settings.ShowCookies),
                    ShowEnvironment = settings.GetValue<bool>(Settings.ShowEnvironment),
                    ShowExceptionDetails = settings.GetValue<bool>(Settings.ShowExceptionDetails),
                    ShowHeaders = settings.GetValue<bool>(Settings.ShowHeaders),
                    ShowQuery = settings.GetValue<bool>(Settings.ShowQuery),
                    ShowSourceCode = settings.GetValue<bool>(Settings.ShowSourceCode),
                    SourceCodeLineCount = settings.GetValue<int>(Settings.SourceCodeLineCount)
                });

            return middleware;
        }

        public override string Name
        {
            get { return "page-of-death"; }
        }

        public override string Description
        {
            get
            {
                return
                    "This feature enables the ErrorMiddleware shipped with Microsoft.Owin.Diagnostics. Good for debugging purposes!";
            }
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

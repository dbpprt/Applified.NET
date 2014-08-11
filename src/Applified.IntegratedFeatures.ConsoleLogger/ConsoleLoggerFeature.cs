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
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Applified.Common;
using Applified.Core.Extensibility;
using Microsoft.Owin;
using Owin;

namespace Applified.IntegratedFeatures.ConsoleLogger
{

    [Export(typeof(IntegratedFeatureBase))]
    public class ConsoleLoggerFeature : IntegratedFeatureBase
    {
        public override SettingsBase GetSettings(Dictionary<string, string> dictionary)
        {
            return null;
        }

        public override Guid FeatureId
        {
            get { return new Guid("31EF6DF6-978C-4001-9E93-56C183776BF7"); }
        }

        public override int ExecutionOrderKey
        {
            // TODO: we need something better here to specify the features position in the pipe
            get { return int.MaxValue - 100; }
        }

        public override Task<OwinMiddleware> UseAsync(Guid applicationId, OwinMiddleware next, IAppBuilder appBuilder, IDependencyScope scope)
        {
            return Task.FromResult(new ConsoleLoggerMiddleware(next) as OwinMiddleware);
        }

        public override string Name
        {
            get { return "console-request-logger"; }
        }

        public override string Description
        {
            get
            {
                return
                    "Logs all request to the standard console output stream. Good for SelfHost console application and debugging purposes";
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

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

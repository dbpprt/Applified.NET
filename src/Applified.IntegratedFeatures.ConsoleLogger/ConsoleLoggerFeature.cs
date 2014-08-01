using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using Applified.Core.Extensibility;
using Microsoft.Owin;
using Owin;

namespace Applified.IntegratedFeatures.ConsoleLogger
{

    [Export(typeof(IntegratedFeatureBase))]
    public class ConsoleLoggerFeature : IntegratedFeatureBase
    {
        public override Guid FeatureId
        {
            get { return new Guid("31EF6DF6-978C-4001-9E93-56C183776BF7"); }
        }

        public override OwinMiddleware GetTenantMiddleware(Guid applicationId, OwinMiddleware next, IAppBuilder appBuilder)
        {
            return new ConsoleLoggerMiddleware(next);
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
    }
}

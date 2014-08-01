using System;
using System.ComponentModel.Composition;
using Applified.Core.Extensibility;
using Microsoft.Owin;
using Owin;

namespace Applified.IntegratedFeatures.StaticFileHandler
{
    [Export(typeof(IntegratedFeatureBase))]
    public class StaticFileHandlerFeature : IntegratedFeatureBase
    {
        public override Guid FeatureId
        {
            get { return new Guid("31EF6DF6-978C-4001-9E93-56C183772AF7"); }
        }

        public override int ExecutionOrderKey
        {
            get { return 1000; }
        }

        public override OwinMiddleware GetTenantMiddleware(Guid applicationId, OwinMiddleware next, IAppBuilder appBuilder)
        {
            return null;
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
    }
}

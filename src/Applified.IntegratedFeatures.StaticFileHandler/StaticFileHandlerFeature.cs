using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
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
    }
}

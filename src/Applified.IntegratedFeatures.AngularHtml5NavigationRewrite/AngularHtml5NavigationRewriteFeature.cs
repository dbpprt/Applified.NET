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

using System.Collections.Generic;
using Applified.Common;

namespace Applified.IntegratedFeatures.AngularHtml5NavigationRewrite
{
    class Settings : SettingsBase
    {
        public const string MatchRoute = "MatchRoute";
        public const string RewriteTo = "RewriteTo";
        public const string Constraint = "Constraint";
        public const string RouteName = "RouteName";

        public Settings(Dictionary<string, string> settings) 
            : base(settings)
        {
            Register(MatchRoute, "{*data}", "The route which should be matched. It's the standard format of HttpRouteCollection.MapRoute().");
            Register(RewriteTo, "/index.html", "A matching route will be rewritten to this path.");
            Register(Constraint, @".*?$(?<!\.js|.css|.eot)", "A regex constraint which must match with the given request. It's used to exclude static files (assets).");
            Register(RouteName, "Angular HTML5 Navigation", "A name for this route, isn't really important. Maybe dropped in future versions.");
        }
    }
}

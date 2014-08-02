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
            Register(MatchRoute, "{*data}");
            Register(RewriteTo, "/index.html");
            Register(Constraint, @".*?$(?<!\.js|.css|.eot)");
            Register(RouteName, "Angular HTML5 Navigation");
        }
    }
}

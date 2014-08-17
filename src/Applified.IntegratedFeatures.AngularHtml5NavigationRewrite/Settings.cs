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

using System.Collections.Generic;
using Applified.Common;
using Applified.Common.Configuration;

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

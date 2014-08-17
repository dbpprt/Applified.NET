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
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Applified.Common;
using Applified.IntegratedFeatures.Blog.Controllers;
using Microsoft.Owin;
using Owin;

namespace Applified.IntegratedFeatures.Blog.Middlewares
{
    class BlogApiMiddleware : WebapiMiddleware
    {
        private readonly Settings _settings;

        public BlogApiMiddleware(OwinMiddleware next, IAppBuilder app, Settings settings)
            : base(next, app)
        {
            _settings = settings;
        }

        public override Task Invoke(IOwinContext context)
        {
            
            return base.Invoke(context);
        }

        protected override void RegisterRoutes(HttpConfiguration config)
        {
            // nothing todo here because we're using attribute routing
        }

        public override ICollection<Assembly> GetAssemblies()
        {
            return new List<Assembly>
            {
                Assembly.GetExecutingAssembly()
            };
        }

        public override void RegisterControllers(ICollection<Type> controllerTypes)
        {
            controllerTypes.Add(typeof(BlogController));
        }
    }
}

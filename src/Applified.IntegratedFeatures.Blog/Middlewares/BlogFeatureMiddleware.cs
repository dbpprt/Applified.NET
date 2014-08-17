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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Common;
using Microsoft.Owin;
using Owin;

namespace Applified.IntegratedFeatures.Blog.Middlewares
{
    class BlogFeatureMiddleware : OwinMiddlewareBase
    {
        private readonly MiddlewareRouter _router;

        public BlogFeatureMiddleware(OwinMiddleware next, IAppBuilder app, Settings settings) 
            : base(next, app)
        {
            _router = new MiddlewareRouter(next);

            if (settings.GetValue<bool>(Settings.EnableMetaweblogEndpoint))
            {
                _router.Map(
                    settings.GetValue<string>(Settings.MetaweblogEndpoint),
                    "*",
                    new MetaweblogMiddleware(next, app)
                    );
            }

            _router.Map("api/{*data}", new BlogApiMiddleware(next, app, settings));
            _router.Map(
                settings.GetValue<string>(Settings.MetaweblogManifestEndpoint),
                new WlwManifestMiddleware(next, app));
        }

        public override Task Invoke(IOwinContext context)
        {
            return _router.Invoke(context);
        }
    }
}

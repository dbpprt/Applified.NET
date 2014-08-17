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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Applified.Common;
using Microsoft.Owin;
using Owin;

namespace Applified.IntegratedFeatures.Blog.Middlewares
{
    class WlwManifestMiddleware : OwinMiddlewareBase
    {
        const string ResourceName = "Applified.IntegratedFeatures.Blog.Content.wlwmanifest.xml";

        public WlwManifestMiddleware(OwinMiddleware next, IAppBuilder app) : base(next, app)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            using (var stream = assembly.GetManifestResourceStream(ResourceName))
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                context.Response.ContentType = "application/xml";
                context.Response.WriteAsync(result);
            }

            return Task.FromResult(0);
        }
    }
}

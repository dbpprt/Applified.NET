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

//// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Routing;
//using Applified.Common.OwinDependencyInjection;
//using Applified.Core.Extensibility.Contracts;
//using Microsoft.Owin;

//namespace Applified.Core.Middleware
//{
//    public class SimpleUrlRoutingMiddleware
//    {
//        private readonly HttpRouteCollection _routes;
//        private readonly Func<IDictionary<string, object>, Task> _next;

//        public SimpleUrlRoutingMiddleware(
//            Func<IDictionary<string, object>, Task> next, 
//            HttpRouteCollection routes)
//        {
//            if (next == null)
//            {
//                throw new ArgumentNullException("next");
//            }

//            if (routes == null)
//            {
//                throw new ArgumentNullException("routes");
//            }

//            _next = next;
//            _routes = routes;
//        }

//        public Task Invoke(IDictionary<string, object> environment)
//        {
//            IOwinContext context = new OwinContext(environment);
//            var method = new HttpMethod(context.Request.Method);
//            var requestMessage = new HttpRequestMessage(method, context.Request.Uri);

//            var scope = context.Environment.GetRequestContainer();

//            IHttpRouteData match = null;
//            var dynamicRoutes = scope.ResolveAll<IDynamicRewriteRule>().ToList();
//            if (dynamicRoutes.Any())
//            {
//                var collection = new HttpRouteCollection();
//                dynamicRoutes.ForEach(route => route.Register(collection));

//                match = collection.GetRouteData(
//                   requestMessage
//                );
//            }

//            if (ProcessMatch(match, context))
//            {
//                match = _routes.GetRouteData(
//                    requestMessage
//                );

//                ProcessMatch(match, context);
//            }
            
//            return _next(environment);
//        }

//        private static bool ProcessMatch(IHttpRouteData match, IOwinContext context)
//        {
//            object value;
//            if (match != null && match.Route.Defaults.TryGetValue("rewrite", out value))
//            {
//                var rewritePath = value as string;

//                if (rewritePath != null)
//                {
//                    context.Request.Path = new PathString(rewritePath);
//                }

//                return false;
//            }

//            return true;
//        }

//        private static bool TryMatchPath(IOwinContext context, PathString matchUrl, bool forDirectory, out PathString subpath)
//        {
//            var path = context.Request.Path;

//            if (forDirectory && !path.Value.EndsWith("/", StringComparison.Ordinal))
//            {
//                path += new PathString("/");
//            }

//            if (path.StartsWithSegments(matchUrl, out subpath))
//            {
//                return true;
//            }
//            return false;
//        }
//    }
//}

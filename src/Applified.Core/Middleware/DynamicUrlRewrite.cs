// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.Extensibility.Contracts;
using Microsoft.Owin;

namespace Applified.Core.Middleware
{
    public class DynamicUrlRewriteMiddleware : OwinMiddleware
    {
        public DynamicUrlRewriteMiddleware(OwinMiddleware next) 
            : base(next)
        {
        }

        public override Task Invoke(IOwinContext context) 
        {
            var method = new HttpMethod(context.Request.Method);
            var requestMessage = new HttpRequestMessage(method, context.Request.Uri);

            var scope = context.Environment.GetRequestContainer();

            IHttpRouteData match = null;
            var dynamicRoutes = scope.ResolveAll<IDynamicRewriteRule>().ToList();
            if (dynamicRoutes.Any())
            {
                var collection = new HttpRouteCollection();
                dynamicRoutes.ForEach(route => route.Register(collection));

                match = collection.GetRouteData(
                   requestMessage
                );
            }

            ProcessMatch(match, context);

            return Next.Invoke(context);
        }

        private static bool ProcessMatch(IHttpRouteData match, IOwinContext context)
        {
            object value;
            if (match != null && match.Route.Defaults.TryGetValue("rewrite", out value))
            {
                var rewritePath = value as string;

                if (rewritePath != null)
                {
                    context.Request.Path = new PathString(rewritePath);
                }

                return false;
            }

            return true;
        }

    }
}

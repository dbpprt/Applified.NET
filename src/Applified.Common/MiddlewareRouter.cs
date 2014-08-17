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
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Applified.Common.OwinDependencyInjection;
using Applified.Common.Utilities;
using Microsoft.Owin;

namespace Applified.Common
{
    public delegate Task InvokeDelegate(IOwinContext context, IDependencyScope scope);

    public class MiddlewareRouter : OwinMiddleware
    {
        private readonly HttpRouteCollection _routes;
        
        //public MiddlewareRouter() : base()
        //{
        //    _routes = new HttpRouteCollection();

        //    _routes.MapHttpRoute("this", "api/deploy/{*data}", new
        //    {
        //        handler = (InvokeDelegate)InvokeDeploy,
        //        method = "PUT"
        //    });

        //    _routes.MapHttpRoute("new deployment", "api/isc/events/new-deployment/{deploymentId}", new
        //    {
        //        handler = (InvokeDelegate)InvokeDeploy,
        //        method = "PUT"
        //    });
        //}

        public MiddlewareRouter Map(string name, string template, string method, OwinMiddleware middleware)
        {
            return Map(name, template, new RouterOptions
            {
                InvokeDelegate = ((context, scope) => middleware.Invoke(context)),
                Method = method
            });
        }

        public MiddlewareRouter Map(string template, string method, OwinMiddleware middleware)
        {
            var name = middleware.GetType().Name + middleware.GetHashCode();

            return Map(name, template, method, middleware);
        }

        public MiddlewareRouter Map(string template, OwinMiddleware middleware)
        {
            return Map(template, "GET", middleware);
        }

        public MiddlewareRouter Map(string name, string template, RouterOptions options)
        {
            _routes.MapHttpRoute(name, template, options);
            return this;
        }

        public MiddlewareRouter(OwinMiddleware next) : base(next)
        {
            _routes = new HttpRouteCollection();
        }

        public override async Task Invoke(IOwinContext context)
        {
            var method = new HttpMethod(context.Request.Method);
            var requestMessage = new HttpRequestMessage(method, context.Request.Uri);
            var match = _routes.GetRouteData(requestMessage);
            var scope = context.GetRequestContainer();

            if (match != null)
            {
                object routeMethodObj;
                object handlerObj;
               
                if (match.Values.TryGetValue("InvokeDelegate", out handlerObj) &&
                    match.Values.TryGetValue("Method", out routeMethodObj))
                {
                    var routeMethod = routeMethodObj as string;
                    var handler = handlerObj as InvokeDelegate;

                    if (routeMethod != null && (routeMethod == method.Method || routeMethod == "*") && handler != null)
                    {
                        await handler(context, scope);
                    }
                }
            }
            else
            {
                await Next.Invoke(context);
            }
        }
    }

    public class RouterOptions
    {
        public InvokeDelegate InvokeDelegate { get; set; }
        public string Method { get; set; }
    }
}

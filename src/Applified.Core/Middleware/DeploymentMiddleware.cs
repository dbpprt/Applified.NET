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
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Applified.Common;
using Applified.Common.OwinDependencyInjection;
using Applified.Common.Utilities;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.Services.Services;
using Microsoft.Owin;

namespace Applified.Core.Middleware
{
    public class DeploymentMiddleware : OwinMiddleware
    {
        private readonly HttpRouteCollection _routes;
        public delegate Task InvokeDelegate(IOwinContext context, IDependencyScope scope);

        public DeploymentMiddleware(OwinMiddleware next)
            : base(next)
        {
            _routes = new HttpRouteCollection();
            
            // TODO: route handling should be at a single place

            _routes.MapHttpRoute("this", "api/deploy/{*data}", new
            {
                handler = (InvokeDelegate)InvokeDeploy, 
                method = "PUT"
            });

            _routes.MapHttpRoute("new deployment", "api/isc/events/new-deployment/{deploymentId}", new
            {
                handler = (InvokeDelegate)InvokeDeploy,
                method = "PUT"
            });
        }


        public async Task InvokeDeploy(IOwinContext context, IDependencyScope scope)
        {
            var requestData = context.Request.Body.ToByteArray();

            var deploymentService = scope.Resolve<DeploymentService>();

            context.Response.StatusCode = 200;

            var id = await deploymentService.AddDeploymentAsync(
                new StoredObject
                {
                    Name = "deployment.zip",
                    Data = requestData,
                    Size = requestData.Length
                },
                new Deployment
                {
                    CommitMessage = "",
                    PubDate = DateTime.UtcNow
                },
                true
                ).ConfigureAwait(false);

            await context.Response.WriteAsync("Deployment succeeded with id " + id);
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

                if (match.Values.TryGetValue("handler", out handlerObj) &&
                    match.Values.TryGetValue("method", out routeMethodObj))
                {
                    var routeMethod = routeMethodObj as string;
                    var handler = handlerObj as InvokeDelegate;

                    if (routeMethod != null && routeMethod == method.Method && handler != null)
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
}

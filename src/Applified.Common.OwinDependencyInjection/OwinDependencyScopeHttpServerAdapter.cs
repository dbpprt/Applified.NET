﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace Applified.Common.OwinDependencyInjection
{
    public class OwinDependencyScopeHttpServerAdapter : HttpServer
    {
        public OwinDependencyScopeHttpServerAdapter(HttpConfiguration configuration)
            : base(configuration)
        {
        }

        public OwinDependencyScopeHttpServerAdapter(HttpConfiguration configuration, HttpMessageHandler dispatcher)
            : base(configuration, dispatcher)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Properties[HttpPropertyKeys.DependencyScope] = request.GetOwinDependencyScope();
            return base.SendAsync(request, cancellationToken);
        }
    }
}
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
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Owin;
using Applified.Common.OwinDependencyInjection;
using Applified.Common.Unity;
using Applified.Common.Utilities;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;
using Owin;

namespace Applified.Common
{
    class a : IAssembliesResolver
    {
        public ICollection<Assembly> GetAssemblies()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class WebapiMiddleware : 
        OwinMiddlewareBase, 
        IAssembliesResolver, 
        IHttpControllerTypeResolver
    {
        private readonly IAppBuilder _app;
        private readonly ICollection<Type> _controllerTypes; 

        protected HttpMessageHandlerAdapter UnderlayingMiddleware { get; set; }

        protected abstract void RegisterRoutes(HttpConfiguration config);

        public abstract ICollection<Assembly> GetAssemblies();

        public abstract void RegisterControllers(ICollection<Type> controllerTypes);

        protected virtual HttpConfiguration PrepareHttpConfiguration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver
                = new CamelCasePropertyNamesContractResolver();

            RegisterRoutes(config);
            
            config.DependencyResolver = appBuilder.GetApplicationContainer();
            return config;
        }

        protected virtual HttpServer PrepareHttpServer(HttpConfiguration configuration)
        {
            var httpServer = new OwinDependencyScopeHttpServerAdapter(configuration);
            return httpServer;
        }

        protected virtual HttpMessageHandlerOptions PrepareMessageHandlerOptions(IAppBuilder builder, HttpServer server,
            HttpConfiguration configuration)
        {
            var services = configuration.Services;

            var bufferPolicySelector = services.GetHostBufferPolicySelector()
                ?? new OwinBufferPolicySelector();
            var exceptionLogger = ExceptionServices.GetLogger(services);
            var exceptionHandler = ExceptionServices.GetHandler(services);

            return new HttpMessageHandlerOptions
            {
                MessageHandler = server,
                BufferPolicySelector = bufferPolicySelector,
                ExceptionLogger = exceptionLogger,
                ExceptionHandler = exceptionHandler,
                AppDisposing = builder.GetApplicationShutdownToken()
            };
        }

        public WebapiMiddleware(
            OwinMiddleware next, 
            IAppBuilder app) 
                : base(next, app)
        {
            _app = app;
            _controllerTypes = new List<Type>();
            UnderlayingMiddleware = Prepare();
        }

        private HttpMessageHandlerAdapter Prepare()
        {
            RegisterControllers(_controllerTypes);

            var httpConfiguration = PrepareHttpConfiguration(_app);

            httpConfiguration.Services.Replace(typeof(IAssembliesResolver), this);
            httpConfiguration.Services.Replace(typeof(IHttpControllerTypeResolver), this);
            httpConfiguration.Services.Replace(typeof(IHttpControllerSelector), new DebugHttpControllerSelector(httpConfiguration));
            httpConfiguration.Services.Replace(typeof(IHttpActionSelector), new DebugApiControllerActionSelector());

            var httpServer = PrepareHttpServer(httpConfiguration);
            var options = PrepareMessageHandlerOptions(_app, httpServer, httpConfiguration);

            httpConfiguration.MapHttpAttributeRoutes();

            var adapter = new HttpMessageHandlerAdapter(Next, options);

            return adapter;
        }

        public override Task Invoke(IOwinContext context)
        {
            return UnderlayingMiddleware.Invoke(context);
        }

        public ICollection<Type> GetControllerTypes(IAssembliesResolver assembliesResolver)
        {
            return _controllerTypes;
        }
    }
}

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
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Applified.Common;
using Applified.Common.Logging;
using Applified.Common.OwinDependencyInjection;
using Applified.Common.Utilities;
using Applified.Core.Extensibility.Contracts;
using Microsoft.Owin;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Owin;

namespace Applified.Core.Middleware
{
    class ApplicationEventMiddleware : OwinMiddlewareBase
    {
        private readonly IUnityContainer _container;
        private static int _executionCount = 0;
        private static SemaphoreSlim _executionLock;

        public ApplicationEventMiddleware(
            OwinMiddleware next, 
            IAppBuilder appBuilder,
            IUnityContainer container
            )
            : base(next, appBuilder)
        {
            _container = container;
            _executionLock = new SemaphoreSlim(1);

            var token = GetShutdownToken(appBuilder.Properties);
            token.Register(OnApplicationShutdown);
        }

        private void OnApplicationShutdown()
        {
            using (var scope = _container.CreateChildContainer())
            {
                var handlers = scope.ResolveAll<IApplicationEventHandler>().ToList();


                Log.Write("Application shutdown requested. Calling all ApplicationEventHandlers.")
                    .IsVerbose()
                    .Save();

                using (var watch = new PerformanceWatch())
                {
                    foreach (var handler in handlers)
                    {
                        try
                        {
                            handler.OnShutdown();
                        }
                        catch (Exception exception)
                        {
                            exception.ToEvent()
                                .SetMessage("Unexpected error in ApplicationEventHandler")
                                .IsError()
                                .WithObject(handler)
                                .Save(Log);
                        }
                    }

                    Log.Write("Application shuting down now! ApplicationEventHandlers finished in " + watch.Result.Milliseconds + "ms")
                        .IsVerbose()
                        .Save();
                }
            }
        }

        internal static CancellationToken GetShutdownToken(IDictionary<string, object> env)
        {
            object value;
            return env.TryGetValue("host.OnAppDisposing", out value)
                && value is CancellationToken
                ? (CancellationToken)value
                : default(CancellationToken);
        }

        // this method looks a bit ugly at the moment..
        // but this solution seems to be a good way to make this threadsafe
        // it ensures that all handlers are called before the first request gets processed
        public override async Task Invoke(IOwinContext context)
        {
            if (_executionCount > 0)
            {
                await Next.Invoke(context);
                return;
            }

            var scope = context.GetRequestContainer();
            var handlers = scope.ResolveAll<IApplicationEventHandler>().ToList();

            if (!handlers.Any())
            {
                await Next.Invoke(context);
                return;
            }

            if (_executionCount == 0)
            {
                await _executionLock.WaitAsync();

                if (_executionCount == 0)
                {
                    try
                    {

                        foreach (var applicationEventHandler in handlers)
                        {
                            try
                            {
                                await applicationEventHandler.OnStartup(_container, scope);
                            }
                            catch (Exception ex)
                            {
                                // TODO: implement a logging mechanism

                                throw;
                            }
                        }
                    }
                    finally
                    {
                        _executionCount++;
                        _executionLock.Release();
                    }

                    await RedirectToCurrent(context);
                }
                else
                {
                    await Next.Invoke(context);
                }
            }
            else
            {
                await Next.Invoke(context);
            }
        }

        private Task RedirectToCurrent(IOwinContext context)
        {
            context.Response.Headers.Add("Location", new[] { context.Request.Path.ToString() });
            context.Response.StatusCode = 302;
            return Task.FromResult(0);
        }
    }
}

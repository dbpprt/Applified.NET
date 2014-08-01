using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.Extensibility.Contracts;
using Microsoft.Owin;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Applified.Core.Middleware
{
    class ApplicationEventMiddleware : OwinMiddleware
    {
        private readonly IUnityContainer _container;
        private static int _executionCount = 0;
        private static SemaphoreSlim _executionLock;

        public ApplicationEventMiddleware(OwinMiddleware next, IUnityContainer container)
            : base(next)
        {
            _container = container;
            _executionLock = new SemaphoreSlim(1);
        }

        internal static CancellationToken GetShutdownToken(IDictionary<string, object> env)
        {
            object value;
            return env.TryGetValue("host.OnAppDisposing", out value)
                && value is CancellationToken
                ? (CancellationToken)value
                : default(CancellationToken);
        }

        // TODO: Register Application shutdown events!


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
                }

                await Next.Invoke(context);
            }
            else
            {
                await Next.Invoke(context);
            }
        }
    }
}

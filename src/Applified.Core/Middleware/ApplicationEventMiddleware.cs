using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private static readonly object ExecutionLock = new object();

        public ApplicationEventMiddleware(OwinMiddleware next, IUnityContainer container)
            : base(next)
        {
            _container = container;
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

            lock (ExecutionLock)
            {
                if (_executionCount == 0)
                {
                    handlers.ForEach(async handler => await handler.OnStartup(_container, scope).ConfigureAwait(false));
                }

                _executionCount++;
            }
        }
    }
}

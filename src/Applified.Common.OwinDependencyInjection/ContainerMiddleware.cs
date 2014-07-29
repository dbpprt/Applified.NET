using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Owin;

namespace Applified.Common.OwinDependencyInjection
{
    public class ContainerMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _nextFunc;
        private readonly IAppBuilder _app;

        public ContainerMiddleware(Func<IDictionary<string, object>, Task> nextFunc, IAppBuilder app)
        {
            _nextFunc = nextFunc;
            _app = app;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            using (var scope = environment.SetRequestContainer(_app))
            {
                IEnumerable<IDynamicRegistrationDelegate> delegates = null;
                var context = new OwinContext(environment);

                try
                {
                    delegates = scope.GetUnderlayingContainer().ResolveAll<IDynamicRegistrationDelegate>();
                }
                catch { }

                try
                {
                    if (delegates != null)
                    {
                        delegates = delegates.ToList();
                        delegates.ForEach(_ => _.InterceptRequestScope(scope, context));
                    }

                    await _nextFunc(environment);
                }
                catch (Exception exception)
                {
                    throw;
                }
            }
        }
    }
}
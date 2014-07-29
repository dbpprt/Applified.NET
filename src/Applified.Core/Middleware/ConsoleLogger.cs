using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Applified.Core.Middleware
{
    public class ConsoleLoggerMiddleware 
    {
        private readonly Func<IDictionary<string, object>, Task> _next;

        public ConsoleLoggerMiddleware(
            Func<IDictionary<string, object>, Task> next)
        {

            _next = next;
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);

            //var requestContainer = (IServiceProvider) context.Environment.GetRequestContainer();
            //var unitOfWork = requestContainer.GetService(typeof(IUnitOfWork)) as IUnitOfWork;

            var path = context.Request.Path;

            var stopWatch = Stopwatch.StartNew();
            return _next(environment).ContinueWith(t =>
            {
                Console.WriteLine("{0} - {1} in {2}ms with response {3} {4}", 
                    context.Request.Method,
                    path, 
                    stopWatch.ElapsedMilliseconds,
                    context.Response.StatusCode,
                    string.IsNullOrEmpty(context.Response.ReasonPhrase) ? "" : " - " + context.Response.ReasonPhrase);
                return t;
            });

        }
    }
}

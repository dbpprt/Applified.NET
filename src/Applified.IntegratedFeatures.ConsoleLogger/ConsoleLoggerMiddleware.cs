using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.ServiceContracts;
using Microsoft.Owin;

namespace Applified.IntegratedFeatures.ConsoleLogger
{
    class ConsoleLoggerMiddleware : OwinMiddleware
    {
        public ConsoleLoggerMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            var path = context.Request.Path;
            var scope = context.GetRequestContainer();
            var stopWatch = Stopwatch.StartNew();
            var currentApplication = scope.Resolve<ICurrentContext>();

            return Next.Invoke(context).ContinueWith(t =>
            {
                Console.WriteLine("[{5}] {0} - {1} in {2}ms with response {3} {4}",
                    context.Request.Method,
                    path,
                    stopWatch.ElapsedMilliseconds,
                    context.Response.StatusCode,
                    string.IsNullOrEmpty(context.Response.ReasonPhrase) ? "" : " - " + context.Response.ReasonPhrase,
                    currentApplication.ApplicationId);
                return t;
            });
        }
    }
}

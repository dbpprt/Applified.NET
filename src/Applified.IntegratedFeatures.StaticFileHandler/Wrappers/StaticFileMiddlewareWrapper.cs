using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.StaticFiles;

namespace Applified.IntegratedFeatures.StaticFileHandler.Wrappers
{
    class StaticFileMiddlewareWrapper : OwinMiddleware
    {
        private readonly StaticFileMiddleware _underlayingMiddleware;

        public StaticFileMiddlewareWrapper(
            OwinMiddleware next, 
            StaticFileOptions options
            ) : base(next)
        {
            _underlayingMiddleware = new StaticFileMiddleware(
                objects => next.Invoke(new OwinContext(objects)), 
                options);
        }

        public override Task Invoke(IOwinContext context)
        {
            return _underlayingMiddleware.Invoke(context.Environment);
        }
    }
}

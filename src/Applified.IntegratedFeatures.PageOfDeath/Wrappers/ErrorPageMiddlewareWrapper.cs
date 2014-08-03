using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Diagnostics;

namespace Applified.IntegratedFeatures.PageOfDeath.Wrappers
{
    class ErrorPageMiddlewareWrapper : OwinMiddleware
    {
         private readonly ErrorPageMiddleware _underlayingMiddleware;

         public ErrorPageMiddlewareWrapper(
            OwinMiddleware next, 
            ErrorPageOptions options
            ) : base(next)
        {
            _underlayingMiddleware = new ErrorPageMiddleware(
                objects => next.Invoke(new OwinContext(objects)),
                options,
                null,
                false);
        }

        public override Task Invoke(IOwinContext context)
        {
            return _underlayingMiddleware.Invoke(context.Environment);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.StaticFiles;

namespace Applified.IntegratedFeatures.StaticFileHandler.Wrappers
{
    class DirectoryBrowserMiddlewareWrapper : OwinMiddleware
    {
        private readonly DirectoryBrowserMiddleware _underlayingMiddleware;

        public DirectoryBrowserMiddlewareWrapper(
            OwinMiddleware next, 
            DirectoryBrowserOptions options
            ) : base(next)
        {
            _underlayingMiddleware = new DirectoryBrowserMiddleware(
                objects => next.Invoke(new OwinContext(objects)), 
                options);
        }

        public override Task Invoke(IOwinContext context)
        {
            return _underlayingMiddleware.Invoke(context.Environment);
        }
    }
}

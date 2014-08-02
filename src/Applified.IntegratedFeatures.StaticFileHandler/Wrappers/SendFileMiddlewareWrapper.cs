using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.StaticFiles;

namespace Applified.IntegratedFeatures.StaticFileHandler.Wrappers
{
    class SendFileMiddlewareWrapper : OwinMiddleware
    {
        private readonly SendFileMiddleware _underlayingMiddleware;

        public SendFileMiddlewareWrapper(
            OwinMiddleware next
            ) : base(next)
        {
            _underlayingMiddleware = new SendFileMiddleware(
                objects => next.Invoke(new OwinContext(objects)));
        }

        public override Task Invoke(IOwinContext context)
        {
            return _underlayingMiddleware.Invoke(context.Environment);
        }
    }
}

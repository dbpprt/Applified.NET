using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.StaticFiles;

namespace Applified.IntegratedFeatures.StaticFileHandler.Wrappers
{
    class DefaultFilesMiddlewareWrapper : OwinMiddleware
    {
        private readonly DefaultFilesMiddleware _underlayingMiddleware;

        public DefaultFilesMiddlewareWrapper(
            OwinMiddleware next, 
            DefaultFilesOptions options
            ) : base(next)
        {
            _underlayingMiddleware = new DefaultFilesMiddleware(
                objects => next.Invoke(new OwinContext(objects)), 
                options);
        }

        public override Task Invoke(IOwinContext context)
        {
            return _underlayingMiddleware.Invoke(context.Environment);
        }
    }
}

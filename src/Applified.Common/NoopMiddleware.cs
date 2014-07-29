using System.Threading.Tasks;
using Microsoft.Owin;

namespace Applified.Common
{
    public class NoopMiddleware : OwinMiddleware
    {
        public NoopMiddleware() : base(null)
        {
        }

        public override Task Invoke(IOwinContext context)
        {
            return Task.FromResult(0);
        }
    }
}
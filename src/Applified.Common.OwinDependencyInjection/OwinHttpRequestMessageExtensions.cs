using System.Net.Http;
using System.Web.Http.Dependencies;

namespace Applified.Common.OwinDependencyInjection
{
    internal static class OwinHttpRequestMessageExtensions
    {
        internal static IDependencyScope GetOwinDependencyScope(this HttpRequestMessage request)
        {
            var requestContainer = request.GetOwinContext().Environment.GetRequestContainer();
            return requestContainer;
        }
    }
}
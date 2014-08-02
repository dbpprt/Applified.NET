using System.Threading.Tasks;
using Microsoft.Owin;

namespace Applified.Common.OwinDependencyInjection
{
    public interface IDynamicRegistrationDelegate
    {
        Task InterceptRequestScope(IUnityServiceProvider provider, IOwinContext context);
    }
}

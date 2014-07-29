using Microsoft.Owin;

namespace Applified.Common.OwinDependencyInjection
{
    public interface IDynamicRegistrationDelegate
    {
        void InterceptRequestScope(IUnityServiceProvider provider, IOwinContext context);
    }
}

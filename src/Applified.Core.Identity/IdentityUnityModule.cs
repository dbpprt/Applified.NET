using System;
using System.Threading.Tasks;
using Applified.Common.OwinDependencyInjection;
using Applified.Common.Unity;
using Applified.Core.Entities.Identity;
using Applified.Core.Identity.Managers;
using Applified.Core.Identity.Stores;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Practices.Unity;

namespace Applified.Core.Identity
{
    public class IdentityUnityModule : UnityModule
    {
        public override void RegisterDependencies(IUnityContainer container)
        {
            container
                .RegisterType<IRoleStore<Role, Guid>, RoleStore>(new HierarchicalLifetimeManager())
                .RegisterType<IUserStore<UserAccount, Guid>, UserStore>(new HierarchicalLifetimeManager())

                .RegisterType<RoleManager>(new HierarchicalLifetimeManager())
                .RegisterType<UserManager>(new HierarchicalLifetimeManager())
                .RegisterType<SignInManager>(new HierarchicalLifetimeManager())

                .RegisterType<IDynamicRegistrationDelegate, DynamicOwinAuthenticationManagerRegistration>(
                    "AuthenticationManagerRegistration",
                    new ExternallyControlledLifetimeManager());
        }

        private class DynamicOwinAuthenticationManagerRegistration : IDynamicRegistrationDelegate
        {
            public Task InterceptRequestScope(IUnityServiceProvider provider, IOwinContext context)
            {
                provider.GetUnderlayingContainer().RegisterInstance(context.Authentication, new HierarchicalLifetimeManager());

                return Task.FromResult(0);
            }
        }
    }
}

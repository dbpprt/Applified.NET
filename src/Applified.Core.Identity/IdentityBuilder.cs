using System;
using Applified.Core.Identity.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Applified.Core.Identity
{
    public static class IdentityBuilder
    {
        public static void Build(IAppBuilder app)
        {
            app.SetDataProtectionProvider(new DataProtectionProvider());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity(
                        validateInterval: TimeSpan.FromMinutes(60))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
           
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(60));

            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {

                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                Provider = new IdentityAuthorizationServerProvider(),
                //RefreshTokenProvider = new SimpleRefreshTokenProvider()
                
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {

            });

            app.Use<TenantDemandAuthenticationMiddleware>(app);

        }
    }
}


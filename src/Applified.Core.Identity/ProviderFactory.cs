using Applified.Core.Entities.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.MicrosoftAccount;
using Microsoft.Owin.Security.Twitter;
using Owin;
using Owin.Security.Providers.GitHub;
using Owin.Security.Providers.Instagram;
using Owin.Security.Providers.LinkedIn;
using Owin.Security.Providers.Reddit;
using Owin.Security.Providers.Salesforce;
using Owin.Security.Providers.Yahoo;

namespace Applified.Core.Identity
{
    public static class ProviderFactory
    {
        public static OwinMiddleware ToMiddleware(this ExternalOAuthProvider provider, OwinMiddleware nextMiddleware, IAppBuilder appBuilder)
        {
            // TODO: This could be nicer.. Think about a design pattern

            if (provider.Name == "Twitter")
            {
                return new TwitterAuthenticationMiddleware(nextMiddleware, appBuilder, new TwitterAuthenticationOptions
                {
                    ConsumerKey  = provider.ClientId,
                    ConsumerSecret = provider.ClientSecret
                });
            }
            else if (provider.Name == "Facebook")
            {
                return new FacebookAuthenticationMiddleware(nextMiddleware, appBuilder, new FacebookAuthenticationOptions
                {
                    AppId  = provider.ClientId,
                    AppSecret = provider.ClientSecret
                });
            }
            else if (provider.Name == "Google")
            {
                return new GoogleOAuth2AuthenticationMiddleware(nextMiddleware, appBuilder, new GoogleOAuth2AuthenticationOptions
                {
                    ClientId  = provider.ClientId,
                    ClientSecret = provider.ClientSecret
                });
            }
            else if (provider.Name == "Microsoft")
            {
                return new MicrosoftAccountAuthenticationMiddleware(nextMiddleware, appBuilder, new MicrosoftAccountAuthenticationOptions
                {
                    ClientId  = provider.ClientId,
                    ClientSecret = provider.ClientSecret
                });
            }
            else if (provider.Name == "GitHub")
            {
                return new GitHubAuthenticationMiddleware(nextMiddleware, appBuilder, new GitHubAuthenticationOptions
                {
                    ClientId = provider.ClientId,
                    ClientSecret = provider.ClientSecret
                });

            }
            else if (provider.Name == "Instagram")
            {
                return new InstagramAuthenticationMiddleware(nextMiddleware, appBuilder, new InstagramAuthenticationOptions
                {
                    ClientId = provider.ClientId,
                    ClientSecret = provider.ClientSecret
                });

            }
            else if (provider.Name == "LinkedIn")
            {
                return new LinkedInAuthenticationMiddleware(nextMiddleware, appBuilder, new LinkedInAuthenticationOptions
                {
                    ClientId = provider.ClientId,
                    ClientSecret = provider.ClientSecret
                });

            }
            else if (provider.Name == "Reddit")
            {
                return new RedditAuthenticationMiddleware(nextMiddleware, appBuilder, new RedditAuthenticationOptions
                {
                    ClientId = provider.ClientId,
                    ClientSecret = provider.ClientSecret
                });

            }
            else if (provider.Name == "Salesforce")
            {
                return new SalesforceAuthenticationMiddleware(nextMiddleware, appBuilder, new SalesforceAuthenticationOptions
                {
                    ClientId = provider.ClientId,
                    ClientSecret = provider.ClientSecret
                });

            }
            else if (provider.Name == "Yahoo")
            {
                return new YahooAuthenticationMiddleware(nextMiddleware, appBuilder, new YahooAuthenticationOptions
                {
                    ConsumerKey = provider.ClientId,
                    ConsumerSecret = provider.ClientSecret
                });

            }
            

            return null;
        } 
    }
}

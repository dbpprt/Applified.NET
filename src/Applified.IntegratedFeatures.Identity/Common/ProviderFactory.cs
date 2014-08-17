#region Copyright (C) 2014 Applified.NET 
// Copyright (C) 2014 Applified.NET
// http://www.applified.net

// This file is part of Applified.NET.

// Applified.NET is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.

// You should have received a copy of the GNU Affero General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion

using Applified.IntegratedFeatures.Identity.Entities;
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

namespace Applified.IntegratedFeatures.Identity.Common
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

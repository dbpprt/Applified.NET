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

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.Identity.Managers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;

namespace Applified.Core.Identity
{
    public static class SecurityStampValidator
    {
        public static Func<CookieValidateIdentityContext, Task> OnValidateIdentity(
            TimeSpan validateInterval)
        {
            return async context =>
            {
                var currentUtc = DateTimeOffset.UtcNow;
                if (context.Options != null && context.Options.SystemClock != null)
                    currentUtc = context.Options.SystemClock.UtcNow;
                var issuedUtc = context.Properties.IssuedUtc;
                var validate = !issuedUtc.HasValue;
                if (issuedUtc.HasValue)
                    validate = currentUtc.Subtract(issuedUtc.Value) > validateInterval;
                if (validate)
                {
                    var scope = context.OwinContext.Environment.GetRequestContainer() as IServiceProvider;
                    var userIdAsString = context.Identity.GetUserId<string>();
                    var manager = scope.Resolve<UserManager>();
                    var userId = Guid.Parse(userIdAsString);

                    if (manager != null)
                    {
                        var user = await manager.FindByIdAsync(userId);
                        var reject = true;
                        if (user != null && manager.SupportsUserSecurityStamp)
                        {
                            var securityStamp = context.Identity.FindFirstValue("AspNet.Identity.SecurityStamp");
                            if (securityStamp == await manager.GetSecurityStampAsync(userId))
                            {
                                reject = false;
                                var identity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                                if (identity != null)
                                {
                                    context.OwinContext.Authentication.SignIn(new ClaimsIdentity[1]
                                        {
                                            identity
                                        });
                                }
                            }
                        }
                        if (reject)
                        {
                            context.RejectIdentity();
                            context.OwinContext.Authentication.SignOut(new string[1]
                            {
                                context.Options.AuthenticationType
                            });
                        }
                    }
                }
            };
        }
    }
}

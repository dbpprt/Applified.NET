using System;
using Applified.Core.Entities.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Applified.Core.Identity.Managers
{
    public class SignInManager : SignInManager<UserAccount, Guid>
    {
        public SignInManager(UserManager<UserAccount, Guid> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {

        }
    }
}

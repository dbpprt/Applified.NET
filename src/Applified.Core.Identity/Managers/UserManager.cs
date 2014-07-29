using System;
using Applified.Core.Entities.Identity;
using Microsoft.AspNet.Identity;

namespace Applified.Core.Identity.Managers
{
    public class UserManager : UserManager<UserAccount, Guid>
    {
        public UserManager(IUserStore<UserAccount, Guid> store) : base(store)
        {
        }
    }
}

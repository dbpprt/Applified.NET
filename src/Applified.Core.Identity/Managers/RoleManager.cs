using System;
using Applified.Core.Entities.Identity;
using Microsoft.AspNet.Identity;

namespace Applified.Core.Identity.Managers
{
    public class RoleManager : RoleManager<Role, Guid>
    {
        public RoleManager(IRoleStore<Role, Guid> store) : base(store)
        {
        }
    }
}

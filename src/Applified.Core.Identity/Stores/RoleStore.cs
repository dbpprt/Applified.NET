using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Identity;
using Microsoft.AspNet.Identity;

namespace Applified.Core.Identity.Stores
{
    public class RoleStore : IQueryableRoleStore<Role, Guid>
    {
        private readonly IRepository<Role> _roles;

        public RoleStore(
            IRepository<Role> roles
            )
        {
            _roles = roles;
        }

        public Task CreateAsync(Role role)
        {
            return _roles.InsertAsync(role);
        }

        public Task UpdateAsync(Role role)
        {
            return _roles.UpdateAsync(role);
        }

        public async Task DeleteAsync(Role role)
        {
            var target = await FindByIdAsync(role.Id);
            await _roles.DeleteAsync(target);
        }

        public Task<Role> FindByIdAsync(Guid roleId)
        {
            return _roles.Query()
                .FirstOrDefaultAsync(entity => entity.Id == roleId);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return _roles.Query()
                .FirstOrDefaultAsync(entity => entity.Name == roleName);
        }

        public IQueryable<Role> Roles
        {
            get { return _roles.Query(); }
        }

        public void Dispose()
        {

        }
    }
}

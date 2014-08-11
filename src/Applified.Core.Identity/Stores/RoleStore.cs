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

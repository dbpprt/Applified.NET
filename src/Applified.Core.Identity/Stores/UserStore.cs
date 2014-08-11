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
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Identity;
using Microsoft.AspNet.Identity;

namespace Applified.Core.Identity.Stores
{
    public class UserStore : 
        IUserLoginStore<UserAccount, Guid>, 
        IUserClaimStore<UserAccount, Guid>, 
        IUserRoleStore<UserAccount, Guid>, 
        IUserPasswordStore<UserAccount, Guid>, 
        IUserSecurityStampStore<UserAccount, Guid>, 
        IQueryableUserStore<UserAccount, Guid>, 
        IUserEmailStore<UserAccount, Guid>, 
        IUserPhoneNumberStore<UserAccount, Guid>, 
        IUserTwoFactorStore<UserAccount, Guid>, 
        IUserLockoutStore<UserAccount, Guid>
    {
        private readonly IRepository<UserAccount> _userAccounts;
        private readonly IRepository<UserAccountRoleMapping> _userAccountRoleMappings;
        private readonly IRepository<UserClaim> _claims;
        private readonly IRepository<UserLogin> _userLogins;
        private readonly RoleStore _roleStore;

        private bool _autosave = false;

        public UserStore(
            IRepository<UserAccount> userAccounts,
            IRepository<UserAccountRoleMapping> userAccountRoleMappings,
            IRepository<UserClaim> claims,
            IRepository<UserLogin> userLogins,
            RoleStore roleStore
            )
        {
            _userAccounts = userAccounts;
            _userAccountRoleMappings = userAccountRoleMappings;
            _claims = claims;
            _userLogins = userLogins;
            _roleStore = roleStore;
        }

        public Task CreateAsync(UserAccount user)
        {
            return _userAccounts.InsertAsync(user);
        }

        public Task UpdateAsync(UserAccount user)
        {
            return _userAccounts.UpdateAsync(user);
        }

        public Task DeleteAsync(UserAccount user)
        {
            throw new NotImplementedException();
        }

        public Task<UserAccount> FindByIdAsync(Guid userId)
        {
            return _userAccounts.Query()
                .FirstOrDefaultAsync(entity => entity.Id == userId);
        }

        public Task<UserAccount> FindByNameAsync(string userName)
        {
            return _userAccounts.Query()
                .FirstOrDefaultAsync(entity => entity.UserName == userName);
        }

        public Task AddLoginAsync(UserAccount user, UserLoginInfo login)
        {
            var entity = new UserLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                UserId = user.Id
            };

            return _userLogins.InsertAsync(entity, _autosave);
        }

        public async Task RemoveLoginAsync(UserAccount user, UserLoginInfo login)
        {
            var target = await _userLogins.Query()
                .FirstOrDefaultAsync(
                    entity => entity.LoginProvider == login.LoginProvider && entity.ProviderKey == login.ProviderKey);

            await _userLogins.DeleteAsync(target, _autosave);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(UserAccount user)
        {
            var results = await _userAccounts.Query()
                .Where(entity => entity.Id == user.Id)
                .Include(entity => entity.Logins)
                .SelectMany(entity => entity.Logins)
                .ToListAsync();

            return results
                .Select(result => new UserLoginInfo(result.LoginProvider, result.ProviderKey))
                .ToList();
        }

        public Task<UserAccount> FindAsync(UserLoginInfo login)
        {
            return _userAccounts.Query()
                .Include(entity => entity.Logins)
                .Where(
                    entity =>
                        entity.Logins.Any(
                            inner =>
                                inner.ProviderKey == login.ProviderKey && inner.LoginProvider == login.LoginProvider))
                .FirstOrDefaultAsync();
        }

        public async Task<IList<Claim>> GetClaimsAsync(UserAccount user)
        {
            var results = await _userAccounts.Query()
                .Include(entity => entity.Claims)
                .Where(entity => entity.Id == user.Id)
                .SelectMany(entity => entity.Claims)
                .ToListAsync();

            return results
                .Select(result => new Claim(result.ClaimType, result.ClaimValue))
                .ToList();
        }

        public Task AddClaimAsync(UserAccount user, Claim claim)
        {
            var entity = new UserClaim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                UserId = user.Id
            };

            return _claims.InsertAsync(entity, _autosave);
        }

        public async Task RemoveClaimAsync(UserAccount user, Claim claim)
        {
            var target = await _claims.Query()
                .FirstOrDefaultAsync(
                    entity => entity.UserId == user.Id && entity.ClaimType == claim.Type && entity.ClaimValue == claim.Value);

            await _claims.DeleteAsync(target, _autosave);
        }

        public async Task AddToRoleAsync(UserAccount user, string roleName)
        {
            var role = await _roleStore.FindByNameAsync(roleName);

            if (role == null)
            {
                throw new ArgumentException("roleName");
            }

            var mapping = new UserAccountRoleMapping
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            await _userAccountRoleMappings.InsertAsync(mapping, _autosave);
        }

        public async Task RemoveFromRoleAsync(UserAccount user, string roleName)
        {
            var role = await _roleStore.FindByNameAsync(roleName);

            if (role == null)
            {
                throw new ArgumentException("roleName");
            }

            var mapping = await _userAccountRoleMappings.Query()
                .FirstOrDefaultAsync(entity => entity.UserId == user.Id && entity.RoleId == role.Id);

            if (mapping == null)
                return;

            await _userAccountRoleMappings.DeleteAsync(mapping, _autosave);
        }

        public async Task<IList<string>> GetRolesAsync(UserAccount user)
        {
            var results = await _userAccountRoleMappings.Query()
                .Include(entity => entity.Role)
                .Where(entity => entity.UserId == user.Id)
                .Select(entity => entity.Role.Name)
                .ToListAsync();

            return results;
        }

        public async Task<bool> IsInRoleAsync(UserAccount user, string roleName)
        {
            var role = await _roleStore.FindByNameAsync(roleName);

            if (role == null)
            {
                throw new ArgumentException("roleName");
            }

            return await _userAccountRoleMappings.Query()
                .AnyAsync(entity => entity.RoleId == role.Id && entity.UserId == user.Id);
        }

        public Task SetPasswordHashAsync(UserAccount user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(UserAccount user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(UserAccount user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetSecurityStampAsync(UserAccount user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(UserAccount user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public IQueryable<UserAccount> Users
        {
            get { return _userAccounts.Query(); }
        }

        public Task SetEmailAsync(UserAccount user, string email)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(UserAccount user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(UserAccount user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(UserAccount user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<UserAccount> FindByEmailAsync(string email)
        {
            return _userAccounts.Query()
                .FirstOrDefaultAsync(entity => entity.Email == email);
        }

        public Task SetPhoneNumberAsync(UserAccount user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(UserAccount user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(UserAccount user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(UserAccount user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(UserAccount user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(UserAccount user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(UserAccount user)
        {
            return Task.FromResult(user.LockoutEndDateUtc.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc)) : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(UserAccount user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd == DateTimeOffset.MinValue ? new DateTime?() : lockoutEnd.UtcDateTime;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(UserAccount user)
        {
            user.AccessFailedCount += 1;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(UserAccount user)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(UserAccount user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(UserAccount user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(UserAccount user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        public void Dispose()
        {

        }
    }
}

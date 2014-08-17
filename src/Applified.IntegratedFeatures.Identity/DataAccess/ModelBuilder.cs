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

using System.Data.Entity;
using Applified.Core.DataAccess.Contracts;
using Applified.IntegratedFeatures.Identity.Entities;

namespace Applified.IntegratedFeatures.Identity.DataAccess
{
    class ModelBuilder : IModelBuilder
    {
        public void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExternalOAuthProvider>()
                .ToTable("identity.ExternalOAuthProviders");
            modelBuilder.Entity<OAuthClient>()
                .ToTable("identity.OAuthClients");
            //modelBuilder.Entity<RefreshToken>()
            //    .ToTable("identity.RefreshTokens");
            modelBuilder.Entity<Role>()
                .ToTable("identity.Roles");
            modelBuilder.Entity<UserAccount>()
                .ToTable("identity.UserAccounts");
            modelBuilder.Entity<UserAccountRoleMapping>()
                .ToTable("identity.UserAccountRoleMappings");
            modelBuilder.Entity<UserClaim>()
                .ToTable("identity.UserClaims");
            modelBuilder.Entity<UserLogin>()
                .ToTable("identity.UserLogins");
        }
    }
}

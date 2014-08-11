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

//using System;
//using System.Data.Entity;
//using System.Threading.Tasks;
//using Applified.Core.Entities.Identity;
//using Applified.Core.Entities.Infrastructure;
//using Microsoft.AspNet.Identity;
//using PersonalPage.DataAccess.Contracts;
//using PersonalPage.Model.Contracts;
//using PersonalPage.Model.Entities.Identity;
//using PersonalPage.Model.Entities.Infrastructure;
//using PersonalPage.Shared;

//namespace PersonalPage.Identity.Actions
//{
//    public class SeedIdentityPostApplicationCreationStep : IPostApplicationCreationStep
//    {
//        private readonly RoleManager _roleManager;
//        private readonly UserManager _userManager;
//        private readonly IRepository<OAuthClient> _oAuthClients;

//        public SeedIdentityPostApplicationCreationStep(
//            RoleManager roleManager,
//            UserManager userManager,
//            IRepository<OAuthClient> oAuthClients
//            )
//        {
//            _roleManager = roleManager;
//            _userManager = userManager;
//            _oAuthClients = oAuthClients;
//        }

//        public async Task ExcecuteAsync(Application application)
//        {
//            await _roleManager.CreateAsync(new Role
//            {
//                Name = WellKnownRoles.Administrators
//            });

//            await _roleManager.CreateAsync(new Role
//            {
//                Name = WellKnownRoles.BlogAuthors
//            });

//            var user = await _userManager.FindByNameAsync("admin");
//            if (user == null)
//            {
//                user = new UserAccount { UserName = "admin", Email = "admin@change.me" };
//                var result = await _userManager.CreateAsync(user, "adminadmin");
//                await _userManager.SetLockoutEnabledAsync(user.Id, false);
//            }

//            await _userManager.AddToRolesAsync(user.Id, new[]
//            {
//                WellKnownRoles.Administrators
//            });

//            var oAuthClient = new OAuthClient
//            {
//                Active = true,
//                AllowedGrant = OAuthGrant.ResourceOwner,
//                ApplicationId = application.Id,
//                Name = "Web Application OAuth Client",
//                RefreshTokenLifeTime = new TimeSpan(30, 0, 0, 0).Minutes,
//                AllowedOrigin = "*",
//                Id = "webApplication",
//                Secret = null
//            };

//            if (!await _oAuthClients.Query().AnyAsync(entity => entity.Id == oAuthClient.Id))
//            {
//                await _oAuthClients.InsertAsync(oAuthClient);
//            }
//        }
//    }
//}

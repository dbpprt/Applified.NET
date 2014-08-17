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

namespace Applified.IntegratedFeatures.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "identity.ExternalOAuthProviders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        ClientSecret = c.String(nullable: false),
                        ClientId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.Name, t.ApplicationId }, unique: true, name: "EnsureUniqueName");
            
            CreateTable(
                "identity.OAuthClients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ApplicationId = c.Guid(nullable: false),
                        Secret = c.String(),
                        Name = c.String(nullable: false, maxLength: 100),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 100),
                        AllowedGrant = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.Id, t.ApplicationId }, unique: true, name: "EnsureUniqueRoleName");
            
            CreateTable(
                "identity.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => new { t.Id, t.ApplicationId })
                .Index(t => t.Id, unique: true)
                .Index(t => new { t.ApplicationId, t.Id, t.Name }, unique: true, name: "EnsureUniqueRoleName");
            
            CreateTable(
                "identity.UserAccountRoleMappings",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        Test = c.String(),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("identity.Roles", t => new { t.RoleId, t.ApplicationId }, cascadeDelete: true)
                .ForeignKey("identity.UserAccounts", t => new { t.UserId, t.ApplicationId }, cascadeDelete: true)
                .Index(t => new { t.UserId, t.ApplicationId })
                .Index(t => t.RoleId, name: "IX_RoleId_ApplicationId");
            
            CreateTable(
                "identity.UserAccounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        Email = c.String(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(nullable: false),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => new { t.Id, t.ApplicationId })
                .Index(t => t.Id, unique: true);
            
            CreateTable(
                "identity.UserClaims",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        ClaimType = c.String(nullable: false, maxLength: 128),
                        ApplicationId = c.Guid(nullable: false),
                        ClaimValue = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ClaimType })
                .ForeignKey("identity.UserAccounts", t => new { t.UserId, t.ApplicationId }, cascadeDelete: true)
                .Index(t => new { t.UserId, t.ApplicationId });
            
            CreateTable(
                "identity.UserLogins",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("identity.UserAccounts", t => new { t.UserId, t.ApplicationId }, cascadeDelete: true)
                .Index(t => new { t.UserId, t.ApplicationId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("identity.UserAccountRoleMappings", new[] { "UserId", "ApplicationId" }, "identity.UserAccounts");
            DropForeignKey("identity.UserLogins", new[] { "UserId", "ApplicationId" }, "identity.UserAccounts");
            DropForeignKey("identity.UserClaims", new[] { "UserId", "ApplicationId" }, "identity.UserAccounts");
            DropForeignKey("identity.UserAccountRoleMappings", new[] { "RoleId", "ApplicationId" }, "identity.Roles");
            DropIndex("identity.UserLogins", new[] { "UserId", "ApplicationId" });
            DropIndex("identity.UserClaims", new[] { "UserId", "ApplicationId" });
            DropIndex("identity.UserAccounts", new[] { "Id" });
            DropIndex("identity.UserAccountRoleMappings", "IX_RoleId_ApplicationId");
            DropIndex("identity.UserAccountRoleMappings", new[] { "UserId", "ApplicationId" });
            DropIndex("identity.Roles", "EnsureUniqueRoleName");
            DropIndex("identity.Roles", new[] { "Id" });
            DropIndex("identity.OAuthClients", "EnsureUniqueRoleName");
            DropIndex("identity.ExternalOAuthProviders", "EnsureUniqueName");
            DropTable("identity.UserLogins");
            DropTable("identity.UserClaims");
            DropTable("identity.UserAccounts");
            DropTable("identity.UserAccountRoleMappings");
            DropTable("identity.Roles");
            DropTable("identity.OAuthClients");
            DropTable("identity.ExternalOAuthProviders");
        }
    }
}

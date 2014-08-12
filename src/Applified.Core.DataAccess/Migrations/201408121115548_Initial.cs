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

namespace Applified.Core.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "core.StoredObjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        Type = c.String(),
                        Data = c.Binary(nullable: false),
                        Name = c.String(nullable: false),
                        Size = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.Applications", t => t.ApplicationId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "core.Applications",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccessToken = c.String(nullable: false, maxLength: 128),
                        ActiveDeploymentId = c.Guid(),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.Deployments", t => t.ActiveDeploymentId)
                .Index(t => t.AccessToken, unique: true)
                .Index(t => t.ActiveDeploymentId)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "core.Deployments",
                c => new
                    {
                        DeploymentId = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        PubDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StoredObjectId = c.Guid(nullable: false),
                        CommitMessage = c.String(),
                        Application_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.DeploymentId)
                .ForeignKey("core.Applications", t => t.ApplicationId)
                .ForeignKey("core.StoredObjects", t => t.StoredObjectId)
                .ForeignKey("core.Applications", t => t.Application_Id)
                .Index(t => t.ApplicationId)
                .Index(t => t.StoredObjectId)
                .Index(t => t.Application_Id);
            
            CreateTable(
                "core.Bindings",
                c => new
                    {
                        Hostname = c.String(nullable: false, maxLength: 128),
                        ApplicationId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Hostname)
                .ForeignKey("core.Applications", t => t.ApplicationId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "core.ApplicationTemplates",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        StoredObjectId = c.Guid(nullable: false),
                        LastUpdate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("core.StoredObjects", t => t.StoredObjectId)
                .Index(t => t.StoredObjectId);
            
            CreateTable(
                "core.GlobalFeatureSettings",
                c => new
                    {
                        FeatureId = c.Guid(nullable: false),
                        Key = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.FeatureId, t.Key })
                .ForeignKey("core.Features", t => t.FeatureId)
                .Index(t => t.FeatureId);
            
            CreateTable(
                "core.Features",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false),
                        VersionIdentifier = c.String(nullable: false),
                        Author = c.String(nullable: false),
                        AssemblyName = c.String(),
                        FeatureType = c.Int(nullable: false),
                        ExecutionOrderKey = c.Int(nullable: false),
                        StoredObjectId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.StoredObjects", t => t.StoredObjectId)
                .Index(t => t.Name, unique: true)
                .Index(t => t.StoredObjectId);
            
            CreateTable(
                "core.FeatureApplicationMappings",
                c => new
                    {
                        ApplicationId = c.Guid(nullable: false),
                        FeatureId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationId, t.FeatureId })
                .ForeignKey("core.Applications", t => t.ApplicationId)
                .ForeignKey("core.Features", t => t.FeatureId)
                .Index(t => t.ApplicationId)
                .Index(t => t.FeatureId);
            
            CreateTable(
                "core.ApplicationFeatureSettings",
                c => new
                    {
                        FeatureId = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        Key = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.FeatureId, t.ApplicationId, t.Key })
                .ForeignKey("core.Applications", t => t.ApplicationId)
                .ForeignKey("core.Features", t => t.FeatureId)
                .Index(t => t.FeatureId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "core.WellKnownApplications",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("core.Applications", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("core.WellKnownApplications", "Id", "core.Applications");
            DropForeignKey("core.ApplicationFeatureSettings", "FeatureId", "core.Features");
            DropForeignKey("core.ApplicationFeatureSettings", "ApplicationId", "core.Applications");
            DropForeignKey("core.FeatureApplicationMappings", "FeatureId", "core.Features");
            DropForeignKey("core.FeatureApplicationMappings", "ApplicationId", "core.Applications");
            DropForeignKey("core.GlobalFeatureSettings", "FeatureId", "core.Features");
            DropForeignKey("core.Features", "StoredObjectId", "core.StoredObjects");
            DropForeignKey("core.ApplicationTemplates", "StoredObjectId", "core.StoredObjects");
            DropForeignKey("core.StoredObjects", "ApplicationId", "core.Applications");
            DropForeignKey("core.Deployments", "Application_Id", "core.Applications");
            DropForeignKey("core.Bindings", "ApplicationId", "core.Applications");
            DropForeignKey("core.Applications", "ActiveDeploymentId", "core.Deployments");
            DropForeignKey("core.Deployments", "StoredObjectId", "core.StoredObjects");
            DropForeignKey("core.Deployments", "ApplicationId", "core.Applications");
            DropIndex("core.WellKnownApplications", new[] { "Id" });
            DropIndex("core.ApplicationFeatureSettings", new[] { "ApplicationId" });
            DropIndex("core.ApplicationFeatureSettings", new[] { "FeatureId" });
            DropIndex("core.FeatureApplicationMappings", new[] { "FeatureId" });
            DropIndex("core.FeatureApplicationMappings", new[] { "ApplicationId" });
            DropIndex("core.Features", new[] { "StoredObjectId" });
            DropIndex("core.Features", new[] { "Name" });
            DropIndex("core.GlobalFeatureSettings", new[] { "FeatureId" });
            DropIndex("core.ApplicationTemplates", new[] { "StoredObjectId" });
            DropIndex("core.Bindings", new[] { "ApplicationId" });
            DropIndex("core.Deployments", new[] { "Application_Id" });
            DropIndex("core.Deployments", new[] { "StoredObjectId" });
            DropIndex("core.Deployments", new[] { "ApplicationId" });
            DropIndex("core.Applications", new[] { "Name" });
            DropIndex("core.Applications", new[] { "ActiveDeploymentId" });
            DropIndex("core.Applications", new[] { "AccessToken" });
            DropIndex("core.StoredObjects", new[] { "ApplicationId" });
            DropTable("core.WellKnownApplications");
            DropTable("core.ApplicationFeatureSettings");
            DropTable("core.FeatureApplicationMappings");
            DropTable("core.Features");
            DropTable("core.GlobalFeatureSettings");
            DropTable("core.ApplicationTemplates");
            DropTable("core.Bindings");
            DropTable("core.Deployments");
            DropTable("core.Applications");
            DropTable("core.StoredObjects");
        }
    }
}

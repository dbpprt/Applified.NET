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
    
    public partial class AddedFarmFeatures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "core.ActiveFarmFeatures",
                c => new
                    {
                        FeatureId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.FeatureId)
                .ForeignKey("core.Features", t => t.FeatureId)
                .Index(t => t.FeatureId);
            
            AddColumn("core.Features", "Scope", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("core.ActiveFarmFeatures", "FeatureId", "core.Features");
            DropIndex("core.ActiveFarmFeatures", new[] { "FeatureId" });
            DropColumn("core.Features", "Scope");
            DropTable("core.ActiveFarmFeatures");
        }
    }
}

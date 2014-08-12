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

namespace Applified.IntegratedFeatures.Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "blog.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        ApplicationId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.Name });
            
            CreateTable(
                "blog.PostCategoryMappings",
                c => new
                    {
                        PostId = c.Guid(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        CategoryName = c.String(nullable: false, maxLength: 50),
                        ApplicationId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostId, t.CategoryId, t.CategoryName, t.ApplicationId })
                .ForeignKey("blog.Categories", t => new { t.CategoryId, t.CategoryName }, cascadeDelete: true)
                .ForeignKey("blog.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => new { t.CategoryId, t.CategoryName });
            
            CreateTable(
                "blog.Posts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Author = c.String(nullable: false),
                        Slug = c.String(nullable: false, maxLength: 200),
                        Excerpt = c.String(),
                        Content = c.String(nullable: false),
                        PubDate = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Slug, unique: true);
            
            CreateTable(
                "blog.Comments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Author = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Website = c.String(),
                        Content = c.String(nullable: false),
                        PubDate = c.DateTime(nullable: false),
                        Ip = c.String(nullable: false),
                        UserAgent = c.String(nullable: false),
                        PostId = c.Guid(nullable: false),
                        ApplicationId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("blog.Posts", t => t.PostId, cascadeDelete: true)
                .Index(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("blog.PostCategoryMappings", "PostId", "blog.Posts");
            DropForeignKey("blog.Comments", "PostId", "blog.Posts");
            DropForeignKey("blog.PostCategoryMappings", new[] { "CategoryId", "CategoryName" }, "blog.Categories");
            DropIndex("blog.Comments", new[] { "PostId" });
            DropIndex("blog.Posts", new[] { "Slug" });
            DropIndex("blog.PostCategoryMappings", new[] { "CategoryId", "CategoryName" });
            DropIndex("blog.PostCategoryMappings", new[] { "PostId" });
            DropTable("blog.Comments");
            DropTable("blog.Posts");
            DropTable("blog.PostCategoryMappings");
            DropTable("blog.Categories");
        }
    }
}

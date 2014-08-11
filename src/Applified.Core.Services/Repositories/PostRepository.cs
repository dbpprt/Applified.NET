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
//using System.Linq;
//using Applified.Core.DataAccess.Contracts;
//using PersonalPage.DataAccess;
//using PersonalPage.DataAccess.Contracts;
//using PersonalPage.Model.Contracts;
//using PersonalPage.Model.Entities;
//using PersonalPage.Model.Entities.Blog;

//namespace PersonalPage.Services.Repositories
//{
//    class PostRepository : ApplicationDependantRepository<Post>
//    {
//        public PostRepository(IDbContext context, ICurrentContext currentApplication) 
//            : base(context, currentApplication)
//        {

//        }

//        public override void BeforeUpdate(Post update)
//        {
//            base.BeforeUpdate(update);

//            update.LastModified = DateTime.UtcNow;
//        }

//        public override void BeforeAdd(Post entity)
//        {
//            base.BeforeAdd(entity);

//            entity.LastModified = DateTime.UtcNow;
//        }

//        public override IQueryable<Post> Query()
//        {
//            return base.Query()
//                .Include(entity => entity.PostCategoryMappings)
//                .Include(entity => entity.PostCategoryMappings.Select(inner => inner.Category));
//        }
//    }
//}

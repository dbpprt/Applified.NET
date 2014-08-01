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

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
using System.Linq;
using Applified.Common;
using Applified.Common.Utilities;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Services.Contracts;
using Applified.IntegratedFeatures.Blog.Contracts;
using Applified.IntegratedFeatures.Blog.Entities;

namespace Applified.IntegratedFeatures.Blog.Services
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _context;
        private readonly IRepository<Post> _posts;
        private readonly IRepository<PostCategoryMapping> _postCategoryMappings;
        private readonly IRepository<Category> _categories;
        private readonly IRepository<Comment> _comments;

        public BlogService(
            IUnitOfWork context,
            IRepository<Post> posts,
            IRepository<PostCategoryMapping> postCategoryMappings,
            IRepository<Category> categories,
            IRepository<Comment> comments
            )
        {
            _context = context;
            _posts = posts;
            _postCategoryMappings = postCategoryMappings;
            _categories = categories;
            _comments = comments;
        }

        private void WriteCategories(List<string> categories, Guid postId)
        {
            var existingCategories = GetCategories();
            var addedCategories = new List<Category>();

            addedCategories.AddRange(
                categories.Where(category => existingCategories.All(entity => entity.Name != category))
                    .Select(category => _categories.Insert(
                        new Category
                        {
                            Name = category,
                        })
                    )
                );

            _context.Save();
            var allCategories = existingCategories.Union(addedCategories, new LambdaComparer<Category>((a, b) => a.Id == b.Id && a.Name == b.Name && a.ApplicationId == b.ApplicationId)).ToList();
            var existingMappings = _postCategoryMappings.Query()
                .Where(mapping => mapping.PostId == postId)
                .ToList();

            foreach (var mapping in existingMappings)
            {
                _postCategoryMappings.Delete(mapping);
            }

            var newCategories = allCategories
                .Where(category => categories.Contains(category.Name))
                .ToList();

            _postCategoryMappings.InsertRange(
                newCategories.Select(category => new PostCategoryMapping
                {
                    CategoryId = category.Id,
                    PostId = postId,
                    CategoryName = category.Name
                }), false
            );
        }

        public Post AddPost(Post post, List<string> categories)
        {
            post.Id = Guid.NewGuid();

            if (categories != null)
            {
                WriteCategories(categories, post.Id);
            }

            _posts.Insert(post);
            return post;
        }

        public Post AddPost(Post post)
        {
            return AddPost(post, null);
        }

        public void DeletePost(Guid postId)
        {
            var target = _posts.Query()
                .FirstOrDefault(post => post.Id == postId);
            _posts.Delete(target);
        }

        public void UpdatePost(Post post, List<string> categories)
        {
            if (categories != null)
            {
                WriteCategories(categories, post.Id);
            }

            _posts.Update(post);
        }

        public void UpdatePost(Post post)
        {
            UpdatePost(post, null);
        }

        public List<Post> GetRecentPosts(int count)
        {
            var targets = _posts.Query()
                   .OrderBy(entity => entity.PubDate)
                   .Take(count)
                   .ToList();

            return targets.Select(target => target.UnmapCategories()).ToList();
        }

        public Post GetPost(Guid postId)
        {
            var target = _posts.Query()
                    .FirstOrDefault(post => post.Id == postId);

            return target == null
                ? null
                : target.UnmapCategories();
        }

        public Post GetPost(string slug)
        {
            var target = _posts.Query()
                    .FirstOrDefault(post => post.Slug == slug);

            return target == null
                ? null
                : target.UnmapCategories();
        }

        public List<Category> GetCategories()
        {
            return _categories.Query()
                .ToList();
        }

        public Category AddCategory(Category category)
        {
            return _categories.Insert(category);
        }

        public void DeleteCategory(int categoryId)
        {
            var target = _categories.Query()
                .FirstOrDefault(category => category.Id == categoryId);
            _categories.Delete(target);
        }

        public void DeleteCategory(string name)
        {
            var target = _categories.Query()
                .FirstOrDefault(category => category.Name == name);
            _categories.Delete(target);
        }

        public List<Post> GetPublicPosts(PagingData pagingData)
        {
            var now = DateTime.UtcNow;

            var targets = _posts.Query()
                .Where(entity => entity.PubDate < now)
                .OrderByDescending(entity => entity.PubDate)
                .ToList(pagingData);

            var posts = targets
                .Select(target => target.UnmapCategories());

            return posts.ToList();
        }
    }
}

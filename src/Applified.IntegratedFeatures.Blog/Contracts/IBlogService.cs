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
using Applified.Common;
using Applified.IntegratedFeatures.Blog.Entities;

namespace Applified.IntegratedFeatures.Blog.Contracts
{
    public interface IBlogService
    {
        Post AddPost(Post post, List<string> categories);
        Post AddPost(Post post);
        void DeletePost(Guid postId);
        void UpdatePost(Post post, List<string> categories);
        void UpdatePost(Post post);
        List<Post> GetRecentPosts(int count);
        Post GetPost(Guid postId);
        Post GetPost(string slug);
        List<Category> GetCategories();
        Category AddCategory(Category category);
        void DeleteCategory(int categoryId);
        void DeleteCategory(string name);
        List<Post> GetPublicPosts(PagingData pagingData);
    }
}

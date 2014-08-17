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
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Applified.Common;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.ServiceContracts;
using Applified.IntegratedFeatures.Blog.Contracts;
using Applified.IntegratedFeatures.Blog.Entities;
using Applified.IntegratedFeatures.Blog.ViewModels;
using Applified.IntegratedFeatures.Blog.XmlRpcOwin;
using CookComputing.XmlRpc;
using Microsoft.Owin;
using Owin;

namespace Applified.IntegratedFeatures.Blog.Middlewares
{
    public class MetaweblogMiddleware : OwinXmlRpcService
    {
        private readonly IDependencyResolver _container;
        //private UserManager _userManager;

        public MetaweblogMiddleware(
            OwinMiddleware next,
            IAppBuilder app)
            : base(next, app)
        {
            _container = app.GetApplicationContainer();
        }

        private Author ValidateCredentials(IDependencyScope scope, string username, string password)
        {
            return new Author { DisplayName = "Ich" };
            //  var user = _userManager.FindByName(username);

            //if (user == null)
            //{
            //    throw new XmlRpcException("User not found");
            //}

            //if (_userManager.IsLockedOut(user.Id))
            //{
            //    throw new XmlRpcException("Account is locked out");
            //}

            //if (!_userManager.CheckPassword(user, password))
            //{
            //    throw new XmlRpcException("Password is incorrect");
            //}

            //if (!_userManager.IsInRole(user.Id, WellKnownRoles.BlogAuthors) &&
            //    !_userManager.IsInRole(user.Id, WellKnownRoles.Administrators))
            //{
            //    throw new XmlRpcException("Access denied");
            //}

            //var author = new Author
            //{
            //    DisplayName = user.UserName
            //};

            //return author;
        }

        public override string AddPost(string blogid, string username, string password, Post post, bool publish)
        {
            using (var scope = _container.BeginScope())
            {
                var blogService = scope.Resolve<IBlogService>();

                var author = ValidateCredentials(scope, username, password);

                if (publish) post.PubDate = DateTime.UtcNow;

                post.Author = author.DisplayName;

                blogService.AddPost(post, post.Categories.ToList());

                return post.Id.ToString();
            }
        }

        public override bool UpdatePost(string postid, string username, string password, Post post, bool publish)
        {
            using (var scope = _container.BeginScope())
            {
                var blogService = scope.Resolve<IBlogService>();

                var author = ValidateCredentials(scope, username, password);

                Guid postId;
                if (!Guid.TryParse(postid, out postId))
                    throw new XmlRpcFaultException(0, "post does not exists");

                if (publish && post.PubDate == DateTime.MinValue)
                {
                    post.PubDate = DateTime.UtcNow;
                }

                post.Author = author.DisplayName;
                post.Id = postId;

                blogService.UpdatePost(post, post.Categories.ToList());
                return true;
            }
        }

        public override object GetPost(string postid, string username, string password)
        {
            using (var scope = _container.BeginScope())
            {
                var blogService = scope.Resolve<IBlogService>();

                var author = ValidateCredentials(scope, username, password);

                Guid postId;
                if (!Guid.TryParse(postid, out postId))
                    throw new XmlRpcFaultException(0, "post does not exists");

                var target = blogService.GetPost(postId);

                if (target == null)
                    throw new XmlRpcFaultException(0, "post does not exists");

                var result = target
                    .ToMetaObject();

                return result;
            }
        }

        public override object[] GetCategories(string blogid, string username, string password)
        {
            using (var scope = _container.BeginScope())
            {
                var blogService = scope.Resolve<IBlogService>();

                var author = ValidateCredentials(scope, username, password);

                return blogService
                    .GetCategories()
                    .Select(category => new
                    {
                        title = category.Name
                    })
                    .ToArray();
            }
        }

        public override object[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            using (var scope = _container.BeginScope())
            {
                var blogService = scope.Resolve<IBlogService>();

                var author = ValidateCredentials(scope, username, password);

                return blogService.GetRecentPosts(numberOfPosts).Select(target => target.ToMetaObject()).ToArray();
            }

        }

        public override object NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
        {
            using (var scope = _container.BeginScope())
            {
                var blogService = scope.Resolve<IBlogService>();
                var storageService = scope.Resolve<IStorageService>();
                var urlBuilderService = scope.Resolve<IUrlBuilderService>();

                var author = ValidateCredentials(scope, username, password);

                var id = storageService.StoreObject(mediaObject.name, mediaObject.bits, mediaObject.type);
                var url = urlBuilderService.GetStoredObjectUrl(id);

                return new { url = url };
            }
        }

        public override bool DeletePost(string key, string postid, string username, string password, bool publish)
        {
            using (var scope = _container.BeginScope())
            {
                var blogService = scope.Resolve<IBlogService>();

                var author = ValidateCredentials(scope, username, password);

                Guid postId;
                if (!Guid.TryParse(postid, out postId))
                    return false;

                blogService.DeletePost(postId);
                return true;
            }
        }

        public override object[] GetUsersBlogs(string key, string username, string password)
        {
            using (var scope = _container.BeginScope())
            {
                var blogService = scope.Resolve<IBlogService>();

                var author = ValidateCredentials(scope, username, password);

                return new object[]
                {
                    // TODO: getUserBlogs
                    new
                    {
                        blogid = 1,
                        blogName = "Blog",
                        url = ""
                    }
                };
            }
        }
    }
}

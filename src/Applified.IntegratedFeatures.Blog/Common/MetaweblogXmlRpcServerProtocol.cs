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

using Applified.IntegratedFeatures.Blog.Contracts;
using Applified.IntegratedFeatures.Blog.Entities;
using Applified.IntegratedFeatures.Blog.ViewModels;
using CookComputing.XmlRpc;

namespace Applified.IntegratedFeatures.Blog.Common
{
    class MetaweblogXmlRpcServerProtocol : XmlRpcHttpServerProtocol, IMetaWeblog
    {
        private readonly IMetaWeblog _service;

        public MetaweblogXmlRpcServerProtocol(IMetaWeblog service)
        {
            _service = service;
        }

        public string AddPost(string blogid, string username, string password, Post post, bool publish)
        {
            return _service.AddPost(blogid, username, password, post, publish);
        }

        public bool UpdatePost(string postid, string username, string password, Post post, bool publish)
        {
            return _service.UpdatePost(postid, username, password, post, publish);
        }

        public object GetPost(string postid, string username, string password)
        {
            return _service.GetPost(postid, username, password);
        }

        public object[] GetCategories(string blogid, string username, string password)
        {
            return _service.GetCategories(blogid, username, password);
        }

        public object[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            return _service.GetRecentPosts(blogid, username, password, numberOfPosts);
        }

        public object NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
        {
            return _service.NewMediaObject(blogid, username, password, mediaObject);
        }

        public bool DeletePost(string key, string postid, string username, string password, bool publish)
        {
            return _service.DeletePost(key, postid, username, password, publish);
        }

        public object[] GetUsersBlogs(string key, string username, string password)
        {
            return _service.GetUsersBlogs(key, username, password);
        }
    }
}

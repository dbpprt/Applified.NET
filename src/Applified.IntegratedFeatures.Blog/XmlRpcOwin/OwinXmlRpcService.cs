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
using System.Threading.Tasks;
using Applified.Common;
using Applified.IntegratedFeatures.Blog.Common;
using Applified.IntegratedFeatures.Blog.Contracts;
using Applified.IntegratedFeatures.Blog.Entities;
using Applified.IntegratedFeatures.Blog.ViewModels;
using CookComputing.XmlRpc;
using Microsoft.Owin;
using Owin;

namespace Applified.IntegratedFeatures.Blog.XmlRpcOwin
{
    public abstract class OwinXmlRpcService : OwinMiddlewareBase, IMetaWeblog
    {
        private readonly MetaweblogXmlRpcServerProtocol _rpcHttpServerProtocol;

        protected OwinXmlRpcService(OwinMiddleware next, IAppBuilder app)
            : base(next, app)
        {
            _rpcHttpServerProtocol = new MetaweblogXmlRpcServerProtocol(this);
        }

        public override Task Invoke(IOwinContext context)
        {
            try
            {
                _rpcHttpServerProtocol.HandleHttpRequest(new OwinXmlRpcHttpRequest(context.Request),
                    new OwinXmlRpcHttpResponse(context.Response));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
            }

            return Task.FromResult(0);
        }

        public abstract string AddPost(string blogid, string username, string password, Post post, bool publish);
        public abstract bool UpdatePost(string postid, string username, string password, Post post, bool publish);
        public abstract object GetPost(string postid, string username, string password);
        public abstract object[] GetCategories(string blogid, string username, string password);
        public abstract object[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts);
        public abstract object NewMediaObject(string blogid, string username, string password, MediaObject mediaObject);
        public abstract bool DeletePost(string key, string postid, string username, string password, bool publish);
        public abstract object[] GetUsersBlogs(string key, string username, string password);
    }
}

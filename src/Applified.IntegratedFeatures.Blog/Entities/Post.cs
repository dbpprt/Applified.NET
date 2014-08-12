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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Applified.Core.Entities.Contracts;
using Applified.Core.Entities.Infrastructure;

namespace Applified.IntegratedFeatures.Blog.Entities
{
    //[XmlRpcMissingMapping(MappingAction.Ignore)]
    public class Post : IApplicationDependant
    {
        public Post()
        {

        }

        [Required]
        public Guid ApplicationId { get; set; }

        //[XmlRpcMember("postid")]
        [Key]
        public Guid Id { get; set; }

        //[XmlRpcMember("title")]
        [Required]
        public string Title { get; set; }

        //[XmlRpcMember("author")]
        [Required]
        public string Author { get; set; }

        //[XmlRpcMember("wp_slug")]
        [Required]
        [Index(IsUnique = true)]
        [MaxLength(200)]
        public string Slug { get; set; }

        //[XmlRpcMember("mt_excerpt")]
        //[Required]
        public string Excerpt { get; set; }

        //[XmlRpcMember("description")]
        [Required]
        public string Content { get; set; }

        //[XmlRpcMember("dateCreated")]
        [Required]
        public DateTime PubDate { get; set; }

        //[XmlRpcMember("dateModified")]
        [Required]
        public DateTime LastModified { get; set; }

        [NotMapped]
        public bool IsPublished {
            get { return PubDate > DateTime.UtcNow; } 
        }

        //[XmlRpcMember("categories")]
        [NotMapped]
        public string[] Categories { get; set; }

        public ICollection<Comment> Comments { get; private set; }

        public ICollection<PostCategoryMapping> PostCategoryMappings { get; set; }

        public object ToMetaObject()
        {
            return new
            {
                description = Content,
                title = Title,
                dateCreated = PubDate,
                wp_slug = Slug,
                categories = Categories,
                postid = Id.ToString()
            };
        }

        public Post UnmapCategories()
        {
            Categories = PostCategoryMappings != null
                ? PostCategoryMappings.Select(mapping => mapping.Category.Name).ToArray()
                : new List<string>().ToArray();
            return this;
        }
        
        //public Uri AbsoluteUrl
        //{
        //    get
        //    {
        //        Uri requestUrl = HttpContext.Current.Request.Url;
        //        return new Uri(requestUrl.Scheme + "://" + requestUrl.Authority + Url, UriKind.Absolute);
        //    }
        //}

        //public Uri Url
        //{
        //    get { return new Uri(VirtualPathUtility.ToAbsolute("~/post/" + Slug), UriKind.Relative); }
        //}

        //public bool AreCommentsOpen(HttpContextBase context)
        //{
        //    return PubDate > DateTime.UtcNow.AddDays(-Blog.DaysToComment) || context.User.Identity.IsAuthenticated;
        //}

        //public int CountApprovedComments(HttpContextBase context)
        //{
        //    return (Blog.ModerateComments && !context.User.Identity.IsAuthenticated)
        //        ? this.Comments.Count(c => c.IsApproved)
        //        : this.Comments.Count;
        //}

        //public string GetHtmlContent()
        //{
        //    string result = Content;

        //    // Youtube content embedded using this syntax: [youtube:xyzAbc123]
        //    var video =
        //        "<div class=\"video\"><iframe src=\"//www.youtube.com/embed/{0}?modestbranding=1&amp;theme=light\" allowfullscreen></iframe></div>";
        //    result = Regex.Replace(result, @"\[youtube:(.*?)\]", (Match m) => string.Format(video, m.Groups[1].Value));

        //    // Images replaced by CDN paths if they are located in the /posts/ folder
        //    var cdn = ConfigurationManager.AppSettings.Get("blog:cdnUrl");
        //    result = Regex.Replace(result, "<img.*?src=\"([^\"]+)\"", (Match m) =>
        //    {
        //        string src = m.Groups[1].Value;
        //        int index = src.IndexOf("/posts/");

        //        if (index > -1)
        //        {
        //            string clean = src.Substring(index);
        //            return m.Value.Replace(src, cdn + clean);
        //        }

        //        return m.Value;
        //    });

        //    return result;
        //}
    }
}

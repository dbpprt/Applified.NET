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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Applified.Core.Entities.Contracts;
using Applified.Core.Entities.Infrastructure;

namespace Applified.IntegratedFeatures.Blog.Entities
{
    public class Comment : IApplicationDependant
    {
        public Comment()
        {
            PubDate = DateTime.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Email { get; set; }
        
        
        public string Website { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime PubDate { get; set; }
        
        [Required]
        public string Ip { get; set; }
        
        [Required]
        public string UserAgent { get; set; }

        [Required]
        public Guid PostId { get; set; }
        
        [ForeignKey("PostId")]
        public Post Post { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        //public bool IsAdmin { get; set; }
        //public bool IsApproved { get; set; }

        //public string GravatarUrl(int size)
        //{
        //    var hash = FormsAuthentication.HashPasswordForStoringInConfigFile(Email.ToLowerInvariant(), "MD5").ToLower();

        //    return string.Format("http://gravatar.com/avatar/{0}?s={1}&d=mm", hash, size);
        //}

        //public string ContentWithLinks()
        //{
        //    return _linkRegex.Replace(Content, new MatchEvaluator(Evaluator));
        //}

        //private static string Evaluator(Match match)
        //{
        //    var info = CultureInfo.InvariantCulture;
        //    return string.Format(info, Link, !match.Value.Contains("://") ? "http://" : string.Empty, match.Value,
        //        ShortenUrl(match.Value, 50));
        //}

        //private static string ShortenUrl(string url, int max)
        //{
        //    if (url.Length <= max)
        //    {
        //        return url;
        //    }

        //    // Remove the protocal
        //    var startIndex = url.IndexOf("://");
        //    if (startIndex > -1)
        //    {
        //        url = url.Substring(startIndex + 3);
        //    }

        //    if (url.Length <= max)
        //    {
        //        return url;
        //    }

        //    // Compress folder structure
        //    var firstIndex = url.IndexOf("/") + 1;
        //    var lastIndex = url.LastIndexOf("/");
        //    if (firstIndex < lastIndex)
        //    {
        //        url = url.Remove(firstIndex, lastIndex - firstIndex);
        //        url = url.Insert(firstIndex, "...");
        //    }

        //    if (url.Length <= max)
        //    {
        //        return url;
        //    }

        //    // Remove URL parameters
        //    var queryIndex = url.IndexOf("?");
        //    if (queryIndex > -1)
        //    {
        //        url = url.Substring(0, queryIndex);
        //    }

        //    if (url.Length <= max)
        //    {
        //        return url;
        //    }

        //    // Remove URL fragment
        //    var fragmentIndex = url.IndexOf("#");
        //    if (fragmentIndex > -1)
        //    {
        //        url = url.Substring(0, fragmentIndex);
        //    }

        //    if (url.Length <= max)
        //    {
        //        return url;
        //    }

        //    // Compress page
        //    firstIndex = url.LastIndexOf("/") + 1;
        //    lastIndex = url.LastIndexOf(".");
        //    if (lastIndex - firstIndex > 10)
        //    {
        //        var page = url.Substring(firstIndex, lastIndex - firstIndex);
        //        var length = url.Length - max + 3;
        //        if (page.Length > length)
        //        {
        //            url = url.Replace(page, string.Format("...{0}", page.Substring(length)));
        //        }
        //    }

        //    return url;
        //}
    }
}

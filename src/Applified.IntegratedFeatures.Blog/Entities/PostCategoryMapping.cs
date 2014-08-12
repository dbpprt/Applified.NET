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
    public class PostCategoryMapping : IApplicationDependant
    {
        [Key, Column(Order = 0)]
        public Guid PostId { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }

        [Key, Column(Order = 1)]
        public int CategoryId { get; set; }

        [Key, Column(Order = 2)]
        public string CategoryName { get; set; }

        //[ForeignKey("")]
        public Category Category { get; set; }

        [Required]
        [Key, Column(Order = 3)]
        public Guid ApplicationId { get; set; }
    }
}

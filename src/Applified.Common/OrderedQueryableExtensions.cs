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

using System.Collections.Generic;
using System.Linq;

namespace Applified.Common
{
    public static class OrderedQueryableExtensions
    {
        public static List<TEntity> ToList<TEntity>(this IOrderedQueryable<TEntity> query, PagingData pagingData)
        {
            var pageSize = pagingData.PageSize + 1;
            pagingData.HasPreviousPage = pagingData.Page > 1;

            var result = query.Skip((pagingData.Page - 1) * pagingData.PageSize).Take(pageSize).ToList();
            if (pageSize == result.Count())
            {
                pagingData.HasNextPage = true;
                return result.Take(pagingData.PageSize).ToList();
            }
            pagingData.HasNextPage = false;
            return result;
        }

    }
}

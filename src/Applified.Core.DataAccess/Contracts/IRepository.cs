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
using System.Threading.Tasks;

namespace Applified.Core.DataAccess.Contracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Update(TEntity entity, bool saveChanges = true);
        void Delete(TEntity entity, bool saveChanges = true);
        TEntity Insert(TEntity entity, bool saveChanges = true);
        void InsertRange(IEnumerable<TEntity> entities, bool saveChanges = true);
        Task UpdateAsync(TEntity entity, bool saveChanges = true);
        Task DeleteAsync(TEntity entity, bool saveChanges = true);
        Task<TEntity> InsertAsync(TEntity entity, bool saveChanges = true);
        Task InsertRangeAsync(IEnumerable<TEntity> entities, bool saveChanges = true);
        IQueryable<TEntity> Query();
    }
}

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

using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Applified.Core.DataAccess.Contracts
{
    public interface IDbContext
    {
        IDbSet<T> Set<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void SetState(object o, EntityState state);
        EntityState GetState(object o);
        void Dispose();
        void UseTransaction(DbTransaction transaction);
        DbContextTransaction BeginTransaction(IsolationLevel isolationLevel);
    }
}

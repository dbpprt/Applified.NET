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
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Contracts;

namespace Applified.Core.DataAccess
{

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IDbContext Context;
        protected readonly IDbSet<TEntity> DbSet;

        public virtual void BeforeAdd(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var keyEntity = entity as IGuidKey;
            if (keyEntity != null && keyEntity.Id == Guid.Empty)
            {
                keyEntity.Id = Guid.NewGuid();
            }
        }

        public virtual void BeforeUpdate(TEntity update)
        {
            if (update == null)
                throw new ArgumentNullException("update");
        }

        public virtual void BeforeDelete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
        }

        public virtual void AfterAdd(TEntity entity) { }
        public virtual void AfterUpdate(TEntity update) { }
        public virtual void AfterDelete(TEntity entity) { }

        public Repository(IDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual void Update(TEntity entity, bool saveChanges = true)
        {
            BeforeUpdate(entity);
            DbSet.Attach(entity);
            Context.SetState(entity, EntityState.Modified);

            if (saveChanges)
            {
                Context.SaveChanges();
                AfterUpdate(entity);
            }
        }

        public virtual void Delete(TEntity entity, bool saveChanges = true)
        {
            BeforeDelete(entity);
            DbSet.Attach(entity);
            DbSet.Remove(entity);

            if (saveChanges)
            {
                Context.SaveChanges();
                AfterDelete(entity);
            }
        }

        public virtual TEntity Insert(TEntity entity, bool saveChanges = true)
        {
            BeforeAdd(entity);
            DbSet.Attach(entity);
            Context.SetState(entity, EntityState.Added);

            if (saveChanges)
            {
                Context.SaveChanges();
                AfterDelete(entity);
            }

            return entity;
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities, bool saveChanges = true)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            entities.ToList().ForEach(entity => Insert(entity, saveChanges));
        }

        public virtual async Task UpdateAsync(TEntity entity, bool saveChanges = true)
        {
            BeforeUpdate(entity);
            DbSet.Attach(entity);
            Context.SetState(entity, EntityState.Modified);

            if (saveChanges)
            {
                await Context.SaveChangesAsync().ConfigureAwait(false); ;
                AfterUpdate(entity);
            }
        }

        public virtual async Task DeleteAsync(TEntity entity, bool saveChanges = true)
        {
            BeforeDelete(entity);
            DbSet.Attach(entity);
            DbSet.Remove(entity);

            if (saveChanges)
            {
                await Context.SaveChangesAsync().ConfigureAwait(false); ;
                AfterDelete(entity);
            }
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity, bool saveChanges = true)
        {
            BeforeAdd(entity);
            DbSet.Attach(entity);
            Context.SetState(entity, EntityState.Added);

            if (saveChanges)
            {
                await Context.SaveChangesAsync().ConfigureAwait(false); ;
                AfterAdd(entity);
            }

            return entity;
        }

        public virtual Task InsertRangeAsync(IEnumerable<TEntity> entities, bool saveChanges = true)
        {
            var tasks = new List<Task>();
            entities.ToList().ForEach(entity => tasks.Add(InsertAsync(entity, saveChanges)));
            return Task.WhenAll(tasks);
        }

        public virtual IQueryable<TEntity> Query()
        {
            return DbSet;
        } 
    }
}

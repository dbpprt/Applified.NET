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

using System.Linq;
using Applified.Core.DataAccess;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Contracts;
using Applified.Core.ServiceContracts;

namespace Applified.Core.Services.Repositories
{
    public class ApplicationDependantRepository<TEntity> : Repository<TEntity> where TEntity : class, IApplicationDependant
    {
        private readonly ICurrentContext _currentContext;

        public ApplicationDependantRepository(
            IDbContext context,
            ICurrentContext currentContext
            ) 
            : base(context)
        {
            _currentContext = currentContext;
        }

        public override void BeforeAdd(TEntity entity)
        {
            base.BeforeAdd(entity);

            entity.ApplicationId = _currentContext.ApplicationId;
        }

        public override void BeforeUpdate(TEntity update)
        {
            base.BeforeUpdate(update);

            update.ApplicationId = _currentContext.ApplicationId;
        }

        public override void BeforeDelete(TEntity entity)
        {
            entity.ApplicationId = _currentContext.ApplicationId;

            base.BeforeDelete(entity);
        }

        public override IQueryable<TEntity> Query()
        {
            return base.Query()
                .Where(entity => entity.ApplicationId == _currentContext.ApplicationId);
        }
    }
}

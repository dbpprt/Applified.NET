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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.ServiceContracts;
using Applified.Core.Services.Contracts;

namespace Applified.Core.Services.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IRepository<Application> _applications;
        private readonly IRepository<Binding> _bindings;
        private readonly IUnitOfWork _context;

        public ApplicationService(
            IRepository<Application> applications,
            IRepository<Binding> bindings,
            IUnitOfWork context
            )
        {
            _applications = applications;
            _bindings = bindings;
            _context = context;
        }

        public Task<Application> FindApplication(string nameOrGuid)
        {
            Guid featureId;

            if (Guid.TryParse(nameOrGuid, out featureId))
            {
                return GetApplicationAsync(featureId);
            }

            return _applications.Query()
                .FirstOrDefaultAsync(entity => entity.Name == nameOrGuid);
        }

        public Task<Application> CreateApplicationAsync(Application application)
        {
            return _applications.InsertAsync(application);
        }

        public Task<Application> GetApplicationAsync(Guid applicationId)
        {
            return _applications.Query()
                .FirstOrDefaultAsync(entity => entity.Id == applicationId);
        }

        public Task<List<Application>> GetApplicationsAsync()
        {
            return _applications.Query()
                .Include(entity => entity.Bindings)
                .ToListAsync();
        }

        public Task<List<Binding>> GetBindingsAsync()
        {
            return _bindings.Query()
                .ToListAsync();
        }

        public Task AddBindingAsync(string binding)
        {
            var entity = new Binding
            {
                Hostname = binding
            };

            return _bindings.InsertAsync(entity);
        }

        public Task DeleteBindingAsync(string binding)
        {
            throw new NotImplementedException();
        }

        public Task UpdateApplication(Application application)
        {
            throw new NotImplementedException();
        }
    }
}

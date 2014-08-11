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
using System.Threading.Tasks;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.ServiceContracts;

namespace Applified.Core.Services.Services
{
    public class DeploymentService : IDeploymentService
    {
        private readonly IRepository<Deployment> _deployments;
        private readonly IRepository<Application> _applications;
        private readonly IStorageService _storageService;
        private readonly ICurrentContext _currentContext;
        private readonly INativeRepository<Deployment> _nativeDeployments;

        public DeploymentService(
            IRepository<Deployment> deployments,
            IRepository<Application> applications,
            IStorageService storageService,
            ICurrentContext currentContext,
            INativeRepository<Deployment> nativeDeployments 
            )
        {
            _deployments = deployments;
            _applications = applications;
            _storageService = storageService;
            _currentContext = currentContext;
            _nativeDeployments = nativeDeployments;
        }

        private void EnsureAccess()
        {
            if (!_currentContext.IsAdmin)
            {
                throw new UnauthorizedAccessException();
            }   
        }

        private IQueryable<Deployment> SafeQuery()
        {
            EnsureAccess();

            return _deployments.Query()
                .Include(entity => entity.Application)
                .Where(deployment => deployment.ApplicationId == _currentContext.ApplicationId);
        }

        public async Task<Guid> AddDeploymentAsync(StoredObject storedObject, Deployment deployment, bool setActive = false)
        {
            EnsureAccess();

            var currentApplicationId = _currentContext.ApplicationId;
            var storedObjectId = await _storageService
                .StoreObjectAsync(storedObject.Name, storedObject.Data)
                .ConfigureAwait(false);

            deployment.DeploymentId = Guid.NewGuid();
            deployment.ApplicationId = currentApplicationId;
            deployment.StoredObjectId = storedObjectId;

            if (setActive)
            {
                var entity = await _applications.Query()
                    .FirstAsync(application => application.Id == currentApplicationId)
                    .ConfigureAwait(false);

                entity.ActiveDeploymentId = deployment.DeploymentId;
                _applications.Update(entity, false);
            }

            await _deployments.InsertAsync(deployment).ConfigureAwait(false); ;

            return deployment.DeploymentId;
        }

        public Task<List<Deployment>> GetDeploymentsAsync()
        {
            return SafeQuery()
                .ToListAsync();
        }

        public Task<Deployment> GetDeploymentAsync(Guid deploymentId)
        {
            return SafeQuery()
                .FirstOrDefaultAsync(deployment => deployment.DeploymentId == deploymentId);
        }

        public async Task SetDeploymentActiveAsync(Guid deployment)
        {
            var target = await SafeQuery()
                .FirstOrDefaultAsync(entity => entity.DeploymentId == deployment)
                .ConfigureAwait(false); ;

            if (target == null)
            {
                throw new ArgumentException("deployment");
            }

            var application = target.Application;
            application.ActiveDeploymentId = deployment;

            await _applications.UpdateAsync(application).ConfigureAwait(false); ;
        }

        public async Task SetCommitMessageAsync(Guid deploymentId, string commitMessage)
        {
            var target = await SafeQuery()
                .FirstOrDefaultAsync(entity => entity.DeploymentId == deploymentId)
                .ConfigureAwait(false); ;

            if (target == null)
            {
                throw new ArgumentException("deployment");
            }

            target.CommitMessage = commitMessage;

            await _deployments.UpdateAsync(target).ConfigureAwait(false);
        }

        public Task<StoredObject> GetDeploymentPayloadInternalAsync(Guid deploymentId)
        {
            // TODO: Is it okay to skip application authorization here?

            var target = _nativeDeployments.Query()
                .Include(entity => entity.StoredObject)
                .Where(entity => entity.DeploymentId == deploymentId)
                .Select(entity => entity.StoredObject)
                .FirstOrDefaultAsync();

            return target;
        }
    }
}

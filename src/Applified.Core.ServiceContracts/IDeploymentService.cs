using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.ServiceContracts
{
    public interface IDeploymentService
    {
        Task<Guid> AddDeploymentAsync(StoredObject storedObject, Deployment deployment,
            bool setActive = false);
        Task<List<Deployment>> GetDeploymentsAsync();
        Task<Deployment> GetDeploymentAsync(Guid deploymentId);
        Task SetDeploymentActiveAsync(Guid deployment);
        Task SetCommitMessageAsync(Guid deploymentId, string commitMessage);
        Task<StoredObject> GetDeploymentPayloadInternalAsync(Guid deploymentId);

    }
}

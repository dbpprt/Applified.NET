using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.ServiceContracts
{
    public interface IApplicationService
    {
        Task<Application> FindApplication(string nameOrGuid);

        Task<Application> CreateApplicationAsync(Application application);

        Task<Application> GetApplicationAsync(Guid applicationId);

        Task<List<Application>> GetApplicationsAsync();

        Task<List<Binding>> GetBindingsAsync();

        Task AddBindingAsync(string binding);

        Task DeleteBindingAsync(string binding);

        Task UpdateApplication(Application application);
    }
}

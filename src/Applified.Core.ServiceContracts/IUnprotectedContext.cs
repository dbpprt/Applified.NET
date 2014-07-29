using System;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.ServiceContracts
{
    public interface IUnprotectedContext
    {
        void SetDeploymentToServe(Guid? guid);
        void SetIsAdmin(bool isAdmin);
        void SetCurrentApplication(Application application);
    }
}
using System;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.ServiceContracts
{
    public interface IUnprotectedContext
    {
        bool IsAdmin { get; set; }

        Guid ApplicationId { get; set; }

        Guid? DeploymentId { get; set; }

        string BaseDirectory { get; set; }

    }
}
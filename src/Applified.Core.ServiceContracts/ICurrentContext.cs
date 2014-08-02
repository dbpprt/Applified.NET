using System;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.ServiceContracts
{
    public interface ICurrentContext
    {
        bool IsAdmin { get; }

        Guid ApplicationId { get; }

        Guid? DeploymentId { get; }

        string BaseDirectory { get; }
    }
}

using System;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.ServiceContracts
{
    public interface ICurrentApplication
    {
        bool IsAdmin { get; }
        Application Application { get; }
        Guid? DeploymentToServe { get; }
    }
}

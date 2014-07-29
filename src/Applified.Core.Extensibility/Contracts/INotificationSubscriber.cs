using System;

namespace Applified.Core.Extensibility.Contracts
{
    public interface INotificationSubscriber
    {
        void OnNewDeployment(Guid deploymentId);
    }
}

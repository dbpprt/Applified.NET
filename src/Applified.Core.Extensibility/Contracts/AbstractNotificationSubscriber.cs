using System;

namespace Applified.Core.Extensibility.Contracts
{
    public class AbstractNotificationSubscriber : INotificationSubscriber
    {
        public virtual void OnNewDeployment(Guid deploymentId) { }
    }
}

using System;
using System.Threading.Tasks;

namespace Applified.Core.Extensibility.Contracts
{
    public class AbstractNotificationSubscriber : INotificationSubscriber
    {
        public virtual Task OnNewDeployment(Guid deploymentId)
        {
            return Task.FromResult(0);
        }
    }
}

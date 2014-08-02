using System;
using System.Threading.Tasks;

namespace Applified.Core.Extensibility.Contracts
{
    public interface INotificationSubscriber
    {
        Task OnNewDeployment(Guid deploymentId);
    }
}

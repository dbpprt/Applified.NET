using System.Threading.Tasks;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.Extensibility.Contracts
{
    public interface IPostDeploymentAction
    {
        Task ExcecuteAsync(Deployment deployment);
    }
}

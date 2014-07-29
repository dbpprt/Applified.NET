using System.Threading.Tasks;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.Extensibility.Contracts
{
    public interface IPostApplicationCreationStep
    {
        Task ExcecuteAsync(Application application);
    }
}

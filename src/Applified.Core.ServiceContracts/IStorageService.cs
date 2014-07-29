using System;
using System.Threading.Tasks;
using Applified.Core.Entities.Infrastructure;

namespace Applified.Core.ServiceContracts
{
    public interface IStorageService
    {
        Guid StoreObject(string name, byte[] data, string type = null);
        StoredObject GetObject(Guid id);
        Task<Guid> StoreObjectAsync(string name, byte[] data, string type = null);
    }
}

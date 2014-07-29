using System;
using System.Linq;
using System.Threading.Tasks;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.ServiceContracts;

namespace Applified.Core.Services.Services
{
    public class DatabaseStorageService : IStorageService
    {
        private readonly IRepository<StoredObject> _storedObjects;

        public DatabaseStorageService(
            IRepository<StoredObject> storedObjects
            )
        {
            _storedObjects = storedObjects;
        }

        public Guid StoreObject(string name, byte[] data, string type)
        {
            var obj = new StoredObject
            {
                Data = data,
                Id = Guid.NewGuid(),
                Type = type,
                Name = name,
                Size = data.Length
            };

            _storedObjects.Insert(obj);
            return obj.Id;
        }

        public StoredObject GetObject(Guid id)
        {
            return _storedObjects.Query()
                .FirstOrDefault(obj => obj.Id == id);
        }

        public async Task<Guid> StoreObjectAsync(string name, byte[] data, string type = null)
        {
            var obj = new StoredObject
            {
                Data = data,
                Id = Guid.NewGuid(),
                Type = type,
                Name = name,
                Size = data.Length
            };

            await _storedObjects.InsertAsync(obj);
            return obj.Id;
        }
    }
}

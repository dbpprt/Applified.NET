#region Copyright (C) 2014 Applified.NET 
// Copyright (C) 2014 Applified.NET
// http://www.applified.net

// This file is part of Applified.NET.

// Applified.NET is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.

// You should have received a copy of the GNU Affero General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion

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

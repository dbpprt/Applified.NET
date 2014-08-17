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
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.Extensibility;
using Applified.Core.ServiceContracts;
using Applified.Core.Services.Contracts;

namespace Applified.Core.Services.Services
{
    public class SetupService : ISetupService
    {
        private readonly IServerEnvironment _serverEnvironment;
        private readonly IRepository<Feature> _features;
        private readonly IUnitOfWork _context;

        [ImportMany(typeof(IntegratedFeatureBase))]
        private IntegratedFeatureBase[] _integratedFeatures = null;

        public SetupService(
            IServerEnvironment serverEnvironment,
            IRepository<Feature> features,
            IUnitOfWork context
            )
        {
            _serverEnvironment = serverEnvironment;
            _features = features;
            _context = context;
        }

        public async Task InitializeIntegratedFeatures(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                directory = _serverEnvironment.ApplicationBaseDirectory;
            var baseDirectory = directory;

            // TODO: move search pattern to config.. just in case
            var catalog = new DirectoryCatalog(baseDirectory, "*.IntegratedFeatures.*.dll");
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);

            var loadedFeatures = _integratedFeatures.ToList();

            var existingFeatures = await _features.Query()
                .Where(entity => entity.FeatureType == FeatureType.Integrated)
                .ToListAsync();

            foreach (var loadedFeature in loadedFeatures)
            {
                var existing = existingFeatures.FirstOrDefault(entity => entity.Id == loadedFeature.FeatureId);

                if (existing == null)
                {
                    var newFeature = new Feature
                    {
                        Author = loadedFeature.Author,
                        Description = loadedFeature.Description,
                        FeatureType = FeatureType.Integrated,
                        AssemblyName = loadedFeature.AssemblyName,
                        Name = loadedFeature.Name,
                        Id = loadedFeature.FeatureId,
                        VersionIdentifier = loadedFeature.Version,
                        StoredObjectId = null,
                        StoredObject = null,
                        ExecutionOrderKey = loadedFeature.ExecutionOrderKey,
                        Scope = loadedFeature.Scope
                    };

                    _features.Insert(newFeature, false);
                }
                else
                {
                    existing.Author = loadedFeature.Author;
                    existing.Description = loadedFeature.Description;
                    existing.Name = loadedFeature.Name;
                    existing.VersionIdentifier = loadedFeature.Version;
                    existing.StoredObjectId = null;
                    existing.AssemblyName = loadedFeature.AssemblyName;
                    existing.ExecutionOrderKey = loadedFeature.ExecutionOrderKey;
                    existing.Scope = loadedFeature.Scope;

                    _features.Update(existing, false);
                }
            }

            await _context.SaveAsync();
        }
    }
}

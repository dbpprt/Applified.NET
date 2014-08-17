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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Applified.Common.Exceptions;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.Extensibility;
using Applified.Core.ServiceContracts;
using Applified.Core.Services.Contracts;
using Newtonsoft.Json;

namespace Applified.Core.Services.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly IRepository<Feature> _features;
        private readonly IRepository<FeatureApplicationMapping> _featureApplicationMappings;
        private readonly IRepository<GlobalFeatureSetting> _globalFeatureSettings;
        private readonly IRepository<ApplicationFeatureSetting> _applicationFeatureSettings;
        private readonly IRepository<ActiveFarmFeature> _activatedFarmFeatures;
        private readonly IStorageService _storageService;
        private readonly IServerEnvironment _serverEnvironment;
        private readonly IUnitOfWork _context;

        public FeatureService(
            IRepository<Feature> features,
            IRepository<FeatureApplicationMapping> featureApplicationMappings,
            IRepository<GlobalFeatureSetting> globalFeatureSettings,
            IRepository<ApplicationFeatureSetting> applicationFeatureSettings,
            IRepository<ActiveFarmFeature> activatedFarmFeatures,
            IStorageService storageService,
            IServerEnvironment serverEnvironment,
            IUnitOfWork context
            )
        {
            _features = features;
            _featureApplicationMappings = featureApplicationMappings;
            _globalFeatureSettings = globalFeatureSettings;
            _applicationFeatureSettings = applicationFeatureSettings;
            _activatedFarmFeatures = activatedFarmFeatures;
            _storageService = storageService;
            _serverEnvironment = serverEnvironment;
            _context = context;
        }

        public Task<Feature> FindFeature(string nameOrGuid)
        {
            Guid featureId;

            if (Guid.TryParse(nameOrGuid, out featureId))
            {
                return GetFeatureAsync(featureId);
            }

            return _features.Query()
                .FirstOrDefaultAsync(entity => entity.Name == nameOrGuid);
        }

        public Task<List<Feature>> GetFeaturesAsync()
        {
            return _features.Query()
                .ToListAsync();
        }

        public Task<List<Feature>> GetActivatedFeaturesAsync()
        {
            return _featureApplicationMappings.Query()
                .Include(entity => entity.Feature)
                .Select(entity => entity.Feature)
                .ToListAsync();
        }

        public Task<Feature> GetFeatureAsync(Guid featureId)
        {
            return _features.Query()
                .FirstOrDefaultAsync(entity => entity.Id == featureId);
        }

        public Feature GetFeature(Guid featureId)
        {
            return _features.Query()
                .FirstOrDefault(entity => entity.Id == featureId);
        }

        public async Task<Dictionary<string, string>> GetGlobalFeatureSettingsAsync(Guid featureId)
        {
            var settings = await _globalFeatureSettings.Query()
                .Where(entity => entity.FeatureId == featureId)
                .ToListAsync();

            return settings.ToDictionary(key => key.Key, value => value.Value);
        }

        public async Task AddOrSetGlobalFeatureSettingAsync(Guid featureId, string key, string value)
        {
            var existing = await _globalFeatureSettings.Query()
                .FirstOrDefaultAsync(entity => entity.FeatureId == featureId && entity.Key == key);

            if (existing != null)
            {
                existing.Value = value;
                await _globalFeatureSettings.UpdateAsync(existing);
            }
            else
            {
                await _globalFeatureSettings.InsertAsync(new GlobalFeatureSetting
                {
                    FeatureId = featureId,
                    Key = key,
                    Value = value
                });
            }
        }

        public async Task DeleteGlobalFeatureSettingAsync(Guid featureId, string key)
        {
            var existing = await _globalFeatureSettings.Query()
                .FirstOrDefaultAsync(entity => entity.FeatureId == featureId && entity.Key == key);

            if (existing != null)
            {
                await _globalFeatureSettings.DeleteAsync(existing);
            }
        }

        public async Task<Dictionary<string, string>> GetApplicationFeatureSettingsAsync(Guid featureId)
        {
            var settings = await _applicationFeatureSettings.Query()
                .Where(entity => entity.FeatureId == featureId)
                .ToListAsync();

            return settings.ToDictionary(key => key.Key, value => value.Value);
        }

        public async Task AddOrSetApplicationFeatureSettingAsync(Guid featureId, string key, string value)
        {
            var existing = await _applicationFeatureSettings.Query()
                .FirstOrDefaultAsync(entity => entity.FeatureId == featureId && entity.Key == key);

            if (existing != null)
            {
                existing.Value = value;
                await _applicationFeatureSettings.UpdateAsync(existing);
            }
            else
            {
                await _applicationFeatureSettings.InsertAsync(new ApplicationFeatureSetting
                {
                    FeatureId = featureId,
                    Key = key,
                    Value = value
                });
            }
        }

        public async Task DeleteApplicationFeatureSettingAsync(Guid featureId, string key)
        {
            var existing = await _applicationFeatureSettings.Query()
                .FirstOrDefaultAsync(entity => entity.FeatureId == featureId && entity.Key == key);

            if (existing != null)
            {
                await _applicationFeatureSettings.DeleteAsync(existing);
            }
        }

        public Task<List<Feature>> GetApplicationFeaturesAsync()
        {
            return _featureApplicationMappings.Query()
                .Include(entity => entity.Feature)
                .Select(entity => entity.Feature)
                .ToListAsync();
        }

        public async Task AddApplicationFeatureAsync(Guid featureId)
        {
            if (! await _featureApplicationMappings.Query().AnyAsync(mapping => mapping.FeatureId == featureId))
            {
                await _featureApplicationMappings.InsertAsync(new FeatureApplicationMapping
                {
                    FeatureId = featureId
                });
            }
        }

        public async Task DeleteApplicationFeatureAsync(Guid featureId)
        {

            var existing = await _featureApplicationMappings.Query()
                .FirstOrDefaultAsync(entity => entity.FeatureId == featureId);

            if (existing != null)
            {
                await _featureApplicationMappings.DeleteAsync(existing);
            }
        }

        public Task DeleteFeatureAsync(Guid featureId)
        {
            throw new NotImplementedException();
        }

        public async Task<Feature> AddFeatureFromZipAsync(byte[] zipArchive)
        {
            var config = await GetFeatureConfigurationAsync(zipArchive);

            return null;
        }

        public Task UpdateFeatureFromZipAsync(byte[] zipArchive)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, string>> GetSettingsAsync(Guid featureId)
        {
            var globalSettings = await GetGlobalFeatureSettingsAsync(featureId);
            var applicationSettings = await GetApplicationFeatureSettingsAsync(featureId);

            foreach (var globalSetting in globalSettings.Where(globalSetting => !applicationSettings.ContainsKey(globalSetting.Key)))
            {
                applicationSettings.Add(globalSetting.Key, globalSetting.Value);
            }

            return applicationSettings;
        }

        private Assembly LoadIntegratedFeatureAssembly(Feature feature)
        {
            // we assume that integrated features are located in a global scope... gac, basedir, etc..
            //Assembly.l
            var assembly = Assembly.Load(feature.AssemblyName);
            return assembly;
        }

        public async Task<FeatureBase> InstantiateFeatureAsync(Guid featureId)
        {
            var feature = await GetFeatureAsync(featureId);

            if (feature == null)
            {
                return null;
            }

            return InstantiateInternal(feature);
        }

        private FeatureBase InstantiateInternal(Feature feature)
        {
            Assembly assembly = null;

            switch (feature.FeatureType)
            {
                case FeatureType.Integrated:
                    assembly = LoadIntegratedFeatureAssembly(feature);
                    break;

                default:
                    return null;
            }

            if (assembly == null)
            {
                return null;
            }

            var types = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof (FeatureBase)))
                .ToList();

            if (types.Count > 1)
            {
                // TODO: own exception
                throw new ArgumentOutOfRangeException("A feature must contain only 1 class inherit from FeatureBase");
            }

            if (!types.Any())
            {
                // TODO: own exception
                throw new ArgumentOutOfRangeException("The feature doesnt contains a subclass of FeatureBase");
            }

            return Activator.CreateInstance(types.First()) as FeatureBase;
        }

        public List<Feature> GetFarmFeatures()
        {
            return _features.Query()
                .Where(feature => feature.Scope == FeatureScope.Farm || feature.Scope == FeatureScope.FarmAndApplication)
                .ToList();
        }

        public void EnableFarmFeature(Guid featureId)
        {
            if (!_activatedFarmFeatures.Query().Any(entity => entity.FeatureId == featureId))
            {
                _activatedFarmFeatures.Insert(new ActiveFarmFeature
                {
                    FeatureId = featureId
                });
            }
        }

        public void DisableFarmFeature(Guid featureId)
        {
            if (_activatedFarmFeatures.Query().Any(entity => entity.FeatureId == featureId))
            {
                _activatedFarmFeatures.Delete(new ActiveFarmFeature
                {
                    FeatureId = featureId
                });
            }
        }

        public Task<List<Feature>> GetFarmFeaturesAsync()
        {
            return _features.Query()
                .Where(feature => feature.Scope == FeatureScope.Farm || feature.Scope == FeatureScope.FarmAndApplication)
                .ToListAsync();
        }

        public List<Feature> GetActivatedFarmFeatures()
        {
            return _activatedFarmFeatures.Query()
                .Include(entity => entity.Feature)
                .Select(entity => entity.Feature)
                .ToList();
        }

        public Task<List<Feature>> GetActivatedFarmFeaturesAsync()
        {
            return _activatedFarmFeatures.Query()
                .Include(entity => entity.Feature)
                .Select(entity => entity.Feature)
                .ToListAsync();
        }

        public async Task EnableFarmFeatureAsync(Guid featureId)
        {
            if (!await _activatedFarmFeatures.Query().AnyAsync(entity => entity.FeatureId == featureId))
            {
                await _activatedFarmFeatures.InsertAsync(new ActiveFarmFeature
                {
                    FeatureId = featureId
                });
            }
        }

        public async Task DisableFarmFeatureAsync(Guid featureId)
        {
            if (await _activatedFarmFeatures.Query().AnyAsync(entity => entity.FeatureId == featureId))
            {
                await _activatedFarmFeatures.DeleteAsync(new ActiveFarmFeature
                {
                    FeatureId = featureId
                });
            }
        }

        public FeatureBase InstantiateFeature(Guid featureId)
        {
            var feature = GetFeature(featureId);

            if (feature == null)
            {
                return null;
            }

            return InstantiateInternal(feature);
        }

        private Task<ConfigurationModel> GetFeatureConfigurationAsync(byte[] zipArchive)
        {
            using (var stream = new MemoryStream(zipArchive))
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    var configEntry = archive.Entries.FirstOrDefault(entry => entry.Name == "feature.json");

                    if (configEntry == null)
                    {
                        throw new InvalidFeaturePackageException(null);
                    }

                    var configStream = configEntry.Open();

                    using (var configReader = new StreamReader(configStream))
                    {
                        var content = configReader.ReadToEnd();
                        return JsonConvert.DeserializeObjectAsync<ConfigurationModel>(content);
                    }
                }
            }
        }
    }
}

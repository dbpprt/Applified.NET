﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
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
        private readonly IStorageService _storageService;
        private readonly IUnitOfWork _context;

        [ImportMany(typeof(IntegratedFeatureBase))]
        private IntegratedFeatureBase[] _integratedFeatures = null;

        [ImportMany(typeof(FeatureBase))]
        private FeatureBase[] _thirdPartyFeatures = null;

        public FeatureService(
            IRepository<Feature> features,
            IRepository<FeatureApplicationMapping> featureApplicationMappings,
            IRepository<GlobalFeatureSetting> globalFeatureSettings,
            IRepository<ApplicationFeatureSetting> applicationFeatureSettings,
            IStorageService storageService,
            IUnitOfWork context
            )
        {
            _features = features;
            _featureApplicationMappings = featureApplicationMappings;
            _globalFeatureSettings = globalFeatureSettings;
            _applicationFeatureSettings = applicationFeatureSettings;
            _storageService = storageService;
            _context = context;
        }

        public Task<List<Feature>> GetFeaturesAsync()
        {
            return _features.Query()
                .ToListAsync();
        }

        public Task<Feature> GetFeatureAsync(Guid featureId)
        {
            return _features.Query()
                .FirstOrDefaultAsync(entity => entity.Id == featureId);
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

        public Task AddApplicationFeatureAsync(Guid featureId)
        {
            return _featureApplicationMappings.InsertAsync(new FeatureApplicationMapping
            {
                FeatureId = featureId
            });
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

        public Task<List<FeatureBase>> GetFeatureInstancesAsync()
        {
            throw new NotImplementedException();
        }

        public Task SynchronizeIntegratedFeaturesWithDatabaseAsync(string baseDirectory)
        {
            var catalog = new DirectoryCatalog(baseDirectory, "*.IntegratedFeature.*");
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
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

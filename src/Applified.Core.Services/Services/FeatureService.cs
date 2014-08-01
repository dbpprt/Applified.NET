
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
        private readonly IStorageService _storageService;
        private readonly IServerEnvironment _serverEnvironment;
        private readonly IUnitOfWork _context;

        [ImportMany(typeof(IntegratedFeatureBase))]
        private IntegratedFeatureBase[] _integratedFeatures = null;

        private static List<IntegratedFeatureBase> IntegratedFeatures = null;
        private static SemaphoreSlim IntegratedFeatureLock = new SemaphoreSlim(1);

            
        [ImportMany(typeof(FeatureBase))]
        private FeatureBase[] _thirdPartyFeatures = null;

        public FeatureService(
            IRepository<Feature> features,
            IRepository<FeatureApplicationMapping> featureApplicationMappings,
            IRepository<GlobalFeatureSetting> globalFeatureSettings,
            IRepository<ApplicationFeatureSetting> applicationFeatureSettings,
            IStorageService storageService,
            IServerEnvironment serverEnvironment,
            IUnitOfWork context
            )
        {
            _features = features;
            _featureApplicationMappings = featureApplicationMappings;
            _globalFeatureSettings = globalFeatureSettings;
            _applicationFeatureSettings = applicationFeatureSettings;
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

        private void LoadIntegratedFeatures(string directory = null)
        {
            if (string.IsNullOrEmpty(directory))
                directory = _serverEnvironment.ApplicationBaseDirectory;
            var baseDirectory = directory;
            var catalog = new DirectoryCatalog(baseDirectory, "*.IntegratedFeatures.*.dll");
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        public async Task<List<FeatureBase>> GetFeatureInstancesAsync()
        {
            LoadIntegratedFeatures();

            // TODO: better loading strategy
            var instances = _integratedFeatures.Cast<FeatureBase>();

            var activeFeatures = await _featureApplicationMappings.Query()
                .ToListAsync();

            return instances.Where(
                    instance => activeFeatures.Any(active => active.FeatureId == instance.FeatureId))
                .ToList();
        }

        public async Task SynchronizeIntegratedFeaturesWithDatabaseAsync(string baseDirectory = null)
        {
            LoadIntegratedFeatures(baseDirectory);
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
                        Name = loadedFeature.Name,
                        Id = loadedFeature.FeatureId,
                        VersionIdentifier = loadedFeature.Version,
                        StoredObjectId = null,
                        StoredObject = null,
                        ExecutionOrderKey = loadedFeature.ExecutionOrderKey
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
                    existing.ExecutionOrderKey = loadedFeature.ExecutionOrderKey;

                    _features.Update(existing, false);
                }
            }


            await _context.SaveAsync();
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

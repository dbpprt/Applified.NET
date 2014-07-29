using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Core.DataAccess.Contracts;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.ServiceContracts;
using Applified.Core.Services.Contracts;

namespace Applified.Core.Services.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly IRepository<Feature> _features;
        private readonly IRepository<FeatureApplicationMapping> _featureApplicationMappings;
        private readonly IRepository<GlobalFeatureSetting> _globalFeatureSettings;
        private readonly IRepository<ApplicationFeatureSetting> _applicationFeatureSettings;
        private readonly IUnitOfWork _context;

        public FeatureService(
            IRepository<Feature> features, 
            IRepository<FeatureApplicationMapping> featureApplicationMappings,
            IRepository<GlobalFeatureSetting> globalFeatureSettings,
            IRepository<ApplicationFeatureSetting> applicationFeatureSettings,
            IUnitOfWork context
            )
        {
            _features = features;
            _featureApplicationMappings = featureApplicationMappings;
            _globalFeatureSettings = globalFeatureSettings;
            _applicationFeatureSettings = applicationFeatureSettings;
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

        public Task<Feature> AddFeatureFromZipAsync(byte[] zipArchive)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFeatureFromZipAsync(byte[] zipArchive)
        {
            throw new NotImplementedException();
        }
    }
}

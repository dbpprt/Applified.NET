using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.Extensibility;

namespace Applified.Core.ServiceContracts
{
    public interface IFeatureService
    {
        Task<Feature> FindFeature(string nameOrGuid);

        Task<List<Feature>> GetFeaturesAsync();

        Task<Feature> GetFeatureAsync(Guid featureId);

        Task<Dictionary<string, string>> GetGlobalFeatureSettingsAsync(Guid featureId);

        Task AddOrSetGlobalFeatureSettingAsync(Guid featureId, string key, string value);

        Task DeleteGlobalFeatureSettingAsync(Guid featureId, string key);

        Task<Dictionary<string, string>> GetApplicationFeatureSettingsAsync(Guid featureId);

        Task AddOrSetApplicationFeatureSettingAsync(Guid featureId, string key, string value);

        Task DeleteApplicationFeatureSettingAsync(Guid featureId, string key);

        Task<List<Feature>> GetApplicationFeaturesAsync();

        Task AddApplicationFeatureAsync(Guid featureId);

        Task DeleteApplicationFeatureAsync(Guid featureId);

        Task DeleteFeatureAsync(Guid featureId);

        Task<Feature> AddFeatureFromZipAsync(byte[] zipArchive);

        Task UpdateFeatureFromZipAsync(byte[] zipArchive);

        List<FeatureBase> GetFeatureInstancesAsync();

        Task SynchronizeIntegratedFeaturesWithDatabaseAsync(string baseDirectory);
    }
}

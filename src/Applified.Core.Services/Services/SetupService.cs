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
                    existing.AssemblyName = loadedFeature.AssemblyName;
                    existing.ExecutionOrderKey = loadedFeature.ExecutionOrderKey;

                    _features.Update(existing, false);
                }
            }

            await _context.SaveAsync();
        }
    }
}

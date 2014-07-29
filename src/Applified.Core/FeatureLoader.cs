using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.Extensibility;
using Microsoft.Practices.Unity;

namespace Applified.Core
{
    public class FeatureLoader
    {
        private readonly string _featureDirectory;
        private readonly IUnityContainer _container;
        private readonly IUnityServiceProvider _scope;
        private readonly DirectoryCatalog _catalog;

        [ImportMany(typeof(FeatureBase))] 
        private FeatureBase[] _featuresBase = null;


        public FeatureLoader(
            string featureDirectory,
            IUnityContainer container,
            IUnityServiceProvider scope)
        {
            _featureDirectory = featureDirectory;
            _container = container;
            _scope = scope;
            _catalog = new DirectoryCatalog(_featureDirectory);
        }

        public void LoadAndRegisterFeatures()
        {
            var container = new CompositionContainer(_catalog);
            container.ComposeParts(this);
        }
    }
}

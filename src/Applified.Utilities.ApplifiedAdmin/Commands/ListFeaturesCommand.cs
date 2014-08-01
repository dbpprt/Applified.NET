using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Common;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.ServiceContracts;
using Microsoft.Practices.Unity;

namespace Applified.Utilities.ApplifiedAdmin.Commands
{
    class ListFeaturesCommand : AppContextCommandBase
    {
        private readonly IFeatureService _featureService;

        public ListFeaturesCommand(
            Options options, 
            IUnityContainer container,
            IFeatureService featureService) 
            : base(options, container)
        {
            _featureService = featureService;
        }

        public override async Task<int> Execute()
        {
            if (!string.IsNullOrEmpty(Options.TargetApplication))
            {
                return await base.Execute();
            }

            var features = await _featureService.GetFeaturesAsync();
            var output = ObjectDumper.Dump(features);

            Console.WriteLine(output);

            return 0;
        }

        public override async Task AppContextInvoke(IUnityContainer scope)
        {
            var featureService = scope.Resolve<IFeatureService>();
            var features = await featureService.GetApplicationFeaturesAsync();

            var output = ObjectDumper.Dump(features);

            Console.WriteLine(output);
        }
    }
}

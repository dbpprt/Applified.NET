using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Common;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.ServiceContracts;

namespace Applified.Utilities.ApplifiedAdmin.Commands
{
    class ListGlobalFeatureSettings : CommandBase
    {
        private readonly IFeatureService _featureService;

        public ListGlobalFeatureSettings(
            IFeatureService featureService,
            Options options
            ) 
            : base(options)
        {
            _featureService = featureService;
        }

        public override async Task<int> Execute()
        {
            var feature = await _featureService.FindFeature(Options.TargetFeature);
            if (feature == null)
            {
                Console.WriteLine("Feature not found");
                return -1;
            }

            var features = await _featureService.GetGlobalFeatureSettingsAsync(feature.Id);
            var output = ObjectDumper.Dump(
                features
                );

            Console.WriteLine(output);

            return 0;
        }
    }
}

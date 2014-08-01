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
    class SetGlobalFeatureSetting : CommandBase
    {
        private readonly IFeatureService _featureService;

        public SetGlobalFeatureSetting(
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

            var key = Options.Key;
            var value = Options.Value;

            await _featureService.AddOrSetGlobalFeatureSettingAsync(feature.Id, key, value);

            Console.WriteLine("Setting written!");

            return 0;
        }
    }
}

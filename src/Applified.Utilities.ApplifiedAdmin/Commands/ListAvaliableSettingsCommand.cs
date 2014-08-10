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
    class ListAvaliableSettingsCommand : CommandBase
    {
        private readonly IFeatureService _featureService;

        public ListAvaliableSettingsCommand(
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

            var instance = await _featureService.InstantiateFeatureAsync(feature.Id);

            if (instance == null)
            {
                
                throw new Exception("Something went wrong..");
            }

            var settings = instance.GetAvaliableSettings();
            var output = ObjectDumper.Dump(settings.ToArray());
            Console.WriteLine(output);
            
            return 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Common;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.ServiceContracts;

namespace Applified.Utilities.ApplifiedAdmin.Commands
{
    class SynchronizeFeaturesCommand : CommandBase
    {
        private readonly IFeatureService _featureService;

        public SynchronizeFeaturesCommand(
            IFeatureService featureService,
            Options options
            ) 
            : base(options)
        {
            _featureService = featureService;
        }

        public override async Task<int> Execute()
        {
            Debugger.Launch();

            await _featureService.SynchronizeIntegratedFeaturesWithDatabaseAsync(Options.TargetDirectory);

            Console.WriteLine("Features synchronized");

            return 0;
        }
    }
}

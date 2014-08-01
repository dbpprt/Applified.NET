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
    class CreateApplicationCommand : CommandBase
    {
        private readonly IApplicationService _applicationService;

        public CreateApplicationCommand(
            Options options,
            IApplicationService applicationService
            ) 
            : base(options)
        {
            _applicationService = applicationService;
        }

        public override async Task<int> Execute()
        {
            Debugger.Launch();

            var application = await _applicationService.CreateApplicationAsync(new Application
            {
                Name = Options.TargetName
            });
            
            var output = ObjectDumper.Dump(application);

            Console.WriteLine(output);

            return 0;
        }
    }
}

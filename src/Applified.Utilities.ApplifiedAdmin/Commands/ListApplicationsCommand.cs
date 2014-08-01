using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Common;
using Applified.Core.ServiceContracts;

namespace Applified.Utilities.ApplifiedAdmin.Commands
{
    class ListApplicationsCommand : CommandBase
    {
        private readonly IApplicationService _applicationService;

        public ListApplicationsCommand(
            Options options,
            IApplicationService applicationService
            ) 
            : base(options)
        {
            _applicationService = applicationService;
        }

        public override async Task<int> Execute()
        {
            var applications = await _applicationService.GetApplicationsAsync();
            var output = ObjectDumper.Dump(applications);

            Console.WriteLine(output);

            return 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Common;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.ServiceContracts;
using Microsoft.Practices.Unity;

namespace Applified.Utilities.ApplifiedAdmin.Commands
{
    class ListBindingsCommand : AppContextCommandBase
    {
        public ListBindingsCommand(Options options, IUnityContainer container) 
            : base(options, container)
        {
        }

        public override async Task AppContextInvoke(IUnityContainer scope)
        {
            var applicationService = scope.Resolve<IApplicationService>();

            var applications = await applicationService.GetBindingsAsync();
            var output = ObjectDumper.Dump(applications);

            Console.WriteLine(output);
        }
    }
}

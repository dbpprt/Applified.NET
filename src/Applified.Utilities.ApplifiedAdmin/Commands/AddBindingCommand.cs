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
    class AddBindingCommand : AppContextCommandBase
    {
        public AddBindingCommand(Options options, IUnityContainer container) 
            : base(options, container)
        {
        }

        public override async Task AppContextInvoke(IUnityContainer scope)
        {
            var applicationService = scope.Resolve<IApplicationService>();

            await applicationService.AddBindingAsync(Options.TargetName);

            Console.WriteLine("Binding added");
        }
    }
}

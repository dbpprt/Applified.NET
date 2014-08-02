using System;
using System.Threading.Tasks;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.ServiceContracts;
using Microsoft.Practices.Unity;

namespace Applified.Utilities.ApplifiedAdmin.Commands
{
    abstract class AppContextCommandBase : CommandBase
    {
        public IUnityContainer Container { get; set; }

        public AppContextCommandBase(
            Options options,
            IUnityContainer container
            ) 
            : base(options)
        {
            Container = container;
        }

        public override async Task<int> Execute()
        {
            using (var scope = Container.CreateChildContainer())
            {
                var applicationService = scope.Resolve<IApplicationService>();

                var application = await applicationService.FindApplication(Options.TargetApplication);
                if (application == null)
                {
                    Console.WriteLine("Application not found");
                    return -1;
                }

                using (var innerScope = scope.CreateChildContainer())
                {
                    innerScope.RegisterInstance<ICurrentContext>(new CustomContext(application));

                    await AppContextInvoke(innerScope);
                }
            }


            return 0;
        }

        public abstract Task AppContextInvoke(IUnityContainer scope);
    }
}

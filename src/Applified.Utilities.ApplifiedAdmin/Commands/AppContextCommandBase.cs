#region Copyright (C) 2014 Applified.NET 
// Copyright (C) 2014 Applified.NET
// http://www.applified.net

// This file is part of Applified.NET.

// Applified.NET is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.

// You should have received a copy of the GNU Affero General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion

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

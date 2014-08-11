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

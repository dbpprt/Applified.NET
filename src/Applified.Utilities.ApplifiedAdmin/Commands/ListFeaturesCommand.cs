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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Common;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.ServiceContracts;
using Microsoft.Practices.Unity;

namespace Applified.Utilities.ApplifiedAdmin.Commands
{
    class ListFeaturesCommand : AppContextCommandBase
    {
        private readonly IFeatureService _featureService;

        public ListFeaturesCommand(
            Options options, 
            IUnityContainer container,
            IFeatureService featureService) 
            : base(options, container)
        {
            _featureService = featureService;
        }

        public override async Task<int> Execute()
        {
            if (!string.IsNullOrEmpty(Options.TargetApplication))
            {
                return await base.Execute();
            }

            var features = await _featureService.GetFeaturesAsync();
            var output = ObjectDumper.Dump(features);

            Console.WriteLine(output);

            return 0;
        }

        public override async Task AppContextInvoke(IUnityContainer scope)
        {
            var featureService = scope.Resolve<IFeatureService>();
            var features = await featureService.GetApplicationFeaturesAsync();

            var output = ObjectDumper.Dump(features);

            Console.WriteLine(output);
        }
    }
}

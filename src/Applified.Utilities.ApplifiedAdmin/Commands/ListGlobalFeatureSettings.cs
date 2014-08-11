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

namespace Applified.Utilities.ApplifiedAdmin.Commands
{
    class ListGlobalFeatureSettings : CommandBase
    {
        private readonly IFeatureService _featureService;

        public ListGlobalFeatureSettings(
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

            var features = await _featureService.GetGlobalFeatureSettingsAsync(feature.Id);
            var output = ObjectDumper.Dump(
                features
                );

            Console.WriteLine(output);

            return 0;
        }
    }
}

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
using Applified.Core.Entities.Infrastructure;
using Applified.Core.Extensibility;

namespace Applified.Core.ServiceContracts
{
    public interface IFeatureService
    {
        Task<Feature> FindFeature(string nameOrGuid);

        Task<List<Feature>> GetFeaturesAsync();

        Task<Feature> GetFeatureAsync(Guid featureId);

        Task<Dictionary<string, string>> GetGlobalFeatureSettingsAsync(Guid featureId);

        Task AddOrSetGlobalFeatureSettingAsync(Guid featureId, string key, string value);

        Task DeleteGlobalFeatureSettingAsync(Guid featureId, string key);

        Task<Dictionary<string, string>> GetApplicationFeatureSettingsAsync(Guid featureId);

        Task AddOrSetApplicationFeatureSettingAsync(Guid featureId, string key, string value);

        Task DeleteApplicationFeatureSettingAsync(Guid featureId, string key);

        Task<List<Feature>> GetApplicationFeaturesAsync();

        Task AddApplicationFeatureAsync(Guid featureId);

        Task DeleteApplicationFeatureAsync(Guid featureId);

        Task DeleteFeatureAsync(Guid featureId);

        Task<Feature> AddFeatureFromZipAsync(byte[] zipArchive);

        Task UpdateFeatureFromZipAsync(byte[] zipArchive);

        Task<Dictionary<string, string>> GetSettingsAsync(Guid featureId);

        Task<FeatureBase> InstantiateFeatureAsync(Guid featureId);
    }
}

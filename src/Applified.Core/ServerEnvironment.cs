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
using System.IO;
using Applified.Core.ServiceContracts;

namespace Applified.Core
{
    class ServerEnvironment : IServerEnvironment
    {
        private readonly Settings _settings;

        public ServerEnvironment(Settings settings)
        {
            _settings = settings;
        }

        public string ApplicationBaseDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public string DeploymentDirectory
        {
            get { return Path.Combine(
                _settings.GetValue<string>(Settings.ApplicationWorkingDirectory),
                _settings.GetValue<string>(Settings.DeploymentDirectory)
                ); }
        }

        public string FeatureDirectory
        {
            get
            {
                return Path.Combine(
                    _settings.GetValue<string>(Settings.ApplicationWorkingDirectory),
                    _settings.GetValue<string>(Settings.FeatureDirectory)
                    );
            }
        }

        public string GetDeploymentDirectory(Guid deploymentId)
        {
            return Path.Combine(DeploymentDirectory, deploymentId.ToString());
        }
    }
}

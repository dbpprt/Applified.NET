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

namespace Applified.IntegratedFeatures.StaticFileHandler
{
    class Settings : SettingsBase
    {
        public const string EnableDirectoryBrowsing = "EnableDirectoryBrowsing";
        public const string EnableDefaultFiles = "EnableDefaultFiles";
        public const string DefaultFiles = "DefaultFiles";
        public const string ServeUnknownFileTypes = "ServeUnknownFileTypes";
        public const string RequestPath = "RequestPath";

        public Settings(Dictionary<string, string> settings) 
            : base(settings)
        {
            Register(EnableDirectoryBrowsing, false, "Specifies wether to enable a directory browser. Should only be used for testing purposes. I'm not quite sure wether it's compatible with angular-html5-navigation-rewrite.");
            Register(EnableDefaultFiles, true, "Allows requests to folders to be rewritten to an existing index file.");
            Register(DefaultFiles, "index.html;index.htm", "The default files which will be searched in the deployments directory.");
            Register(ServeUnknownFileTypes, true, "Specifies wether the FileHandler should serve unknown files, which have no registered MimeType.");
            Register(RequestPath, "", "The base request path, which this feature listens to.");
        }
    }
}

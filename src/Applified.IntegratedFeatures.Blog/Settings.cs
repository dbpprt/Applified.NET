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

using System.Collections.Generic;
using Applified.Common;
using Applified.Common.Configuration;

namespace Applified.IntegratedFeatures.Blog
{
    class Settings : SettingsBase
    {
        public const string MetaweblogEndpoint = "MetaweblogEndpoint";
        public const string EnableMetaweblogEndpoint = "EnableMetaweblogEndpoint";
        public const string MetaweblogServiceName = "MetaweblogServiceName";
        public const string MetaweblogWatermarkImageUrl = "MetaweblogWatermarkImageUrl";
        public const string MetaweblogImageUrl = "MetaweblogImageUrl";
        public const string MetaweblogHomepageLinkText = "MetaweblogHomepageLinkText";
        public const string MetaweblogAdminLinkText = "MetaweblogAdminLinkText";
        public const string MetaweblogAdminUrl = "MetaweblogAdminUrl";
        public const string MetaweblogRequiredGroupMembership = "MetaweblogRequiredGroupMembership";
        public const string MetaweblogManifestEndpoint = "MetaweblogManifestEndpoint";

        public Settings(Dictionary<string, string> settings)
            : base(settings)
        {
            Register(MetaweblogEndpoint, "metaweblog", "");
            Register(EnableMetaweblogEndpoint, true, "");
            Register(MetaweblogServiceName, "blog", "");
            Register(MetaweblogWatermarkImageUrl, "", "");
            Register(MetaweblogImageUrl, "/favicon.ico", "");
            Register(MetaweblogHomepageLinkText, "View blog", "");
            Register(MetaweblogAdminUrl, "", "");
            Register(MetaweblogAdminLinkText, "", "");
            Register(MetaweblogRequiredGroupMembership, "MetaweblogBlogger", "");
            Register(MetaweblogManifestEndpoint, "wlwmanifest.xml", "");
        }
    }
}

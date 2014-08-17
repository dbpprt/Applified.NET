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

namespace Applified.IntegratedFeatures.PageOfDeath
{
    class Settings : SettingsBase
    {
        public const string ShowCookies = "ShowCookies";
        public const string ShowEnvironment = "ShowEnvironment";
        public const string ShowExceptionDetails = "ShowExceptionDetails";
        public const string ShowHeaders = "ShowHeaders";
        public const string ShowQuery = "ShowQuery";
        public const string ShowSourceCode = "ShowSourceCode";
        public const string SourceCodeLineCount = "SourceCodeLineCount";

        public Settings(Dictionary<string, string> settings) 
            : base(settings)
        {
            Register(ShowCookies, true, "Specifies wether the error page should show the clients cookies.");
            Register(ShowEnvironment, true, "Specifies wether the the error page should show owin environment variables.");
            Register(ShowExceptionDetails, true, "Specifies wether the the error page should show detailed exception infos.");
            Register(ShowHeaders, true, "Specifies wether the the error page should show all header variables.");
            Register(ShowQuery, true, "Specifies wether the the error page should show the query string.");
            Register(ShowSourceCode, true, "Specifies wether the the error page should show the source code where the exception occurred.");
            Register(SourceCodeLineCount, 20, "Specifies how much lines of the source code the error page should display.");
        }
    }
}

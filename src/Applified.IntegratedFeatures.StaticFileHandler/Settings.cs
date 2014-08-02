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
        public const string UseDefaultFiles = "UseDefaultFiles";
        public const string DefaultFiles = "DefaultFiles";
        public const string ServeUnknownFileTypes = "ServeUnknownFileTypes";
        public const string RequestPath = "RequestPath";

        public Settings(Dictionary<string, string> settings) 
            : base(settings)
        {
            Register(EnableDirectoryBrowsing, false);
            Register(UseDefaultFiles, true);
            Register(DefaultFiles, "index.html;index.htm");
            Register(ServeUnknownFileTypes, true);
            Register(RequestPath, "");
        }
    }
}

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

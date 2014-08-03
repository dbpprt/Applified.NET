using System.Collections.Generic;
using Applified.Common;

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

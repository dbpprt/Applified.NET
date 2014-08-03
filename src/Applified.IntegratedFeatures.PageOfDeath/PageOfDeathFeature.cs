using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Applified.Common.OwinDependencyInjection;
using Applified.Core.Extensibility;
using Applified.Core.ServiceContracts;
using Applified.IntegratedFeatures.PageOfDeath.Wrappers;
using Microsoft.Owin;
using Microsoft.Owin.Diagnostics;
using Owin;

namespace Applified.IntegratedFeatures.PageOfDeath
{
    [Export(typeof(IntegratedFeatureBase))]
    public class PageOfDeathFeature : IntegratedFeatureBase
    {
        public override Guid FeatureId
        {
            get { return new Guid("12EF8DF6-978C-4001-9E93-56C183776BF7"); }
        }

        public override int ExecutionOrderKey
        {
            get { return int.MaxValue; }
        }

        public override async Task<OwinMiddleware> UseAsync(
            Guid applicationId,
            OwinMiddleware next, 
            IAppBuilder appBuilder, 
            IDependencyScope scope)
        {
            var featureService = scope.Resolve<IFeatureService>();
            var dictionary = await featureService.GetSettingsAsync(FeatureId);
            var settings = new Settings(dictionary);

            var middleware = new ErrorPageMiddlewareWrapper(
                next,
                new ErrorPageOptions
                {
                    ShowCookies = settings.GetValue<bool>(Settings.ShowCookies),
                    ShowEnvironment = settings.GetValue<bool>(Settings.ShowEnvironment),
                    ShowExceptionDetails = settings.GetValue<bool>(Settings.ShowExceptionDetails),
                    ShowHeaders = settings.GetValue<bool>(Settings.ShowHeaders),
                    ShowQuery = settings.GetValue<bool>(Settings.ShowQuery),
                    ShowSourceCode = settings.GetValue<bool>(Settings.ShowSourceCode),
                    SourceCodeLineCount = settings.GetValue<int>(Settings.SourceCodeLineCount)
                });

            return middleware;
        }

        public override string Name
        {
            get { return "page-of-death"; }
        }

        public override string Description
        {
            get
            {
                return
                    "This feature enables the ErrorMiddleware shipped with Microsoft.Owin.Diagnostics. Good for debugging purposes!";
            }
        }

        public override string Version
        {
            get { return "0.0.1-alpha-1"; }
        }

        public override string Author
        {
            get { return "Dennis Bappert"; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Applified.Core.Extensibility;
using Microsoft.Owin;
using Owin;

namespace Applified.IntegratedFeatures.PageOfDeath
{
    public class PageOfDeathFeature : IntegratedFeatureBase
    {
        public override Guid FeatureId
        {
            get { throw new NotImplementedException(); }
        }

        public override int ExecutionOrderKey
        {
            get { throw new NotImplementedException(); }
        }

        public override Task<OwinMiddleware> UseAsync(Guid applicationId, OwinMiddleware next, IAppBuilder appBuilder, IDependencyScope scope)
        {
            throw new NotImplementedException();
        }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        public override string Description
        {
            get { throw new NotImplementedException(); }
        }

        public override string Version
        {
            get { throw new NotImplementedException(); }
        }

        public override string Author
        {
            get { throw new NotImplementedException(); }
        }
    }
}

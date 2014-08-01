using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Core.Entities.Infrastructure;
using Applified.Core.ServiceContracts;

namespace Applified.Utilities.ApplifiedAdmin
{
    class CustomContext : ICurrentContext
    {
        public CustomContext(Application application)
        {
            ApplicationId = application.Id;
            DeploymentId = application.ActiveDeploymentId;
            IsAdmin = true;
        }

        public bool IsAdmin { get; private set; }
        public Guid ApplicationId { get; private set; }
        public Guid? DeploymentId { get; private set; }
        public string BaseDirectory { get; private set; }
    }
}

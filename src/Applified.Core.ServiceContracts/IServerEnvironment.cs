using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Applified.Core.ServiceContracts
{
    public interface IServerEnvironment
    {
        string ApplicationBaseDirectory { get; }

        string DeploymentDirectory { get; }

        string FeatureDirectory { get; }

        string GetDeploymentDirectory(Guid deploymentId);
    }
}

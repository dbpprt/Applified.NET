using System;
using Applified.Core.ServiceContracts;

namespace Applified.Core
{
    class DevelopmentServerEnvironment : IServerEnvironment
    {
        public string ApplicationBaseDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public string DeploymentDirectory
        {
            get { return @"C:\Applified\deployments"; }
        }

        public string FeatureDirectory { get { return @"C:\Applified\features"; } }
    }
}

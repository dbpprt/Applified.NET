using System;

namespace Applified.Common.Exceptions
{
    public class NoActiveDeploymentException : ApplifiedException
    {
        public NoActiveDeploymentException(Exception innerException)
            : base("An application should have a active deployment", innerException)
        {
            
        }
    }
}

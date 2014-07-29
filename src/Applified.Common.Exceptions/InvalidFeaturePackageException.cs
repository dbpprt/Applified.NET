using System;

namespace Applified.Common.Exceptions
{
    public class InvalidFeaturePackageException : ApplifiedException
    {
        public InvalidFeaturePackageException(Exception innerException)
            : base("A feature package should contain a feature.json file and should be a valid zip archive", innerException)
        {
            
        }
    }
}

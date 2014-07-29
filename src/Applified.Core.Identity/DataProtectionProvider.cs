using Microsoft.Owin.Security.DataProtection;

namespace Applified.Core.Identity
{
    public class DataProtectionProvider : IDataProtectionProvider
    {
        // TODO: SECURITY CRITICAL! Implement some logic here!

        public IDataProtector Create(params string[] purposes)
        {
            return new DataProtector();
        }
    }
}

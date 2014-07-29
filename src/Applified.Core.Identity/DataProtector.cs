using Microsoft.Owin.Security.DataProtection;

namespace Applified.Core.Identity
{
    public class DataProtector : IDataProtector
    {
        public byte[] Protect(byte[] userData)
        {
            return userData;
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            return protectedData;
        }
    }
}

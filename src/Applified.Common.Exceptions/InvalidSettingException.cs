using System;
using System.Security.Cryptography;

namespace Applified.Common.Exceptions
{
    public class InvalidSettingException : ApplifiedException
    {
        public InvalidSettingException(string desiredKey)
            : base("A settings with the key " + desiredKey + " isnt registered in this settings class!")
        {
            
        }
    }
}

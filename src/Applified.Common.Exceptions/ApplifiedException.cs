using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applified.Common.Exceptions
{
    public class ApplifiedException : Exception
    {
        public ApplifiedException() { }

        public ApplifiedException(string message) : base(message) { }

        public ApplifiedException(string message, Exception innerException) { }
    }
}

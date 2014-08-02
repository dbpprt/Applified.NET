using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Applified.Common
{
    public class Optional<T>
    {
        public bool IsSet { get; set; }

        public bool IsValid { get; set; }

        public Exception InnerException { get; set; } 

        public T Value { get; set; }
    }
}

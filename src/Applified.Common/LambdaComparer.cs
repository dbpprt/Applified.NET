using System;
using System.Collections.Generic;

namespace Applified.Common
{
    public class LambdaComparer<T> : IEqualityComparer<T>
    {
        public LambdaComparer(Func<T, T, bool> cmp)
        {
            this.cmp = cmp;
        }
        public bool Equals(T x, T y)
        {
            return cmp(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public Func<T, T, bool> cmp { get; set; }
    }
}

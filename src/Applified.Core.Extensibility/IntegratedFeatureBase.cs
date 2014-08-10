using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Applified.Core.Extensibility
{
    public abstract class IntegratedFeatureBase : FeatureBase
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        public abstract string Version { get; }

        public abstract string Author { get; }

    }
}

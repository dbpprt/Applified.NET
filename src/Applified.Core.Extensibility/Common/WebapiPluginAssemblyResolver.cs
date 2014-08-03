using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace Applified.Core.Extensibility.Common
{
    public abstract class WebapiPluginAssemblyResolver : IAssembliesResolver
    {
        public abstract ICollection<Assembly> GetAssemblies();
    }
}

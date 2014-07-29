using Microsoft.Practices.Unity;

namespace Applified.Common.Unity
{
    public abstract class UnityModule : IUnityModule
    {
        public abstract void RegisterDependencies(IUnityContainer container);
    }
}

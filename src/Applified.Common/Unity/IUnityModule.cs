using Microsoft.Practices.Unity;

namespace Applified.Common.Unity
{
    public interface IUnityModule
    {
        void RegisterDependencies(IUnityContainer container);
    }
}
using System.Web.Http;

namespace Applified.Core.Extensibility.Contracts
{
    public interface IDynamicRewriteRule
    {
        void Register(HttpRouteCollection routes);
    }
}

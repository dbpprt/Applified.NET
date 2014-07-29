using Applified.Core;
using Applified.Hosting.IIS;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Applified.Hosting.IIS
{
    public class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            ApplicationBuilder.Build(app);
        }
    }
}
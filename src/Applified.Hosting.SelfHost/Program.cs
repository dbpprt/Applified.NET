using Applified.Core;
using Microsoft.Owin.Hosting;

namespace Applified.Hosting.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start(
                "http://localhost:8080", 
                ApplicationBuilder.Build))
            {
                System.Console.ReadLine(); 
                
            }
        }
    }
}

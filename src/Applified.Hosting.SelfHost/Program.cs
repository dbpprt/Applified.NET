using System;
using System.Diagnostics;
using Applified.Core;
using Microsoft.Owin.Hosting;

namespace Applified.Hosting.SelfHost
{
    class Program
    {
        private const string ListenOn = "http://+:8080/";
        static void Main(string[] args)
        {
            try
            {
                using (WebApp.Start(
                    ListenOn,
                    ApplicationBuilder.Build))
                {
                    Console.ReadLine();
                }
            }
            catch (Exception)
            {
                NetAclChecker.AddAddress(ListenOn);

                Console.WriteLine("Trying to add a rule for applified.net listen on " + ListenOn);

                using (WebApp.Start(
                    ListenOn,
                    ApplicationBuilder.Build))
                {
                    Console.ReadLine();
                }
            }

        }
    }
}

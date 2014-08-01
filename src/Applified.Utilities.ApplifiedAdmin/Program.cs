using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Applified.Common.Unity;
using Applified.Core;
using Applified.Utilities.ApplifiedAdmin.Commands;
using Microsoft.Practices.Unity;
using Plossum.CommandLine;

namespace Applified.Utilities.ApplifiedAdmin
{
    class Program
    {
        static void Main(string[] args)
        {
            var mainAsync = MainAsync(args);
            mainAsync.Wait();
        }

        static async Task<int> MainAsync(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(options);
            parser.Parse();

            if (options.Help)
            {
                Console.WriteLine(parser.UsageInfo.ToString(78, false));
                return 0;
            }
            else if (parser.HasErrors)
            {
                Console.WriteLine(parser.UsageInfo.ToString(78, true));
                return -1;
            }

            var container = RegisterDependencies(new UnityContainer());
            var commands = RegisterCommands(new CommandCollection());

            container.RegisterInstance(options);
            var match = commands.GetMatch(options);

            using (var scope = container.CreateChildContainer())
            {
                if (match != null)
                {
                    var handler = scope.Resolve(match) as CommandBase;

                    if (handler != null)
                    {
                        return await handler.Execute();
                    }
                }
            }


            return -1;
        }

        static IUnityContainer RegisterDependencies(IUnityContainer unityContainer)
        {
            unityContainer.RegisterModule<MainUnityModule>();

            unityContainer.RegisterType(typeof(ListApplicationsCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof(CreateApplicationCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof(ListFeaturesCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof(ListGlobalFeatureSettings), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof(SetGlobalFeatureSetting), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof(SynchronizeFeaturesCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof(ListBindingsCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof (AddBindingCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof (EnableFeatureCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof (DisableFeatureCommand), new HierarchicalLifetimeManager());

            unityContainer.RegisterInstance(unityContainer);

            return unityContainer;
        }

        static CommandCollection RegisterCommands(CommandCollection commands)
        {
            commands.RegisterType(options => options.ListApplications, typeof(ListApplicationsCommand));
            commands.RegisterType(options => options.ListFeatures, typeof(ListFeaturesCommand));
            commands.RegisterType(options => options.ListGlobalFeatureSettings, typeof(ListGlobalFeatureSettings));
            commands.RegisterType(options => options.SetGlobalFeatureSetting, typeof(SetGlobalFeatureSetting));
            commands.RegisterType(options => options.SynchronizeFeatures, typeof(SynchronizeFeaturesCommand));
            commands.RegisterType(options => options.CreateApplication, typeof(CreateApplicationCommand));
            commands.RegisterType(options => options.ListBindings, typeof(ListBindingsCommand));
            commands.RegisterType(options => options.AddBinding, typeof(AddBindingCommand));
            commands.RegisterType(options => options.EnableFeature, typeof(EnableFeatureCommand));
            commands.RegisterType(options => options.DisableFeature, typeof(DisableFeatureCommand));

            return commands;
        }
    }
}

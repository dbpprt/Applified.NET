#region Copyright (C) 2014 Applified.NET 
// Copyright (C) 2014 Applified.NET
// http://www.applified.net

// This file is part of Applified.NET.

// Applified.NET is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.

// You should have received a copy of the GNU Affero General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
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
            unityContainer.RegisterType(typeof(AddBindingCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof(EnableFeatureCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof(DisableFeatureCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof(ListAvaliableSettingsCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof(MigrateFeatureDatabaseCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof(MigrateDatabaseCommand), new HierarchicalLifetimeManager());
            unityContainer.RegisterType(typeof (CreateEventSourceCommand), new HierarchicalLifetimeManager());

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
            commands.RegisterType(options => options.ListAvaliableSettings, typeof(ListAvaliableSettingsCommand));
            commands.RegisterType(options => options.MigrateFeatureDatabase, typeof(MigrateFeatureDatabaseCommand));
            commands.RegisterType(options => options.MigrateDatabase, typeof(MigrateDatabaseCommand));
            commands.RegisterType(options => options.CreateEventSource, typeof(CreateEventSourceCommand));

            return commands;
        }
    }
}

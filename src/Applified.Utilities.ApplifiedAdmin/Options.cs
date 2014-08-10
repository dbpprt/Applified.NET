using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plossum.CommandLine;

namespace Applified.Utilities.ApplifiedAdmin
{
    [CommandLineManager(ApplicationName = "Applified admin utility  --",
        Copyright = "Copyright (C) Dennis Bappert 2014",
        EnabledOptionStyles = OptionStyles.Group | OptionStyles.LongUnix)]
    [CommandLineOptionGroup("commands", Name = "Commands",
        Require = OptionGroupRequirement.ExactlyOne)]
    [CommandLineOptionGroup("options", Name = "Options")]
    internal class Options
    {
        [CommandLineOption(Name = "v", Aliases = "verbose",
            Description = "Produce verbose output", GroupId = "options")]
        public bool Verbose { get; set; }

        [CommandLineOption(Name = "list-applications",
            Description = "List all applications", GroupId = "commands")]
        public bool ListApplications { get; set; }

        [CommandLineOption(Name = "list-features",
            Description = "List all avaliable features", GroupId = "commands")]
        public bool ListFeatures { get; set; }

        [CommandLineOption(Name = "list-servers",
            Description = "List all servers in this farm", GroupId = "commands")]
        public bool ListServers { get; set; }

        [CommandLineOption(Name = "create-application",
            Description = "Creates a new applications", GroupId = "commands")]
        public bool CreateApplication { get; set; }

        [CommandLineOption(Name = "add-binding",
            Description = "Adds a new binding to a application", GroupId = "commands")]
        public bool AddBinding { get; set; }

        [CommandLineOption(Name = "list-bindings",
            Description = "List all application bindings", GroupId = "commands")]
        public bool ListBindings { get; set; }

        [CommandLineOption(Name = "list-global-feature-settings",
            Description = "List all global feature settings", GroupId = "commands")]
        public bool ListGlobalFeatureSettings { get; set; }

        [CommandLineOption(Name = "synchronize-features",
            Description = "Synchronize all features with the database", GroupId = "commands")]
        public bool SynchronizeFeatures { get; set; }

        [CommandLineOption(Name = "enable-feature",
            Description = "Enables a feature for a given application", GroupId = "commands")]
        public bool EnableFeature { get; set; }

        [CommandLineOption(Name = "disable-feature",
            Description = "Disables a feature for a given application", GroupId = "commands")]
        public bool DisableFeature { get; set; }

        [CommandLineOption(Name = "set-global-feature-setting",
            Description = "Sets or adds a global feature settings", GroupId = "commands")]
        public bool SetGlobalFeatureSetting { get; set; }

        [CommandLineOption(Name = "list-avaliable-settings",
            Description = "Lists all settings that are avaliable for a given feature", GroupId = "commands")]
        public bool ListAvaliableSettings { get; set; }

        [CommandLineOption(Name = "f", Aliases = "feature",
            Description = "The target feature", GroupId = "options")]
        public string TargetFeature { get; set; }

        [CommandLineOption(Name = "d", Aliases = "directory",
            Description = "The target directory", GroupId = "options")]
        public string TargetDirectory { get; set; }

        [CommandLineOption(Name = "a", Aliases = "application",
            Description = "The target application", GroupId = "options")]
        public string TargetApplication { get; set; }

        [CommandLineOption(Name = "n", Aliases = "name",
            Description = "The target name", GroupId = "options")]
        public string TargetName
        { get; set; }

        [CommandLineOption(Name = "key",
            Description = "The settings key", GroupId = "options")]
        public string Key { get; set; }

        [CommandLineOption(Name = "value",
            Description = "The settings value", GroupId = "options")]
        public string Value { get; set; }

        //[CommandLineOption(Name = "c", Aliases = "create",
        //    Description = "Create a new archive", GroupId = "commands")]
        //public bool Create { get; set; }

        //[CommandLineOption(Name = "x", Aliases = "extract",
        //    Description = "Extract files from archive", GroupId = "commands")]
        //public bool Extract { get; set; }

        //[CommandLineOption(Name = "f", Aliases = "file",
        //    Description = "Specify the file name of the archive", MinOccurs = 1)]
        //public string FileName { get; set; }

        [CommandLineOption(Name = "h", Aliases = "help",
            Description = "Shows this help text", GroupId = "commands")]
        public bool Help { get; set; }
    }
}

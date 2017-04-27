using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Database.CD.Lib
{
    public static class ParametersReader
    {
        private static readonly Parameter<bool> Help = new Parameter<bool>
        {
            Name = "--ToolHelp",
            Value = true
        };

        private static readonly Parameter<bool> UseConfigurationFile = new Parameter<bool>
        {
            Name = "--UseConfigurationFile",
            Value = true
        };

        private static readonly Parameter<string> ConnectionString = new Parameter<string>
        {
            Name = "--ConnectionString"
        };

        private static readonly Parameter<string> RollbackVersion = new Parameter<string>
        {
            Name = "--RollbackVersion"
        };

        private static readonly Parameter<bool> Rollback = new Parameter<bool>
        {
            Name = "--Rollback"
        };

        public static ConfigurationOptions ReadOptions(IConfigurationRoot configuration, string[] args)
        {
            LoadAllParameters(configuration, args);



            var options = new ConfigurationOptions()
            {
                ConnectionString = ConnectionString.Value,
                RollbackVersion = RollbackVersion.Value,
                ExecuteRollback = Rollback.Value
            };
            return options;
        }

        private static void LoadAllParameters(IConfigurationRoot configuration, string[] args)
        {
            LoadHelp(args);

            if (Help.Value)
            {
                return;
            }

            LoadUseConfigurationFileParameter(args);
            LoadConnectionString(configuration, args);
            LoadRollback(args);
            LoadRollbackVersion(args);
        }

        private static void LoadRollback(string[] args)
        {
            Rollback.Value = HasArg(args, Rollback);
        }

        private static void LoadRollbackVersion(string[] args)
        {
            if (!Rollback.Value)
            {
                return;
            }

            if (!HasArg(args, RollbackVersion))
            {
                throw new ArgumentException($"Cannot find the parameter {RollbackVersion.Name}");
            }

            RollbackVersion.Value = ReadArg(args, RollbackVersion);
        }

        private static void LoadHelp(string[] args)
        {
            Help.Value = HasArg(args, Help);
        }

        /// <summary>
        /// Verifica se o parâmetro foi informado. Se sim, retorna TRUE
        /// </summary>
        /// <param name="args">Uma Lista de parâmetros</param>
        private static void LoadUseConfigurationFileParameter(string[] args)
        {
            UseConfigurationFile.Value = HasArg(args, UseConfigurationFile);
        }

        public static string PrintHelp()
        {
            string help = "MigrationTool v 1.0.0";
            help += Environment.NewLine;
            help += Environment.NewLine;
            help += $"Common Options:{Environment.NewLine}";
            help += $"{UseConfigurationFile.Name}:            Use a configuration File. This configuration loads the EnvironmentVariables too, overrinding app settings file.{Environment.NewLine}{Environment.NewLine}";
            help += $"{ConnectionString.Name}:                The name of connection string to use, if the {UseConfigurationFile.Name} is set. Otherwise, it's the value of ConnectionString.{Environment.NewLine}{Environment.NewLine}";
            help += $"{Rollback.Name}:                        Instead of apply a migration, rollbacks the database until the version specified.{Environment.NewLine}";
            help += $"                                   If you use this param, you must set the {RollbackVersion.Name} param.{Environment.NewLine}{Environment.NewLine}";
            help += $"{RollbackVersion.Name}:                 The version of the database to which you want to perform the rollback.{Environment.NewLine}{Environment.NewLine}";

            return help;
        }

        public static bool IsPrintHelp(string[] args)
        {
            return HasArg(args, Help);
        }

        private static void LoadConnectionString(IConfiguration configuration, string[] args)
        {
            if (UseConfigurationFile.Value)
            {
                ConnectionString.Value = configuration.GetSection("ConnectionString").Value;
                return;
            }

            if (!HasArg(args, ConnectionString))
            {
                throw new ArgumentException($"Cannot find the parameter {ConnectionString.Name}");
            }

            ConnectionString.Value = ReadArg(args, ConnectionString);
        }

        private static bool HasArg<T>(string[] args, Parameter<T> key)
        {
            return args.ToList().IndexOf(key.Name) >= 0;
        }

        private static string ReadArg<T>(string[] args, Parameter<T> key)
        {
            var index = args.ToList().IndexOf(key.Name);

            if (index < 0)
                return null;
            index++;
            return args[index];
        }
    }

    public class Parameter<T>
    {
        public string Name { get; set; }

        public T Value { get; set; }
    }
}

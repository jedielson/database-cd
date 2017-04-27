using System;
using System.IO;
using Database.CD.Lib;
using Microsoft.Extensions.Configuration;

namespace Migrator
{
    class Program
    {
        // ReSharper disable once UnusedMember.Local
        static void Main(string[] args)
        {
            var configuration = ReadConfiguration();

            if (ParametersReader.IsPrintHelp(args))
            {
                Console.WriteLine(ParametersReader.PrintHelp());
                return;
            }

            var options = ParametersReader.ReadOptions(configuration, args);
            var migrator = new MigrationProgram();
            migrator.Run(options);
        }

        private static IConfigurationRoot ReadConfiguration()
        {
            var pathBase = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                .SetBasePath(pathBase)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables();
            
            return builder.Build();
        }
    }
}
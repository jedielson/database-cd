using System;
using Database.CD.Lib;

namespace Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var options = new ConfigurationOptions
            {
                ConnectionString = "Data Source=JEDI-EWAVE; Initial Catalog=TesteDatabaseCD; Integrated Security=True",
                RevertionVersion = "2017.01.12",
                ExecuteRevertion = true
                //RevertionVersion = "",
                //ExecuteRevertion = false
            };

            var migrator = new MigrationProgram();
            migrator.Run(options);
        }
    }
}
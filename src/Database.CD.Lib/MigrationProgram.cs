using System;

namespace Database.CD.Lib
{
    /// <summary>
    /// Runs the migrations    
    /// </summary>
    public class MigrationProgram
    {
        public void Run(ConfigurationOptions options)
        {
            if (options.ConnectionString == null)
            {
                Console.WriteLine($"Please add a connection string key");
                Console.ReadKey();
                return;
            }


            var manager = new VersionManager(options.ConnectionString);

            var output = !options.ExecuteRevertion 
                ? manager.ExecuteMigrations() 
                : manager.ExecuteReversion(options.RevertionVersion);
            
            foreach (string str in output)
            {
                Console.WriteLine(str);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }        
    }
}

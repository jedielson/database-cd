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
                throw new ArgumentException($"Please add a connection string key");
            }


            var manager = new VersionManager(options.ConnectionString);

            var output = !options.ExecuteRollback 
                ? manager.ExecuteMigrations() 
                : manager.ExecuteReversion(options.RollbackVersion);
            
            foreach (string str in output)
            {
                Console.WriteLine(str);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }        
    }
}

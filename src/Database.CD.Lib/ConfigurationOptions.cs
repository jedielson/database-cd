namespace Database.CD.Lib
{
    public class ConfigurationOptions
    {

        public string ConnectionString { get; set; }

        public string RollbackVersion { get; set; }

        public bool ExecuteRollback { get; set; }

    }
}

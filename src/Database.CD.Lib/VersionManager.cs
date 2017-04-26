using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Database.CD.Lib
{
    internal class VersionManager
    {
        private readonly DbHelper _dbHelper;

        public VersionManager(string connectionString)
        {
            _dbHelper = new DbHelper(connectionString);
        }
        
        public IReadOnlyList<string> ExecuteMigrations()
        {
            var output = new List<string>();
            var currentVersion = GetCurrentVersion();
            output.Add("Current DB schema version is " + currentVersion);

            var migrations = GetNewMigrations(currentVersion);
            output.Add(migrations.Count + " migration(s) found");

            if (HasDuplicateVersion(migrations))
            {
                output.Add("Non-unique migration found...");
                return output;
            }

            foreach (Migration migration in migrations)
            {
                _dbHelper.ExecuteMigration(migration.Content);
                UpdateVersion(migration.Version, migration.Name, migration.Content);
                output.Add($"Executed migration: {migration.Version} - {migration.Name}");
            }

            if (!migrations.Any())
            {
                output.Add("No updates for the current schema version");
            }
            else
            {
                var newVersion = migrations.Last().Version;
                output.Add("New DB schema version is " + newVersion);
            }

            return output;
        }

        public IReadOnlyList<string> ExecuteReversion(string revertVersion)
        {
            var output = new List<string>();
            var migrations = GetRevertionMigration(revertVersion);

            output.Add(migrations.Count + " migration(s) found");

            foreach (Migration migration in migrations)
            {
                _dbHelper.ExecuteMigration(migration.Content);
                RemoveVersion(migration.Version);
                output.Add($"Removed Version: {migration.Version} - {migration.Name}");
            }

            output.Add("New Db schema version is " + GetCurrentVersion());
            return output;

        }

        private static bool HasDuplicateVersion(IReadOnlyList<Migration> migrations)
        {
            var duplicatedVersion = migrations
                .GroupBy(x => x.Version)
                .Count(x => x.Count() > 1);

            return duplicatedVersion > 0;
        }

        private void RemoveVersion(string version)
        {
            _dbHelper.ExecuteNonQuery(@"DELETE FROM dbo.__Migrations WHERE Version = @Version",
                new SqlParameter("Version", version));
        }
        
        private void UpdateVersion(string newVersion, string name, string value)
        {
            _dbHelper.ExecuteNonQuery(@"INSERT dbo.__Migrations (Version, Name, Value, Data)
                                        VALUES (@Version, @Name, @Value, @Data)",
                new SqlParameter("Version", newVersion),
                new SqlParameter("Name", name),
                new SqlParameter("Value", value),
                new SqlParameter("Data", DateTime.UtcNow));
        }
        
        private static IReadOnlyList<Migration> GetNewMigrations(string currentVersion)
        {
            // 01_MyMigration.sql
            var regex = new Regex(@"^(\d)*_(.*)(sql)$");

            return new DirectoryInfo(@"Scripts\Migrations\")
                .GetFiles("*", SearchOption.AllDirectories)
                .Where(x => regex.IsMatch(x.Name))
                .Select(x => new Migration(x))
                .Where(x => string.Compare(x.Version, currentVersion, StringComparison.CurrentCulture) > 0)
                .OrderBy(x => x.Version)
                .ToList();
        }

        private IReadOnlyList<Migration> GetRevertionMigration(string revertVersion)
        {
            // 01_MyMigration.sql
            var regex = new Regex(@"^(\d)*_(.*)(sql)$");

            return new DirectoryInfo(@"Scripts\Revertions\")
                .GetFiles("*", SearchOption.AllDirectories)
                .Where(x => regex.IsMatch(x.Name))
                .Select(x => new Migration(x))
                .Where(x => 
                string.Compare(x.Version, revertVersion, StringComparison.CurrentCulture) >= 0
                && string.Compare(x.Version, GetCurrentVersion(), StringComparison.CurrentCulture) <= 0)
                .OrderByDescending(x => x.Version)
                .ToList();
        }

        private static bool Teste(Migration x, string revert)
        {
            return string.Compare(x.Version, revert, StringComparison.CurrentCulture) > 0;
        }

        private string GetCurrentVersion()
        {
            if (!SettingsTableExists())
            {
                CreateSettingsTable();
                return "0";
            }

            return GetCurrentVersionFromSettingsTable();
        }

        private string GetCurrentVersionFromSettingsTable()
        {
            var version = _dbHelper.ExecuteScalar<string>("select top 1 Version from dbo.__Migrations order by Data desc");
            return version;
        }

        private void CreateSettingsTable()
        {
            var query = @"
                CREATE TABLE dbo.__Migrations
                (
                    Version varchar(50) NOT NULL PRIMARY KEY,
                    Name varchar(200) NOT NULL,
                    Value varchar(max) NOT NULL,
                    Data datetime NOT NULL
                )
                INSERT dbo.__Migrations (Version, Name, Value, Data)
                VALUES (0, 'Zero', '', GETDATE())";

            _dbHelper.ExecuteNonQuery(query);
        }
        
        private bool SettingsTableExists()
        {
            var query = @"
                IF (OBJECT_ID('dbo.__Migrations', 'table') IS NULL)
                SELECT 0
                ELSE SELECT 1";

            return _dbHelper.ExecuteScalar<int>(query) == 1;
        }
    }
}
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Database.CD.Lib
{
    internal class DbHelper
    {
        private readonly string _connectionString;

        public DbHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public T ExecuteScalar<T>(string commandText, params SqlParameter[] parameters)
        {
            using (SqlConnection cnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, cnn)
                {
                    CommandType = CommandType.Text
                })
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                    cnn.Open();

                    return (T)cmd.ExecuteScalar();
                }
            }
        }

        public void ExecuteNonQuery(string commandText, params SqlParameter[] parameters)
        {
            using (SqlConnection cnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, cnn)
                {
                    CommandType = CommandType.Text
                })
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                    cnn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ExecuteMigration(string commandText)
        {
            var regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var subCommands = regex.Split(commandText);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.Connection = connection;
                    cmd.Transaction = transaction;

                    foreach (string command in subCommands)
                    {
                        if (command.Length <= 0)
                            continue;

                        cmd.CommandText = command;
                        cmd.CommandType = CommandType.Text;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                transaction.Commit();
            }
        }
    }
}

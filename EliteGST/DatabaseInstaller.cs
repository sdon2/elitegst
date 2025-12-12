using EliteGST.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteGST
{
    public class DatabaseInstaller
    {
        public DatabaseInstaller(Options options)
        {
            bool install = options.Install;
            string configDatabase = Config.GetStringValue("database", "");

            string database = options.Database;
            if (!string.IsNullOrEmpty(configDatabase))
            {
                database = (database == configDatabase) ? database : configDatabase;
            }

            string financialYear = options.FinancialYear;

            if (install)
            {
                InitDatabase(database);

                Config.SetValue("database", database);

                Console.WriteLine($"Database `{database}` created.");
            }

            if (CreateFinancialYear(database, financialYear))
            {
                Console.WriteLine($"Financial year: `{financialYear}` installed and you're ready to use it!!!");
            }
        }

        private void InitDatabase(string database)
        {
            if (CheckDatabaseExists(database))
            {
                return;
            }

            if (!CreateDatabase(database))
            {
                throw new Exception("Unable to create Database.\n Try creating yourself!");
            }

            if (!ImportMySQL(database))
            {
                throw new Exception("Unable to import Database.\n Try importing yourself!");
            }
        }

        private MySqlConnection GetMySqlConnection(string database = null, bool withDatabase = false)
        {
            var server = Config.GetStringValue("host", "");
            var user = Config.GetStringValue("user", "");
            var password = Config.GetStringValue("password", "");

            string connectionString = $"server={server};user={user};pwd={password};";

            if (withDatabase)
            {
                connectionString += $"database={database};";
            }

            return new MySqlConnection(connectionString);
        }

        public bool CheckDatabaseExists(string database)
        {
            string query = $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @dbName;";

            using (MySqlConnection connection = GetMySqlConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@dbName", database);
                        object result = command.ExecuteScalar();
                        return result != null;
                    }
                    catch (MySqlException)
                    {
                        return false;
                    }
                }
            }
        }

        public bool CreateDatabase(string database)
        {
            string query = $"CREATE DATABASE `{database}`;";

            using (MySqlConnection connection = GetMySqlConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (MySqlException)
                    {
                        return false;
                    }
                }
            }
        }

        public bool ImportMySQL(string database)
        {
            var statements = Database.DbDump;

            using (MySqlConnection conn = GetMySqlConnection(database, true))
            {
                conn.Open();
                MySqlTransaction transaction = conn.BeginTransaction();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = transaction;

                try
                {
                    foreach (var statement in statements)
                    {
                        // Trim whitespace and skip empty statements
                        string trimmedStatement = statement.Trim();
                        if (!string.IsNullOrWhiteSpace(trimmedStatement))
                        {
                            cmd.CommandText = trimmedStatement;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public bool CreateFinancialYear(string database, string financialYear)
        {
            //INSERT INTO `financial_years` (`FYName`) VALUES ('2025-2026');
            string query = $"INSERT INTO `financialyears` (`FinancialYearString`) VALUES (@financialYear);";

            using (MySqlConnection connection = GetMySqlConnection(database, true))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@financialYear", financialYear);
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (MySqlException)
                    {
                        return false;
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;


namespace EliteGST.Data
{
    public class Database
    {
        private static Dictionary<string, string> _credentials = new Dictionary<string, string>();

        private static IDbConnection _connection { get; set; }

        public static void SetCredentials(string host, string database, string user, string password)
        {
            _credentials["host"] = host;
            _credentials["database"] = database;
            _credentials["user"] = user;
            _credentials["password"] = password;
        }

        public static string GetCredentials(string name)
        {
            return _credentials[name];
        }

        public static IDbConnection Connection
        {
            get
            {
                if (_connection != null)
                {
                    if (_connection.State == System.Data.ConnectionState.Open) return _connection;
                    else
                    {
                        _connection.Open();
                        return _connection;
                    }
                }
                else
                {
                    _connection = new MySqlConnection(String.Format("server={0};database={1};uid={2};pwd={3}", _credentials["host"], _credentials["database"], _credentials["user"], _credentials["password"]));
                    _connection.Open();
                    return _connection;
                }
            }
        }

        public static string[] DbDump
        {
            get
            {
                var db = Properties.Resources.db;
                // Split the script into individual statements by semicolon
                // Note: This regex might need adjustment for complex SQL files (e.g., stored procedures with semicolons)
                return Regex.Split(db, @";[\r\n]*", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            }
        }
            

        public static bool CreateDatabaseIfNotExists()
        {
            var server = _credentials["host"];
            var user = _credentials["user"];
            var password = _credentials["password"];
            string connectionString = $"server={server};user={user};pwd={password};";

            var database = _credentials["database"];
            string query = $"CREATE DATABASE IF NOT EXISTS `{database}`;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
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

        public static bool ImportMySQL()
        {
            var server = _credentials["host"];
            var user = _credentials["user"];
            var password = _credentials["password"];
            var database = _credentials["database"];
            string connectionString = $"server={server};user={user};pwd={password};database={database}";

            var statements = DbDump;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
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
    }
}

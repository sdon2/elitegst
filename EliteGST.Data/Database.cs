using System;
using System.Collections.Generic;
using System.Data;
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
    }
}

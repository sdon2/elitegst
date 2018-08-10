using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;


namespace EliteGST.Data
{
    public class Database
    {
        private static string _host { get; set; }
        private static string _database { get; set; }
        
        private static IDbConnection _connection { get; set; }

        public static void SetHost(string host)
        {
            _host = host;
        }

        public static string GetHost()
        {
            return _host;
        }

        public static void SetDatabase(string database)
        {
            _database = database;
        }

        public static string GetDatabase()
        {
            return _database;
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
                        return Connection;
                    }
                }
                else
                {
                    _connection = new MySqlConnection(String.Format("server={0};uid=root;pwd=root;database={1}" , _host, _database));
                    _connection.Open();
                    return _connection;
                }
            }
        }
    }
}

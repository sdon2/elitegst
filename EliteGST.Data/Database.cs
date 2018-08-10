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
        public static string Name { get; set; }

        private static IDbConnection _connection { get; set; }

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
                    _connection = new MySqlConnection("server=elitegstserver;uid=root;pwd=root;database=" + Name);
                    _connection.Open();
                    return _connection;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace EliteGST.DatabaseTool
{
    class Database
    {
        public static MySqlConnection getConnection(string host)
        {
            var connection = new MySqlConnection(String.Format("server={0};uid=root;pwd=root", host));
            connection.Open();
            return connection;
        }
    }
}

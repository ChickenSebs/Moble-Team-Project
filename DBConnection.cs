using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace calendar4
{
    internal class DBConnection
    {
        private readonly string connectionString =
            "Server=localhost;Database=teamproject;Uid=root;Pwd=1111;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
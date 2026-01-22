using System;
using MySql.Data.MySqlClient;

namespace ProjectNet
{
    public static class Database
    {
        
        private static string connectionString = "Server=localhost;Database=cabinet_medical;Uid=root;Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}

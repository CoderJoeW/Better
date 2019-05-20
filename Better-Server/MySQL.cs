using System;
using MySql.Data.MySqlClient;

namespace Better_Server {
    public class MySQL {
        public static MySQLSettings mySQLSettings;

        public static MySqlConnection GetConn() {
            return new MySqlConnection(CreateConnectionString());
        }

        public static void OpenConnection(MySqlConnection conn) {
            conn.Open();
            Console.WriteLine("Connection to MySQL Server has been opened");
        }

        public static void CloseConnection(MySqlConnection conn) {
            conn.Close();
            Console.WriteLine("Connection to MySQL Server has been terminated");
        }

        private static string CreateConnectionString() {
            var db = mySQLSettings;
            string connectionString = "SERVER=" + db.server + ";" +
                "DATABASE=" + db.database + ";" +
                "UID=" + db.user + ";" +
                "PASSWORD=" + db.password + ";";
            return connectionString;
        }
    }

    public struct MySQLSettings {
        public string server;
        public string database;
        public string user;
        public string password;
    }
}

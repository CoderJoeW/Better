using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Better_Server {
    class Database {
        public static string GetIdentifier(string identifier) {
            string query = "SELECT uid FROM users WHERE uid='" + identifier + "'";

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            string uid = "";

            while (reader.Read()) {
                uid = (string)reader["uid"];
            }

            reader.Close();

            return uid;
        }

        public static void CreateAccount(string identifier) {
            string query = "INSERT INTO users SET uid='" + identifier + "' ,username='new_user'";

            Console.WriteLine("Create Account has been called");

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);

            try {
                cmd.ExecuteNonQuery();
            }catch(Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

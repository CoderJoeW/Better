using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

public class Lobby {
    public int ID { set; get; }
    public string UID { set; get; }
    public int Bet { set; get; }
    public string Game { set; get; }
}

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

        public static void CreateLobby(string uid,int bet,string game) {
            string query = "INSERT INTO que SET player1_uid='" + uid + "' , bet=" + bet + " , game='" + game + "'";

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);

            try {
                cmd.ExecuteNonQuery();
            }catch(Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public static string GetLobbyList() {
            string query = "SELECT * FROM que";

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            List<Lobby> items = new List<Lobby>();

            while (reader.Read()) {
                items.Add(new Lobby {
                    ID = (int)reader["id"],
                    UID = (string)reader["player1_uid"],
                    Bet = (int)reader["bet"],
                    Game = (string)reader["game"]
                });
            }

            reader.Close();

            var jsonPacket = JsonConvert.SerializeObject(items);

            return jsonPacket;
        }
    }
}

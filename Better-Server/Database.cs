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

public class MatchInfo {
    public int Player1ID { set; get; }
    public string Player1UID { set; get; }

    public int Player2ID { set; get; }
    public string Player2UID { set; get; }

    public int Player1Score { set; get; }
    public int Player2Score { set; get; }

    public int Bet { set; get; }

    public string Winner { set; get; }
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

        public static void CreateLobby(string uid,int bet,string game,int conID) {
            string query = "INSERT INTO que SET player1_uid='" + uid + "' , bet=" + bet + " , game='" + game + "' , player1_conID=" + conID;

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);

            try {
                cmd.ExecuteNonQuery();
            }catch(Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void JoinLobby(int matchID,string uid,int conID) {
            string query = "UPDATE que SET player2_uid='" + uid + "' , player2_conID=" + conID + " WHERE id=" + matchID;

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);

            try {
                cmd.ExecuteNonQuery();
            }catch(Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void UpdateGameScore(int matchID,string player,int score) {
            string query = "";

            if(player == "Player1") {
                query = "UPDATE que SET player1_score=" + score + " , player1_gameOver=1 WHERE id=" + matchID;
                Console.WriteLine("Player1 score has been updated");
            }else if(player == "Player2") {
                query = "UPDATE que SET player2_score=" + score + " , player2_gameOver=1 WHERE id=" + matchID;
                Console.WriteLine("Player2 score has been updated");
            }

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);

            try {
                cmd.ExecuteNonQuery();
            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }

        public static bool IsMatchOver(int matchID) {
            string query = "SELECT player1_gameOver,player2_gameOver FROM que WHERE id=" + matchID;

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            bool player1_gameOver = false;
            bool player2_gameOver = false;

            while (reader.Read()) {
                player1_gameOver = (bool)reader["player1_gameOver"];
                player2_gameOver = (bool)reader["player2_gameOver"];
            }

            reader.Close();

            if(player1_gameOver && player2_gameOver) {
                return true;
            } else {
                return false;
            }
        }

        public static string GetMatchInfo(int matchID) {
            string query = "SELECT * FROM que WHERE id=" + matchID;

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            string player1_uid = "";
            string player2_uid = "";

            int player1_conID = 0;
            int player2_conID = 0;

            int player1_score = 0;
            int player2_score = 0;

            int bet = 0;

            while (reader.Read()) {
                player1_uid = (string)reader["player1_uid"];
                player2_uid = (string)reader["player2_uid"];

                player1_conID = (int)reader["player1_conID"];
                player2_conID = (int)reader["player2_conID"];

                player1_score = (int)reader["player1_score"];
                player2_score = (int)reader["player2_score"];

                bet = (int)reader["bet"];
            }

            reader.Close();

            Console.WriteLine("Player1 Score: " + player1_score);
            Console.WriteLine("Player2 Score: " + player2_score);

            MatchInfo matchInfo = new MatchInfo();
            matchInfo.Player1UID = player1_uid;
            matchInfo.Player2UID = player2_uid;
            matchInfo.Player1ID = player1_conID;
            matchInfo.Player2ID = player2_conID;
            matchInfo.Player1Score = player1_score;
            matchInfo.Player2Score = player2_score;
            matchInfo.Bet = bet;

            if(player1_score > player2_score) {
                //Player 1 wins
                matchInfo.Winner = "Player1";
            }else if(player2_score > player1_score) {
                //Player 2 wins
                matchInfo.Winner = "Player2";
            } else {
                //Tie game
                matchInfo.Winner = "Draw";
            }

            string jsonPacket = JsonConvert.SerializeObject(matchInfo);

            return jsonPacket;
        }

        public static void GiveMoney(string uid,int amount){
            string query = "UPDATE users SET balance=balance+" + amount + " WHERE uid='" + uid + "'";

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void SubractMoney(string uid, int amount)
        {
            string query = "UPDATE users SET balance=balance-" + amount + " WHERE uid='" + uid + "'";

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static int GetPlayer1ID(int matchID) {
            string query = "SELECT player1_conID FROM que WHERE id=" + matchID;

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            int id = 0;

            while (reader.Read()) {
                id = (int)reader["player1_conID"];
            }

            reader.Close();

            return id;
        }

        public static int GetPlayer2ID(int matchID) {
            string query = "SELECT player2_conID FROM que WHERE id=" + matchID;

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            int id = 0;

            while (reader.Read()) {
                id = (int)reader["player2_conID"];
            }

            reader.Close();

            return id;
        }

        public static string GetMatchGame(int matchID) {
            string query = "SELECT game FROM que WHERE id=" + matchID;

            MySqlCommand cmd = new MySqlCommand(query, MySQL.mySQLSettings.connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            string game = "";

            while (reader.Read()) {
                game = (string)reader["game"];
            }

            reader.Close();

            return game;
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

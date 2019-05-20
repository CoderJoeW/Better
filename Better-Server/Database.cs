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

public class User {
    public string Username { set; get; }
}

public class MatchInfo {
    public int MatchID { set; get; }
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

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query,conn)) {
                using(MySqlDataReader reader = cmd.ExecuteReader()) {
                    string uid = "";

                    while (reader.Read()) {
                        uid = (string)reader["uid"];
                    }

                    reader.Close();

                    conn.Close();

                    return uid;
                }
            }
        }

        public static void CreateAccount(string identifier) {
            string query = "INSERT INTO users SET uid='" + identifier + "' ,username='new_user'";

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                try {
                    cmd.ExecuteNonQuery();
                } catch (Exception e) {
                    Database.LogError(e.Message);
                }
            }

            conn.Close();
        }

        public static void CreateLobby(string uid,int bet,string game,int conID) {
            string query = "INSERT INTO que SET player1_uid='" + uid + "' , bet=" + bet + " , game='" + game + "' , player1_conID=" + conID;

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                try {
                    cmd.ExecuteNonQuery();
                } catch (Exception e) {
                    Database.LogError(e.Message);
                }
            }

            conn.Close();
        }

        public static void JoinLobby(int matchID,string uid,int conID) {
            string query = "UPDATE que SET player2_uid='" + uid + "' , player2_conID=" + conID + " WHERE id=" + matchID;

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                try {
                    cmd.ExecuteNonQuery();
                } catch (Exception e) {
                    Database.LogError(e.Message);
                }
            }

            conn.Close();
        }

        public static void RemoveLobby(int matchID){
            string query = "DELETE FROM que WHERE id=" + matchID;

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                try {
                    cmd.ExecuteNonQuery();
                } catch (Exception e) {
                    Database.LogError(e.Message);
                }
            }

            conn.Close();
        }

        public static int GetLobbyID(string uid) {
            string query = "SELECT id FROM que WHERE player1_uid='" + uid + "'";

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                using (MySqlDataReader reader = cmd.ExecuteReader()) {
                    int matchID = 0;

                    while (reader.Read()) {
                        matchID = (int)reader["id"];
                    }

                    reader.Close();
                    conn.Close();
                    return matchID;
                }
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

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                try {
                    cmd.ExecuteNonQuery();
                } catch (Exception e) {
                    Database.LogError(e.Message);
                }
            }

            conn.Close();
        }

        public static bool IsMatchOver(int matchID) {
            string query = "SELECT player1_gameOver,player2_gameOver FROM que WHERE id=" + matchID;

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                using(MySqlDataReader reader = cmd.ExecuteReader()) {
                    bool player1_gameOver = false;
                    bool player2_gameOver = false;

                    while (reader.Read()) {
                        player1_gameOver = (bool)reader["player1_gameOver"];
                        player2_gameOver = (bool)reader["player2_gameOver"];
                    }

                    reader.Close();
                    conn.Close();
                    if (player1_gameOver && player2_gameOver) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        }

        public static string GetMatchInfo(int matchID) {
            string query = "SELECT * FROM que WHERE id=" + matchID;

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                using(MySqlDataReader reader = cmd.ExecuteReader()) {
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
                    matchInfo.MatchID = matchID;
                    matchInfo.Player1UID = player1_uid;
                    matchInfo.Player2UID = player2_uid;
                    matchInfo.Player1ID = player1_conID;
                    matchInfo.Player2ID = player2_conID;
                    matchInfo.Player1Score = player1_score;
                    matchInfo.Player2Score = player2_score;
                    matchInfo.Bet = bet;

                    if (player1_score > player2_score) {
                        //Player 1 wins
                        matchInfo.Winner = "Player1";
                    } else if (player2_score > player1_score) {
                        //Player 2 wins
                        matchInfo.Winner = "Player2";
                    } else {
                        //Tie game
                        matchInfo.Winner = "Draw";
                    }

                    string jsonPacket = JsonConvert.SerializeObject(matchInfo);

                    conn.Close();
                    return jsonPacket;
                }
            }
        }

        public static void GiveMoney(string uid,int amount){
            string query = "UPDATE users SET balance=balance+" + amount + " WHERE uid='" + uid + "'";

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                try {
                    cmd.ExecuteNonQuery();
                } catch (Exception e) {
                    Database.LogError(e.Message);
                }
            }

            conn.Close();
        }

        public static void SubractMoney(string uid, int amount)
        {
            string query = "UPDATE users SET balance=balance-" + amount + " WHERE uid='" + uid + "'";

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                try {
                    cmd.ExecuteNonQuery();
                } catch (Exception e) {
                    Database.LogError(e.Message);
                }
            }

            conn.Close();
        }

        public static int GetBalance(string uid) {
            string query = "SELECT balance FROM users WHERE uid='" + uid + "'";

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                using(MySqlDataReader reader = cmd.ExecuteReader()) {
                    int balance = 0;

                    while (reader.Read()) {
                        balance = (int)reader["balance"];
                    }
                    reader.Close();

                    conn.Close();

                    return balance;
                }
            }
        }

        public static int GetPlayer1ID(int matchID) {
            string query = "SELECT player1_conID FROM que WHERE id=" + matchID;

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                using(MySqlDataReader reader = cmd.ExecuteReader()) {
                    int id = 0;

                    while (reader.Read()) {
                        id = (int)reader["player1_conID"];
                    }

                    reader.Close();

                    conn.Close();

                    return id;
                }
            }
        }

        public static int GetPlayer2ID(int matchID) {
            string query = "SELECT player2_conID FROM que WHERE id=" + matchID;

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                using(MySqlDataReader reader = cmd.ExecuteReader()) {
                    int id = 0;

                    while (reader.Read()) {
                        id = (int)reader["player2_conID"];
                    }

                    reader.Close();

                    conn.Close();

                    return id;
                }
            }
        }

        public static string GetMatchGame(int matchID) {
            string query = "SELECT game FROM que WHERE id=" + matchID;

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                using(MySqlDataReader reader = cmd.ExecuteReader()) {
                    string game = "";

                    while (reader.Read()) {
                        game = (string)reader["game"];
                    }

                    reader.Close();

                    conn.Close();

                    return game;
                }
            }
        }

        public static void UpdateOnlineStatus(string uid,bool isOnline) {
            string query = "UPDATE users SET isOnline=" + isOnline + " WHERE uid='" + uid + "'";

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                try {
                    cmd.ExecuteNonQuery();
                } catch (Exception e) {
                    Database.LogError(e.Message);
                }
            }

            conn.Close();
        }

        public static string GetUsersOnline() {
            string query = "SELECT * FROM users WHERE isOnline=1";

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using(MySqlCommand cmd = new MySqlCommand(query, conn)) {
                using (MySqlDataReader reader = cmd.ExecuteReader()) {
                    List<User> users = new List<User>();

                    while (reader.Read()) {
                        users.Add(new User {
                            Username = (string)reader["username"]
                        });
                    }

                    reader.Close();
                    conn.Close();

                    var jsonPacket = JsonConvert.SerializeObject(users);

                    return jsonPacket;
                }
            }
        }

        public static string GetLobbyList() {
            string query = "SELECT * FROM que WHERE player2_uid='' or player2_uid is NULL";

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                using (MySqlDataReader reader = cmd.ExecuteReader()) {
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

                    conn.Close();

                    var jsonPacket = JsonConvert.SerializeObject(items);

                    return jsonPacket;
                }
            }
        }

        public static void LogError(string errorMessage) {
            String timeStamp = lib.GetTimestamp(DateTime.Now);
            string query = "INSERT INTO error_logs SET error='" + errorMessage + "', timestamp='" + timeStamp + "'";

            MySqlConnection conn = MySQL.GetConn();
            conn.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, conn)) {
                try {
                    cmd.ExecuteNonQuery();
                } catch (Exception e) {
                    Database.LogError(e.Message);
                }
            }

            conn.Close();
        }
    }
}

using System;
using System.Collections.Generic;

namespace Better_Server {
    public class ServerHandleData {
        public delegate void Packet_(int connectionID, byte[] data);
        public static Dictionary<int, Packet_> packetListener;
        private static int pLength;

        public static void InitializePacketListener() {
            packetListener = new Dictionary<int, Packet_>();
            packetListener.Add((int)ClientPackages.CThankYouMsg, HandleThankYou);
            packetListener.Add((int)ClientPackages.CCheckForAccount, HandleCheckForAccount);
            packetListener.Add((int)ClientPackages.CCreateAccount, HandleCreateAccount);
            packetListener.Add((int)ClientPackages.CGameOver, HandleGameOver);
            packetListener.Add((int)ClientPackages.CCreateLobby, HandleCreateLobby);
            packetListener.Add((int)ClientPackages.CRefreshLobbyList, HandleRefreshLobbyList);
            packetListener.Add((int)ClientPackages.CJoinLobby, HandleJoinLobby);
        }

        public static void HandleData(int connectionID, byte[] data) {
            //Copying our packet info into a temporary array to edit/peek
            byte[] buffer = (byte[])data.Clone();

            //Check if connected player has an instance of the byte buffer
            if (ServerTCP.clientObjects[connectionID].buffer == null) {
                //If there is no instance create a new instance
                ServerTCP.clientObjects[connectionID].buffer = new ByteBuffer();
            }

            //Read package from player
            ServerTCP.clientObjects[connectionID].buffer.WriteBytes(buffer);

            //Check if recieved package is empty, if so dont continue executing
            if (ServerTCP.clientObjects[connectionID].buffer.Count() == 0) {
                ServerTCP.clientObjects[connectionID].buffer.Clear();
                return;
            }

            //Check if package contains info
            if (ServerTCP.clientObjects[connectionID].buffer.Length() >= 4) {
                //if so read full package length
                pLength = ServerTCP.clientObjects[connectionID].buffer.ReadInteger(false);

                if (pLength <= 0) {
                    //If there is no package or package is invalid close this method
                    ServerTCP.clientObjects[connectionID].buffer.Clear();
                    return;
                }
            }

            while (pLength > 0 & pLength <= ServerTCP.clientObjects[connectionID].buffer.Length() - 4) {
                if (pLength <= ServerTCP.clientObjects[connectionID].buffer.Length() - 4) {
                    ServerTCP.clientObjects[connectionID].buffer.ReadInteger();
                    data = ServerTCP.clientObjects[connectionID].buffer.ReadBytes(pLength);
                    HandleDataPackages(connectionID, data);
                }

                pLength = 0;

                if (ServerTCP.clientObjects[connectionID].buffer.Length() >= 4) {
                    pLength = ServerTCP.clientObjects[connectionID].buffer.ReadInteger(false);

                    if (pLength <= 0) {
                        //If there is no package or package is invalid close this method
                        ServerTCP.clientObjects[connectionID].buffer.Clear();
                        return;
                    }
                }

                if (pLength <= 1) {
                    ServerTCP.clientObjects[connectionID].buffer.Clear();
                }
            }
        }

        private static void HandleDataPackages(int connectionID, byte[] data) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packageID = buffer.ReadInteger();

            if (packetListener.TryGetValue(packageID, out Packet_ packet)) {
                packet.Invoke(connectionID, data);
            }
        }

        private static void HandleThankYou(int connectionID, byte[] data) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packageID = buffer.ReadInteger();
            string msg = buffer.ReadString();

            Console.WriteLine("Player {0} has sent following msg {1}", connectionID, msg);
        }

        private static void HandleCheckForAccount(int connectionID, byte[] data) {
            Console.WriteLine("Request recieved checking for account");
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packageID = buffer.ReadInteger();
            string uid = buffer.ReadString();

            string result = Database.GetIdentifier(uid);

            if(result == null || result == "" || result == string.Empty) {
                Console.WriteLine("Account did not exist notifing client");
                ServerTCP.PACKET_AccountExist(connectionID, "false");
            }else {
                Console.WriteLine("Account did exist notifying client");
                ServerTCP.PACKET_AccountExist(connectionID, "true");
            }
        }

        private static void HandleCreateAccount(int connectionID, byte[] data) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packageID = buffer.ReadInteger();
            string uid = buffer.ReadString();

            Database.CreateAccount(uid);

            HandleCheckForAccount(connectionID,data);
        }

        private static void HandleGameOver(int connectionID,byte[] data) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packageID = buffer.ReadInteger();
            int matchID = buffer.ReadInteger();
            string player = buffer.ReadString();
            int score = buffer.ReadInteger();

            Database.UpdateGameScore(matchID, player, score);

            if (Database.IsMatchOver(matchID)) {
                ServerTCP.PACKET_MatchOver(matchID,Database.GetMatchInfo(matchID));
            }
        }

        private static void HandleCreateLobby(int connectionID, byte[] data) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packageID = buffer.ReadInteger();
            string uid = buffer.ReadString();
            int bet = buffer.ReadInteger();
            string game = buffer.ReadString();

            Database.CreateLobby(uid, bet, game,connectionID);

            ServerTCP.PACKET_LobbyCreated(connectionID);
        }

        private static void HandleRefreshLobbyList(int connectionID,byte[] data) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packageID = buffer.ReadInteger();

            string lobbyList = Database.GetLobbyList();

            ServerTCP.PACKET_SendLobbyList(connectionID, lobbyList);
        }

        private static void HandleJoinLobby(int connectionID,byte[] data) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);

            int packageID = buffer.ReadInteger();

            string uid = buffer.ReadString();

            int matchID = buffer.ReadInteger();

            Console.WriteLine("Player: " + uid + " Is attempting to join match ID: " + matchID);

            Database.JoinLobby(matchID, uid, connectionID);

            int player1_id = Database.GetPlayer1ID(matchID);
            int player2_id = Database.GetPlayer2ID(matchID);
            string game = Database.GetMatchGame(matchID);

            ServerTCP.PACKET_PlayerJoined(player1_id, player2_id, matchID,game);
        }
    }
}

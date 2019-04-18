using System;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Better_Server {
    class ServerTCP {
        private static TcpListener serverSocket;

        public static ClientObject[] clientObjects;

        public static void InitializeServer() {
            InitializeMySQLServer();
            InitializeClientObjects();
            InitializeServerSocket();
        }

        private static void InitializeMySQLServer() {
            MySQL.mySQLSettings.user = "root";
            MySQL.mySQLSettings.password = "";
            MySQL.mySQLSettings.server = "localhost";
            MySQL.mySQLSettings.database = "better";

            MySQL.ConnectToMySQL();
        }

        private static void InitializeServerSocket() {
            serverSocket = new TcpListener(IPAddress.Any, Constants.PORT);
            serverSocket.Start();
            serverSocket.BeginAcceptTcpClient(new AsyncCallback(ClientConnectCallback), null);
        }

        private static void InitializeClientObjects() {
            clientObjects = new ClientObject[Constants.MAX_PLAYERS];

            for (int i = 1; i < Constants.MAX_PLAYERS; i++) {
                clientObjects[i] = new ClientObject(null, 0);
            }
        }

        private static void ClientConnectCallback(IAsyncResult result) {
            TcpClient tempClient = serverSocket.EndAcceptTcpClient(result);
            serverSocket.BeginAcceptTcpClient(new AsyncCallback(ClientConnectCallback), null);

            for (int i = 1; i < Constants.MAX_PLAYERS; i++) {
                if(clientObjects[i].socket == null) {
                    clientObjects[i] = new ClientObject(tempClient, i);

                    ServerTCP.PACKET_WelcomeMsg(i, "Welcome to the server");

                    return;
                }
            }
        }

        public static void SendDataTo(int connectionID, byte[] data) {
            ByteBuffer buffer = new ByteBuffer();

            buffer.WriteInteger((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
            buffer.WriteBytes(data);
            clientObjects[connectionID].myStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
            buffer.Dispose();
        }

        public static void PACKET_WelcomeMsg(int connectionID,string msg) {
            ByteBuffer buffer = new ByteBuffer();
            //add package id
            buffer.WriteInteger((int)ServerPackages.SWelcomeMsg);

            //send info
            buffer.WriteString(msg);

            SendDataTo(connectionID, buffer.ToArray());
        }

        public static void PACKET_AccountExist(int connectionID,string result) {
            ByteBuffer buffer = new ByteBuffer();
            //Add package id
            buffer.WriteInteger((int)ServerPackages.SAccountExist);

            //Send info
            buffer.WriteString(result);

            SendDataTo(connectionID, buffer.ToArray());
        }

        public static void PACKET_LobbyCreated(int connectionID) {
            ByteBuffer buffer = new ByteBuffer();

            //Add package id
            buffer.WriteInteger((int)ServerPackages.SLobbyCreated);

            SendDataTo(connectionID,buffer.ToArray());
        }

        public static void PACKET_SendLobbyList(int connectionID,string data) {
            ByteBuffer buffer = new ByteBuffer();

            //Add package id
            buffer.WriteInteger((int)ServerPackages.SSendLobbyList);

            //Send info
            buffer.WriteString(data);

            SendDataTo(connectionID, buffer.ToArray());
        }

        public static void PACKET_PlayerJoined(int player1_conID,int player2_conID,int matchID,string game) {
            ByteBuffer buffer1 = new ByteBuffer();

            //Add package id
            buffer1.WriteInteger((int)ServerPackages.SPlayerJoined);

            //Send info
            buffer1.WriteInteger(matchID);
            buffer1.WriteString(game);
            buffer1.WriteString("Player1");

            SendDataTo(player1_conID,buffer1.ToArray());

            ByteBuffer buffer2 = new ByteBuffer();

            //Add package id
            buffer2.WriteInteger((int)ServerPackages.SPlayerJoined);

            //Send info
            buffer2.WriteInteger(matchID);
            buffer2.WriteString(game);
            buffer2.WriteString("Player2");

            SendDataTo(player2_conID, buffer2.ToArray());
        }

        public static void PACKET_MatchOver(int matchID,string jsonPacket) {
            MatchInfo matchInfo = JsonConvert.DeserializeObject<MatchInfo>(jsonPacket);

            // Console.WriteLine("Player 1 score: " + matchInfo.Player1Score);
            //Console.WriteLine("Player 2 score: " + matchInfo.Player2Score);

            float commision = (matchInfo.Bet * 2) * 0.20f;
            int winningAmount = (matchInfo.Bet * 2) - (int)commision;

            //Give money to winner
            if(matchInfo.Winner == "Player1")
            {
                Database.GiveMoney(matchInfo.Player1UID, winningAmount);
                Database.SubractMoney(matchInfo.Player2UID, matchInfo.Bet);
            }else if(matchInfo.Winner == "Player2")
            {
                Database.GiveMoney(matchInfo.Player2UID, winningAmount);
                Database.SubractMoney(matchInfo.Player1UID, matchInfo.Bet);
            }

            //Send both players match info
            ByteBuffer buffer = new ByteBuffer();

            //Package id
            buffer.WriteInteger((int)ServerPackages.SMatchOver);
            buffer.WriteString(jsonPacket);

            SendDataTo(matchInfo.Player1ID, buffer.ToArray());
            SendDataTo(matchInfo.Player2ID, buffer.ToArray());

        }
    }
}

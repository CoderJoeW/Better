using System;
using System.Net.Sockets;

public class ClientTCP {
	private static TcpClient clientSocket;
	private static NetworkStream myStream;
	private static byte[] recieveBuffer;

	public static void InitializeClientSocket(string address,int port){
		clientSocket = new TcpClient ();
		clientSocket.ReceiveBufferSize = 4096;
		clientSocket.SendBufferSize = 4096;
		recieveBuffer = new byte[4096 * 2];
		clientSocket.BeginConnect (address, port, new AsyncCallback (ClientConnectCallback), clientSocket);
	}

	private static void ClientConnectCallback(IAsyncResult result){
		clientSocket.EndConnect (result);
		if (clientSocket.Connected == false) {
			return;
		} else {
			myStream = clientSocket.GetStream ();
			myStream.BeginRead (recieveBuffer, 0, 4096 * 2, RecieveCallback, null);
		}
	}

	private static void RecieveCallback(IAsyncResult result){
		try{
			int readBytes = myStream.EndRead(result);

			if(readBytes <= 0){
				return;
			}

			byte[] newBytes = new byte[readBytes];
			Buffer.BlockCopy(recieveBuffer,0,newBytes,0,readBytes);

			UnityThread.executeInUpdate(() => {
				ClientHandleData.HandleData(newBytes);
			});

			myStream.BeginRead (recieveBuffer, 0, 4096 * 2, RecieveCallback, null);
		}catch(Exception){
			throw;
		}
	}

	public static void SendData(byte[] data){
		ByteBuffer buffer = new ByteBuffer();

		buffer.WriteInteger((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
		buffer.WriteBytes(data);
		myStream.Write(buffer.ToArray(), 0, buffer.ToArray().Length);
		buffer.Dispose();
	}
    
	public static void PACKET_ThankYou(){
		ByteBuffer buffer = new ByteBuffer ();
		buffer.WriteInteger ((int)ClientPackages.CThankYouMsg);
        buffer.WriteString(UnityEngine.SystemInfo.deviceUniqueIdentifier);
		buffer.WriteString ("Thank You");
		SendData (buffer.ToArray ());
	}

    public static void PACKET_CheckForAccount() {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CCheckForAccount);
        buffer.WriteString(UnityEngine.SystemInfo.deviceUniqueIdentifier);
        SendData(buffer.ToArray());
    }

    public static void PACKET_CreateAccount() {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CCreateAccount);
        buffer.WriteString(UnityEngine.SystemInfo.deviceUniqueIdentifier);
        SendData(buffer.ToArray());
    }

    public static void PACKET_GameOver(int score) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CGameOver);
        buffer.WriteInteger(GameManager.Instance.matchID);
        buffer.WriteString(GameManager.Instance.player);
        buffer.WriteInteger(score);
        SendData(buffer.ToArray());
    }

    public static void PACKET_CreateLobby(int bet,string game) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CCreateLobby);
        buffer.WriteString(UnityEngine.SystemInfo.deviceUniqueIdentifier);
        buffer.WriteInteger(bet);
        buffer.WriteString(game);
        SendData(buffer.ToArray());
    }

    public static void PACKET_RefreshLobbyList() {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CRefreshLobbyList);
        SendData(buffer.ToArray());
    }

    public static void PACKET_RefresUsersOnlineList() {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CRefreshUsersOnlineList);
        SendData(buffer.ToArray());
    }

    public static void PACKET_JoinLobby(int lobbyID) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CJoinLobby);
        buffer.WriteString(UnityEngine.SystemInfo.deviceUniqueIdentifier);
        buffer.WriteInteger(lobbyID);
        SendData(buffer.ToArray());
    }

    public static void PACKET_LeaveLobby() {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CLeaveLobby);
        buffer.WriteString(UnityEngine.SystemInfo.deviceUniqueIdentifier);
        SendData(buffer.ToArray());
    }

    public static void PACKET_GetBalance() {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CGetBalance);
        buffer.WriteString(UnityEngine.SystemInfo.deviceUniqueIdentifier);
        SendData(buffer.ToArray());
    }

    public static void PACKET_CheckVersion(string version) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CCheckVersion);
        buffer.WriteString(version);
        SendData(buffer.ToArray());
    }

    public static void PACKET_PurchaseCompleted(string itemName) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CPurchaseCompleted);
        buffer.WriteString(itemName);
        buffer.WriteString(UnityEngine.SystemInfo.deviceUniqueIdentifier);
        SendData(buffer.ToArray());
    }

    public static void PACKET_DisconnectFromServer() {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackages.CDisconnectClient);
        buffer.WriteString(UnityEngine.SystemInfo.deviceUniqueIdentifier);
        SendData(buffer.ToArray());
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

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

public class ClientHandleData {
	private static ByteBuffer playerBuffer;
	public delegate void Packet_(byte[] data);
	public static Dictionary<int, Packet_> packetListener;
	private static int pLength;

	public static void InitializePacketListener() {
		packetListener = new Dictionary<int, Packet_>();
		packetListener.Add ((int)ServerPackages.SWelcomeMsg,HandleWelcomeMsg);
        packetListener.Add((int)ServerPackages.SAccountExist, HandleAccountExist);
        packetListener.Add((int)ServerPackages.SLobbyCreated, HandleLobbyCreated);
        packetListener.Add((int)ServerPackages.SSendLobbyList, HandleSendLobbyList);
        packetListener.Add((int)ServerPackages.SSendUsersOnlineList, HandleSendUsersOnlineList);
        packetListener.Add((int)ServerPackages.SPlayerJoined, HandlePlayerJoined);
        packetListener.Add((int)ServerPackages.SMatchOver, HandleMatchOver);
        packetListener.Add((int)ServerPackages.SGetBalance, HandleGetBalance);
        packetListener.Add((int)ServerPackages.SNoUpdate, HandleNoUpdateAvailable);
        packetListener.Add((int)ServerPackages.SUpdate, HandleUpdateAvailable);
	}

	public static void HandleData(byte[] data) {
		//Copying our packet info into a temporary array to edit/peek
		byte[] buffer = (byte[])data.Clone();

		//Check if connected player has an instance of the byte buffer
		if(playerBuffer == null) {
			//If there is no instance create a new instance
			playerBuffer = new ByteBuffer();
		}

		//Read package from player
		playerBuffer.WriteBytes(buffer);

		//Check if recieved package is empty, if so dont continue executing
		if(playerBuffer.Count() == 0) {
			playerBuffer.Clear();
			return;
		}

		//Check if package contains info
		if(playerBuffer.Length() >= 4) {
			//if so read full package length
			pLength = playerBuffer.ReadInteger(false);

			if(pLength <= 0) {
				//If there is no package or package is invalid close this method
				playerBuffer.Clear();
				return;
			}
		}

		while(pLength > 0 & pLength <= playerBuffer.Length() - 4) {
			if(pLength <= playerBuffer.Length() - 4) {
				playerBuffer.ReadInteger();
				data = playerBuffer.ReadBytes(pLength);
				HandleDataPackages(data);
			}

			pLength = 0;

			if(playerBuffer.Length() >= 4) {
				pLength = playerBuffer.ReadInteger(false);

				if (pLength <= 0) {
					//If there is no package or package is invalid close this method
					playerBuffer.Clear();
					return;
				}
			}

			if(pLength <= 1) {
				playerBuffer.Clear();
			}
		}
	}

	private static void HandleDataPackages(byte[] data) {
		Packet_ packet;
		ByteBuffer buffer = new ByteBuffer();
		buffer.WriteBytes(data);
		int packageID = buffer.ReadInteger();

		if(packetListener.TryGetValue(packageID, out packet)) {
			packet.Invoke(data);
		}
	}

	private static void HandleWelcomeMsg(byte[] data){
		ByteBuffer buffer = new ByteBuffer ();
		buffer.WriteBytes (data);
		int packageID = buffer.ReadInteger ();
		string msg = buffer.ReadString ();

		Debug.Log (msg);
        LoadManager.Instance.UpdateProgress("VersionCheck");
        ClientTCP.PACKET_CheckVersion(ClientManager.Instance.GetClientVersion());
	}

    private static void HandleAccountExist(byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packageID = buffer.ReadInteger();
        string result = buffer.ReadString();

        Debug.Log("Recieved account notification from server");

        if(result == "true") {
            Debug.Log("Account exist loading menu");
            LoadManager.Instance.UpdateProgress("AccountVerified");
            GameManager.Instance.LoadScene("Menu");
        }else if(result == "false") {
            LoadManager.Instance.UpdateProgress("AccountCreation");
            Debug.Log("Account did not exist sending request to create account");
            ClientTCP.PACKET_CreateAccount();
        }
    }

    private static void HandleLobbyCreated(byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);

        int packageID = buffer.ReadInteger();
        Debug.Log("Lobby has been created");
        LobbyUIManager.Instance.ExitCreateLobbyPopup();
    }

    private static void HandleSendLobbyList(byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);

        int packageID = buffer.ReadInteger();
        string lobbyList = buffer.ReadString();

        LobbyListController.Instance.SetJsonPacket(lobbyList);
    }

    private static void HandleSendUsersOnlineList(byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);

        int packageID = buffer.ReadInteger();
        string userList = buffer.ReadString();

        LobbyUsersOnlineListController.Instance.SetJsonPacket(userList);
    }

    private static void HandlePlayerJoined(byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);

        int packageID = buffer.ReadInteger();
        int matchID = buffer.ReadInteger();
        string game = buffer.ReadString();
        string player = buffer.ReadString();

        GameManager.Instance.StartMatch(matchID, game,player);
    }

    private static void HandleMatchOver(byte[] data){
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);

        int packageID = buffer.ReadInteger();
        string jsonPacket = buffer.ReadString();

        MatchInfo matchInfo = JsonConvert.DeserializeObject<MatchInfo>(jsonPacket);
        Debug.Log(jsonPacket);
        GameManager.Instance.SetMatchInfo(matchInfo);
    }

    private static void HandleGetBalance(byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);

        int packageID = buffer.ReadInteger();
        int balance = buffer.ReadInteger();

        LobbyUIManager.Instance.SetMoneyText(balance);
    }

    private static void HandleUpdateAvailable(byte[] data) {
        LoadPopupController.Instance.ShowPopup("Update Available","Your client is out of date please update to continue","Update");
    }

    private static void HandleNoUpdateAvailable(byte[] data) {
        ByteBuffer buffer = new ByteBuffer();

        buffer.WriteBytes(data);

        int packageID = buffer.ReadInteger();

        LoadManager.Instance.UpdateProgress("AccountVerification");
        ClientTCP.PACKET_ThankYou();
        ClientTCP.PACKET_CheckForAccount();
    }
}

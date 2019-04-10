using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
        packetListener.Add((int)ServerPackages.SPlayerJoined, HandlePlayerJoined);
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

		ClientTCP.PACKET_ThankYou ();
        ClientTCP.PACKET_CheckForAccount();
	}

    private static void HandleAccountExist(byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packageID = buffer.ReadInteger();
        string result = buffer.ReadString();

        Debug.Log("Recieved account notification from server");

        if(result == "true") {
            Debug.Log("Account exist loading menu");
            GameManager.Instance.LoadScene("Menu");
        }else if(result == "false") {
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

    private static void HandlePlayerJoined(byte[] data) {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);

        int packageID = buffer.ReadInteger();
        int matchID = buffer.ReadInteger();
        string game = buffer.ReadString();
        string player = buffer.ReadString();

        GameManager.Instance.StartMatch(matchID, game,player);
    }
}

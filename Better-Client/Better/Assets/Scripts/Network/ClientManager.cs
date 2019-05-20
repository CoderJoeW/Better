using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : Singleton<ClientManager> {
	[SerializeField]
	private string ipAddress;

	[SerializeField]
	private int port;

    [SerializeField]
    private string clientVersion;

	private void Awake(){
		DontDestroyOnLoad (this);
		UnityThread.initUnityThread ();

		ClientHandleData.InitializePacketListener ();
		ClientTCP.InitializeClientSocket (ipAddress, port);
	}

    public string GetClientVersion() {
        return clientVersion;
    }

    private void OnApplicationQuit() {
        ClientTCP.PACKET_DisconnectFromServer();
    }

    private void OnApplicationPause() {
        ClientTCP.PACKET_DisconnectFromServer();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour {
	[SerializeField]
	private string ipAddress;

	[SerializeField]
	private int port;

	private void Awake(){
		DontDestroyOnLoad (this);
		UnityThread.initUnityThread ();

		ClientHandleData.InitializePacketListener ();
		ClientTCP.InitializeClientSocket (ipAddress, port);
	}
}

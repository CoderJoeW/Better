using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour{

    private void Start() {
        ClientTCP.PACKET_GetBalance();
    }

    public void LeaveLobby() {
        ClientTCP.PACKET_LeaveLobby();
        LobbyUIManager.Instance.ExitLobbyWaitingPopup();
    }

    public void OpenStore() {
        GameManager.Instance.LoadScene("BasicStore");
    }
}

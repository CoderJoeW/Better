using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyController : MonoBehaviour {
    [SerializeField]
    private InputField betAmount;

    private string gameMode;

    public void SetGameMode(string game) {
        gameMode = game;
    }

    public void CreateLobby() {
        int betValue;
        int.TryParse(betAmount.text, out betValue);
        ClientTCP.PACKET_CreateLobby(betValue, gameMode);
    }
}

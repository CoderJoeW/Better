using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : Singleton<LobbyUIManager>{
    [SerializeField]
    private GameObject createLobbyPopup;

    [SerializeField]
    private GameObject lobbyWaitingPopup;

    [SerializeField]
    private Text moneyText;

    public void ShowCreateLobbyPopup() {
        createLobbyPopup.SetActive(true);
    }

    public void ExitCreateLobbyPopup() {
        createLobbyPopup.SetActive(false);
    }

    public void ShowLobbyWaitingPopup() {
        lobbyWaitingPopup.SetActive(true);
    }

    public void ExitLobbyWaitingPopup() {
        lobbyWaitingPopup.SetActive(false);
    }

    public void SetMoneyText(int amount) {
        moneyText.text = amount.ToString();
    }

    public string GetMoneyText() {
        return moneyText.text;
    }
}

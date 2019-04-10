using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : Singleton<LobbyUIManager>{
    [SerializeField]
    private GameObject createLobbyPopup;

    public void ShowCreateLobbyPopup() {
        createLobbyPopup.SetActive(true);
    }

    public void ExitCreateLobbyPopup() {
        createLobbyPopup.SetActive(false);
    }
}

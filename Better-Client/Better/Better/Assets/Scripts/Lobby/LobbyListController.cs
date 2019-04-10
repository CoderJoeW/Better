using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

[Serializable]
public class Lobby {
    public int ID { set; get; }
    public string UID { set; get; }
    public int Bet { set; get; }
    public string Game { set; get; }
}

public class LobbyListController : Singleton<LobbyListController>{
    [SerializeField]
    private GameObject contentContainer;

    [SerializeField]
    private GameObject lobbyListItemPrefab;

    private Text idText;
    private Text nameText;
    private Text betText;
    private Text gameText;
    private Button joinLobbyButton;

    [SerializeField]
    private float timer = 0;
    private float timerReset = 5;

    private void Update() {
        if(timer > 0) {
            timer -= 1 * Time.deltaTime;
        }

        if(timer <= 0) {
            ClientTCP.PACKET_RefreshLobbyList();
            timer = timerReset;
        }
    }

    public void SetJsonPacket(string packet) {
        ClearChildElements();
        List<Lobby> items = JsonConvert.DeserializeObject<List<Lobby>>(packet);
        Debug.Log(items);

        for(int i = 0; i < items.Count; i++) {
            Lobby thisItem = items[i];

            GameObject obj = Instantiate(lobbyListItemPrefab,Vector3.zero,Quaternion.identity);
            idText = obj.transform.GetChild(1).GetComponent<Text>();
            nameText = obj.transform.GetChild(3).GetComponent<Text>();
            betText = obj.transform.GetChild(5).GetComponent<Text>();
            gameText = obj.transform.GetChild(7).GetComponent<Text>();
            joinLobbyButton = obj.transform.GetChild(8).GetComponent<Button>();

            idText.text = thisItem.ID.ToString();
            nameText.text = thisItem.UID.ToString();
            betText.text = thisItem.Bet.ToString();
            gameText.text = thisItem.Game.ToString();

            joinLobbyButton.onClick.AddListener(delegate {
                ClientTCP.PACKET_JoinLobby(thisItem.ID);
            });

            obj.transform.SetParent(contentContainer.transform);
        }
    }

    private void ClearChildElements() {
        foreach(Transform child in contentContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }
}

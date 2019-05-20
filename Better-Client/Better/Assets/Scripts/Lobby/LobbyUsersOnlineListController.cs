using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

[Serializable]
public class User {
    public string Username { set; get; }
}

public class LobbyUsersOnlineListController : Singleton<LobbyUsersOnlineListController>{
    [SerializeField]
    private GameObject contentContainer;

    [SerializeField]
    private GameObject userOnlinePanelPrefab;

    private Text usernameText;

    [SerializeField]
    private float timer = 0;
    private float timerReset = 2.5f;

    private void Update() {
        if (timer > 0) {
            timer -= 1 * Time.deltaTime;
        }

        if (timer <= 0) {
            ClientTCP.PACKET_RefresUsersOnlineList();
            timer = timerReset;
        }
    }

    public void SetJsonPacket(string packet) {
        ClearChildElements();
        List<User> users = JsonConvert.DeserializeObject<List<User>>(packet);

        for (int i = 0; i < users.Count; i++) {
            User thisUser = users[i];

            GameObject obj = Instantiate(userOnlinePanelPrefab, Vector3.zero, Quaternion.identity);
            usernameText = obj.transform.GetChild(1).GetComponent<Text>();

            usernameText.text = thisUser.Username.ToString();

            obj.transform.SetParent(contentContainer.transform);
        }
    }

    private void ClearChildElements() {
        foreach (Transform child in contentContainer.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

}

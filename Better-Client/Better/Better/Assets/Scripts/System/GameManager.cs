using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>{
    public int matchID;
    public string game;
    public string player;

    private void Awake() {
        DontDestroyOnLoad(this);
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void StartMatch(int match_ID,string gameName,string playerN) {
        matchID = match_ID;
        game = gameName;
        player = playerN;
        LoadScene(game);
    }
}

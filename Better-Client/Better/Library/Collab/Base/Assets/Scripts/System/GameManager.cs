using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public void LoadURL(string url) {
        Application.OpenURL(url);
    }

    public void StartMatch(int match_ID,string gameName,string playerN) {
        matchID = match_ID;
        game = gameName;
        player = playerN;
        LoadScene(game);
    }

    public void GameOver() {
        GameObject gameOverScreen = GameObject.Find("MatchOverScreenParent").transform.GetChild(0).gameObject;
        gameOverScreen.SetActive(false);
    }

    public void SetMatchInfo(MatchInfo matchInfo) {
        GameObject gameOverScreen = GameObject.Find("MatchOverScreenParent").transform.GetChild(0).gameObject;
        //Set waiting for opponent screen to inactive
        gameOverScreen.transform.GetChild(1).gameObject.SetActive(false);

        //Set match info screen to active
        gameOverScreen.transform.GetChild(2).gameObject.SetActive(true);

        Text player1Score = gameOverScreen.transform.GetChild(2).GetChild(1).GetComponent<Text>();
        Text player2Score = gameOverScreen.transform.GetChild(2).GetChild(3).GetComponent<Text>();
        Text winnerText = gameOverScreen.transform.GetChild(2).GetChild(4).GetComponent<Text>();
        Text amountText = gameOverScreen.transform.GetChild(2).GetChild(5).GetComponent<Text>();
        Button backToLobby = gameOverScreen.transform.GetChild(2).GetChild(6).GetComponent<Button>();

        bool isPlayer1 = false;

        if (GameManager.Instance.player == "Player1") {
            isPlayer1 = true;
        } else {
            isPlayer1 = false;
        }

        player1Score.text = matchInfo.Player1Score.ToString();
        player2Score.text = matchInfo.Player2Score.ToString();

        string winner = matchInfo.Winner;

        if (winner == "Player1" && isPlayer1) {
            winnerText.text = "You Won!";
            amountText.text = (matchInfo.Bet * 2).ToString();
            amountText.color = Color.green;
        } else if (winner == "Player2" && isPlayer1) {
            winnerText.text = "You Lost!";
            amountText.text = matchInfo.Bet.ToString();
            amountText.color = Color.red;
        } else if (winner == "Player2" && !isPlayer1) {
            winnerText.text = "You Won!";
            amountText.text = (matchInfo.Bet * 2).ToString();
            amountText.color = Color.green;
        } else if (winner == "Player1" && !isPlayer1) {
            winnerText.text = "You Lost!";
            amountText.text = matchInfo.Bet.ToString();
            amountText.color = Color.red;
        }

        backToLobby.onClick.AddListener(delegate {
            Time.timeScale = 1;
            GameManager.Instance.LoadScene("Menu");
        });

    }
}

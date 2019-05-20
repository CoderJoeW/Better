using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FBScoreManager : Singleton<FBScoreManager>{
    private int currentScore = 0;

    [SerializeField]
    private Text scoreText;

    public void IncrementScore() {
        currentScore++;
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public int GetScore() {
        return currentScore;
    }
}

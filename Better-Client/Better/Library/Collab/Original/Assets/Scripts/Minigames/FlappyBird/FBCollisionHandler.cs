using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBCollisionHandler : MonoBehaviour{
    private void OnTriggerEnter2D(Collider2D collision) {
        GameManager.Instance.GameOver(FBScoreManager.Instance.GetScore());
    }
}

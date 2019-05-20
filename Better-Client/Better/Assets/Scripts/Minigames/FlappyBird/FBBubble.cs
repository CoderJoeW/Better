using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBBubble : MonoBehaviour{
    [SerializeField]
    private float moveSpeed = 1f;

    private void Start() {
        Reset();
    }

    private void Update() {
        transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        if(transform.position.y > 10) {
            Reset();
        }
    }

    private void Reset() {
        float randomHeight = Random.Range(-8f, -18f);
        transform.position = new Vector3(transform.position.x, randomHeight, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (OtherIsTheFish(other)) {
            FBScoreManager scoreManager = GameObject.FindObjectOfType<FBScoreManager>();
            scoreManager.IncrementScore();
            Reset();
        }
    }

    private bool OtherIsTheFish(Collider2D other) {
        return (other.GetComponent<FBPlayerController>() != null);
    }
}

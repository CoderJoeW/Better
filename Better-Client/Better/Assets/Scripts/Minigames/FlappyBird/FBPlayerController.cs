using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBPlayerController : MonoBehaviour{
    [SerializeField]
    private float _upwardForceMultiplier = 100f;

    private void Update() {
        bool pressedFireButton = Input.GetButtonDown("Fire1");

        if (pressedFireButton) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * _upwardForceMultiplier);
        }

        if(transform.position.y > 6f || transform.position.y < -6f) {
            GameManager.Instance.GameOver(FBScoreManager.Instance.GetScore());
        }
    }
}

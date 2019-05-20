using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgerPlayerController : Singleton<DodgerPlayerController>{
    //Player speed
    [SerializeField]
    private float speed = 10.0f;

    //Score display text
    [SerializeField]
    private Text scoreText;

    //Game Over screen
    [SerializeField]
    private GameObject gameOverScreen;

    //Player bounds
    [SerializeField]
    private float leftBound = -5f;
    [SerializeField]
    private float rightBound = 5f;
    [SerializeField]
    private float upBound = 3.5f;
    [SerializeField]
    private float downBound = -3.5f;

    private int score = 0;

    private void Update() {
        //Update score display
        scoreText.text = score.ToString();

        float pointer_x = Input.GetAxis("Mouse X");
        float pointer_y = Input.GetAxis("Mouse Y");
        if (Input.touchCount > 0) {
            pointer_x = Input.touches[0].deltaPosition.x;
            pointer_y = Input.touches[0].deltaPosition.y;

            float movementSpeedX = Time.deltaTime * pointer_x * speed;
            float movementSpeedY = Time.deltaTime * pointer_y * speed;

            transform.Translate(movementSpeedX, movementSpeedY, 0);
        } else {
            //Horizontal Speed
            float movementSpeedX = Time.deltaTime * Input.GetAxis("Horizontal") * speed;
            //Vertical Speed
            float movementSpeedY = Time.deltaTime * Input.GetAxis("Vertical") * speed;

            //Player movement
            transform.Translate(movementSpeedX, movementSpeedY, 0);

            //Create bounds around player
            if (transform.position.x > rightBound) {
                transform.position = new Vector3(rightBound, transform.position.y, 0);
            } else if (transform.position.x < leftBound) {
                transform.position = new Vector3(leftBound, transform.position.y, 0);
            }

            if (transform.position.y > upBound) {
                transform.position = new Vector3(transform.position.x, upBound, 0);
            } else if (transform.position.y < downBound) {
                transform.position = new Vector3(transform.position.x, downBound, 0);
            }
        }
    }

    public void GameOver() {
        GameManager.Instance.GameOver(score);
    }

    public void UpdateScore(int amount) {
        score += amount;
    }

    public int GetScore() {
        return score;
    }
}

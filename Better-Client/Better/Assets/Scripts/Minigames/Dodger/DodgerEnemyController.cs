using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgerEnemyController : MonoBehaviour{
    //Speed of the enemy
    [SerializeField]
    private float speed = 8f;

    private void Update() {
        //Movement of the enemy
        transform.Translate(0, -speed * Time.deltaTime, 0);

        if(transform.position.y < -5F) {
            DodgerPlayerController.Instance.UpdateScore(1);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            DodgerEnemySpawnController[] objs = GameObject.FindObjectsOfType<DodgerEnemySpawnController>();
            foreach(DodgerEnemySpawnController obj in objs) {
                obj.TurnOff();
            }
            //GameOver
            DodgerPlayerController.Instance.GameOver();
        }
    }
}

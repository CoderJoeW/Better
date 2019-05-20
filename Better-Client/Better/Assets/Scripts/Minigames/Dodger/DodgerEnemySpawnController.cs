using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgerEnemySpawnController : MonoBehaviour{
    //Rate enemies spawn
    [SerializeField]
    private float spawnRate = 1;

    //Enemy Prefab
    [SerializeField]
    private GameObject enemy;

    //Spawner Bounds
    [SerializeField]
    private float leftBound = -5f;
    [SerializeField]
    private float rightBound = 5F;

    private bool isOn = true;

    private void Start() {
        InvokeRepeating("SpawnEnemy", 1, spawnRate);
    }

    private void SpawnEnemy() {
        if (isOn) {
            //Create clone of prefab
            GameObject enemyClone;

            //Spawns enemyClone at this location and rotation
            enemyClone = Instantiate(enemy, this.transform.position, this.transform.rotation) as GameObject;

            //Randomly moves spawner along x axis
            float x = Random.Range(leftBound, rightBound);
            transform.position = new Vector3(x, this.transform.position.y, 0);
        }
    }

    public void TurnOff() {
        isOn = false;
    }
}

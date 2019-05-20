using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour{
    //Food prefab
    [SerializeField]
    private GameObject foodPrefab;

    //Borders
    [SerializeField]
    private Transform topBorder;
    [SerializeField]
    private Transform bottomBorder;
    [SerializeField]
    private Transform leftBorder;
    [SerializeField]
    private Transform rightBorder;

    private void Start() {
        //Spawn food every 4 seconds starting at 3
        InvokeRepeating("Spawn", 1, 2);
    }

    //Spawn one piece of food
    private void Spawn() {
        //X position between left and right border
        int x = (int)Random.Range(leftBorder.position.x, rightBorder.position.x);

        //Y position between top and bottom border
        int y = (int)Random.Range(bottomBorder.position.y, topBorder.position.y);

        //Spawn the food at (x,y)
        Instantiate(foodPrefab, new Vector2(x, y), Quaternion.identity);
    }
}

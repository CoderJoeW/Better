using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Snake : MonoBehaviour{
    //Current movement direction
    //Default moves to the right
    private Vector2 dir = Vector2.right;

    //Keep track of tail
    private List<Transform> tail = new List<Transform>();

    //Did the snake eat something
    private bool ate = false;

    //Tail prefab
    [SerializeField]
    private GameObject tailPrefab;

    private int snakeLength = 0;

    private void Start() {
        //Move the snake every 300ms
        InvokeRepeating("Move", 0.05f, 0.05f);
    }

    private void Update() {
        //Move in a new direction
        if (Input.GetKey(KeyCode.RightArrow)) {
            dir = Vector2.right;
        }else if (Input.GetKey(KeyCode.DownArrow)) {
            dir = -Vector2.up; //-up means down
        }else if (Input.GetKey(KeyCode.LeftArrow)) {
            dir = -Vector2.right; //-right means left
        }else if (Input.GetKey(KeyCode.UpArrow)) {
            dir = Vector2.up;
        }
    }

    private void Move() {
        //Save current position
        Vector2 v = transform.position;

        //Move head into new direction
        transform.Translate(dir);

        //Ate something? then insert new element into gap
        if (ate) {
            //Load prefab into world
            GameObject g = (GameObject)Instantiate(tailPrefab, v, Quaternion.identity);

            //Keep track of it in tail list
            tail.Insert(0, g.transform);

            //Reset flag
            ate = false;
        }

        //Do we have a tail?
        if(tail.Count > 0) {
            //Move last tail element to where the head was
            tail.Last().position = v;

            //Add to front of list, remove from the back
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //Food?
        if (collision.name.StartsWith("FoodPrefab")) {
            //Get longer in next Move call
            ate = true;

            //Remove the food
            Destroy(collision.gameObject);
        } else {
            //Collided with tail or border
            ClientTCP.PACKET_GameOver(snakeLength);
            Debug.Log("GameOver functionality has not yet been implemented");
        }
    }
}

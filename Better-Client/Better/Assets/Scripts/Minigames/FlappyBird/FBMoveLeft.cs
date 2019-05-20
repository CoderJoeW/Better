using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBMoveLeft : MonoBehaviour{
    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private bool _randomizeHeight = true;

    private void Update() {
        transform.Translate(Vector3.left * Time.deltaTime * _speed);

        if(transform.position.x < -15) {
            if (_randomizeHeight) {
                float randomYPosition = UnityEngine.Random.Range(-3, 3);
                transform.position = new Vector3(15, randomYPosition, 0);
            } else {
                float randomXPosition = UnityEngine.Random.Range(7.5f, 15);
                transform.position = new Vector3(randomXPosition, transform.position.y, 0);
            }
        }
    }
}

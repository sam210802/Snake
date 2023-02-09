using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    public Transform snake;
    public int gameWidth = 10;
    public int gameHeight = 10;

    // Start is called before the first frame update
    void Start()
    {
        RandomisePos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RandomisePos() {
        bool uniquePos = false;
        Vector3 newPos = new Vector3();

        while (uniquePos == false) {
            uniquePos = true;
            float x = Random.Range(((gameWidth/2)-1)*-1, (gameWidth/2)-1);
            float y = Random.Range(((gameHeight/2)-1)*-1, (gameHeight/2)-1);

             newPos = new Vector3(x, y, 0.0f);

            foreach (Transform child in snake) {
                if (newPos == child.position) {
                    uniquePos = false;
                }
            }
        }

        this.transform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // if player collides with food change location
        if (other.tag == "Player") {
            RandomisePos();
        }
    }
}

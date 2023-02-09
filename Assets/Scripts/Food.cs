using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    private GameObject snake = null;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomisePos());
        gameObject.GetComponent<TimeBody>().Record();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // sets the transforms position this scipts attatched to
    // to a random position within the game area
    public IEnumerator RandomisePos() {
        bool uniquePos = false;
        Vector3 newPos = new Vector3();

        while (uniquePos == false) {
            uniquePos = true;
            float x = Random.Range(((GameManager.instance.getGameWidth()/2)-1)*-1, (GameManager.instance.getGameWidth()/2)-1);
            float y = Random.Range(((GameManager.instance.getGameHeight()/2)-1)*-1, (GameManager.instance.getGameHeight()/2)-1);

            newPos = new Vector3(x, y, 0.0f);

            if (snake == null) {
                try {
                    snake = GameObject.FindGameObjectsWithTag("Snake")[0];
                } catch (System.IndexOutOfRangeException e) {
                    Debug.Log(e);
                    break;
                }
            }

            foreach (Transform child in snake.transform) {
                if (newPos == child.position) {
                    uniquePos = false;
                }
            }
        }

        this.transform.position = newPos;
        yield return null;
    }

    // called when game object this scripts attatched to collides with another object
    private void OnTriggerEnter2D(Collider2D other) {
        // if player collides with food change location
        if (other.tag == "SnakeHead") {
            StartCoroutine(RandomisePos());
        }
    }
}

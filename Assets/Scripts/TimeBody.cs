using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class to handle rewinding of objects
public class TimeBody : MonoBehaviour
{

    // keeps track of previous positions
    private List<Vector3> positions;
    // only used for snake head
    private List<Vector2> directions;
    private bool snakeHead = false;

    void Awake() {
        // initializing position list
        positions = new List<Vector3>();

        if (gameObject.tag == "SnakeHead") {
            snakeHead = true;
            directions = new List<Vector2>();
        }
    }

    void FixedUpdate() {
        if (GameManager.instance.isRewinding()) {
            Rewind();
        } else {
            Record();
        }
    }

    // rewinds object
    public void Rewind() {
        if (positions.Count <= 0) {
            if (gameObject.tag == "Player") {
                Transform parent = gameObject.transform.parent;
                foreach (Transform child in parent) {
                    if (child.tag == "SnakeHead") {
                        child.GetComponent<Snake>().removeLast();
                    }
                }
            } else {
                Destroy(gameObject);
            }
            return;
        }
        transform.position = positions[0];
        positions.RemoveAt(0);
        if (snakeHead) {
            Snake.instance.setDirection(directions[0]);
            directions.RemoveAt(0);
        }
    }

    // records object
    public void Record() {
        positions.Insert(0, transform.position);

        if (snakeHead) {
            directions.Insert(0, Snake.instance.getDirection());
        }
    }

    // emptys positions list
    public void resetPositions() {
        positions.Clear();
        if (snakeHead) {
            directions.Clear();
        }

        Record();
    }
}

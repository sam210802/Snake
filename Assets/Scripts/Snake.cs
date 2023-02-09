using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private bool directionKeyUpdated = false;
    private Vector2 direction = Vector2.up;
    private List<Transform> segments;

    [SerializeField]
    private Transform snakeBodyPrefab;

    // creates new snake object and makes 
    // game object this script is attatched to child of it
    void Start() {
        // creates empty object to store all snake transforms
        GameObject parent = new GameObject();
        parent.name = "Snake";
        parent.tag = "Snake";

        // makes snake head a child of newly created empty object "Snake"
        transform.parent = parent.transform;

        // adds snake head to segments list
        segments = new List<Transform>();
        segments.Add(transform);
    }

    // Update is called once per frame
    void Update() {
        // stops two direction requests being logged before snake is moved
        // resulting in user being able to go back on self
        if (directionKeyUpdated) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down) {
            direction = Vector2.up;
            directionKeyUpdated = true;
        } else if (Input.GetKeyDown(KeyCode.D)  && direction != Vector2.left) {
            direction = Vector2.right;
            directionKeyUpdated = true;
        }  else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up) {
            direction = Vector2.down;
            directionKeyUpdated = true;
        }  else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right) {
            direction = Vector2.left;
            directionKeyUpdated = true;
        }
    }

    // fixed time interval
    // used for movement
    private void FixedUpdate() {
        // Unity docs advise doing this when using Time.timeScale to pause game
        Time.fixedDeltaTime = GameManager.instance.getDefaultFixedDeltaTime() * Time.timeScale;

        // stops snake from moving before game has started
        // stops snake from moving while game is rewinding
        if (GameManager.instance.isGameStarted() && GameManager.instance.isRewinding() == false) {
            // sets position of each snake segment to the segment ahead of it
            for (int i = segments.Count - 1; i > 0; i--) {
                segments[i].position = segments[i - 1].position;
            }

            // sets position of snake head
            transform.position = new Vector3(
                Mathf.Round(transform.position.x + direction.x),
                Mathf.Round(transform.position.y + direction.y),
                0.0f
            );

            directionKeyUpdated = false;
        }
    }

    // called when game object this scripts attatched to collides with another object
    private void OnTriggerEnter2D(Collider2D other) {
        // if player collides with wall end game
        if (other.tag == "Wall" || other.tag == "Player") {
            Reset();
        }

        // if player collides with food add segment
        if (other.tag == "Food") {
            Grow();
        }
    }

    // resets snake to starting position, length and direction
    // and pauses game
    private void Reset() {
        GameManager.instance.pauseGame();
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        for (int i = segments.Count - 1; i > 0; i--) {
            Destroy(segments[i].gameObject);
            segments.RemoveAt(i);
        }
        direction = Vector2.up;
    }

    // adds a SnakeBody segment to the snake increasing it's length by one
    private void Grow() {
        Transform newSegment = Instantiate(snakeBodyPrefab, transform.parent);
        newSegment.position = segments[segments.Count - 1].position;
        segments.Add(newSegment);
    }
}

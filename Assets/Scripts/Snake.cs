using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private bool directionKeyUpdated = false;
    private Vector2 direction = Vector2.right;
    private List<Transform> segments;
    public Transform segmentPrefab;
    public Transform parent;

    // Start is called before the first frame update
    void Start() {
        segments = new List<Transform>();
        segments.Add(this.transform);
    }

    // Update is called once per frame
    void Update() {
        /* stops two direction requests being logged before snake is moved
           resulting in user being able to go back on self */
        if (directionKeyUpdated) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down) {
            direction = Vector2.up;
            directionKeyUpdated
     = true;
        } else if (Input.GetKeyDown(KeyCode.D)  && direction != Vector2.left) {
            direction = Vector2.right;
            directionKeyUpdated
     = true;
        }  else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up) {
            direction = Vector2.down;
            directionKeyUpdated
     = true;
        }  else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right) {
            direction = Vector2.left;
            directionKeyUpdated
     = true;
        }
    }
    // fixed time interval
    private void FixedUpdate() {

        // sets position of each snake segment to the segment ahead of it
        for (int i = segments.Count - 1; i > 0; i--) {
            segments[i].position = segments[i - 1].position;
        }

        // sets position of snake head
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x + direction.x),
            Mathf.Round(this.transform.position.y + direction.y),
            0.0f
        );

        directionKeyUpdated
 = false;
    }

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

    private void Reset() {
        this.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        for (int i = segments.Count - 1; i > 0; i--) {
            Destroy(segments[i].gameObject);
            segments.RemoveAt(i);
        }
    }

    private void Grow() {
        Transform newSegment = Instantiate(this.segmentPrefab, parent);
        newSegment.position = segments[segments.Count - 1].position;
        segments.Add(newSegment);
    }
}

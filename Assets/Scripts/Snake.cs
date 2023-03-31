using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public static Snake instance;
    // means direction can only be updated once per fixed update
    private bool directionKeyUpdated = false;
    // direction snake is traveling
    private Vector2 direction = Vector2.up;
    // list of transforms for each snake segment
    private List<Transform> segments;
    public List<Transform> segmentsProperty {
        get {
            return segments;
        }
    }

    // snake body prefab
    [SerializeField]
    private Transform snakeBodyPrefab;

    // position of tail on previous frame
    Vector3 tailPreviousPos;

    void Awake() {
        instance = this;
        segments = new List<Transform>();
    }

    void Start() {
        // creates empty object to store all snake transforms
        GameObject parent = new GameObject();
        parent.name = "Snake";
        parent.tag = "Snake";

        // makes snake head a child of newly created empty object "Snake"
        transform.parent = parent.transform;

        // adds snake head to segments list
        segments.Add(transform);

        // records position of snake so player can rewind to it
        gameObject.GetComponent<TimeBody>().Record();

        GameUI.instance.updateScore();
    }

    // Update is called once per frame
    void Update() {
        // stops two direction requests being logged before snake is moved
        // resulting in user being able to go back on self
        if (directionKeyUpdated) {
            return;
        }

        // gets keyboard input from user and changes direction of snake if move is legal
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
        Time.fixedDeltaTime = GameManager.instance.getFixedDeltaTime() * Time.timeScale;

        // if game is rewinding decrement update and return
        if (GameManager.instance.isRewinding()) {
            GameManager.instance.decrementNumUpdates();
            return;
        } 

        // if game is paused return
        if (GameManager.instance.isGamePaused()) return;

        // update before new tail pos
        tailPreviousPos = segments[segments.Count - 1].position;

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

        // set to false so direction can be changed again
        directionKeyUpdated = false;

        GameManager.instance.incrementNumUpdates();

        // can't use OnTriggerEnter2D() as it isn't being called after every fixed update
        // so if food spawns directly infront of snake then snake passes through it
        if (segments[0].position == Food.instance.transform.position) {
            Grow();
            Food.instance.RandomisePos();
            GameUI.instance.updateScore();
            // increments apples eaten by one
            PlayerPrefs.SetInt("Apples_Eaten", PlayerPrefs.GetInt("Apples_Eaten", 0) + 1);
            // plays eat sound effect
            AudioManager.PlaySound(AudioManager.nomSound);
        }
    }

    // removes last Transform in list and destroys it
    public void removeLast() {
        Destroy(segments[segments.Count-1].gameObject); // Destroy
        segments.RemoveAt(segments.Count-1); // Remove
    }

    // called when game object this scripts attatched to collides with another object
    private void OnTriggerEnter2D(Collider2D other) {
        // if player collides with wall end game
        if (other.tag == "Wall" || other.tag == "Player") {
            // increments total deaths by one
            PlayerPrefs.SetInt("Total_Deaths", PlayerPrefs.GetInt("Total_Deaths", 0) + 1);
            AudioManager.PlaySound(AudioManager.deathSound);
            Reset();
        }
    }

    // resets snake to starting position, length and direction
    // and pauses game
    private void Reset() {
        GameManager.instance.pauseGame();
        GameManager.instance.resetNumUpdates();

        GameUI.instance.setHighScore();

        // reset snake
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        for (int i = segments.Count - 1; i > 0; i--) {
            Destroy(segments[i].gameObject);
            segments.RemoveAt(i);
        }
        direction = Vector2.up;

        GameUI.instance.updateScore();
    }

    // adds a SnakeBody segment to the snake increasing it's length by one
    private void Grow() {
        Transform newSegment = Instantiate(snakeBodyPrefab, transform.parent);
        newSegment.position = tailPreviousPos;
        segments.Add(newSegment);
    }

    // get direction of snake
    public Vector2 getDirection()
    {
        return this.direction;
    }

    // set direction of snake
    public void setDirection(Vector2 direction)
    {
        this.direction = direction;
    }
}

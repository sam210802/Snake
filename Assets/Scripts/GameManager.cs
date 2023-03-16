using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform wallPrefab;
    public GameObject foodPrefab;
    public GameObject snakeHeadPrefab;

    [SerializeField]
    private int gameHeight = 10;
    [SerializeField]
    private int gameWidth = 10;

    private float defaultFixedDeltaTime = 0.08f;

    private GameObject gameArea;
    private GameObject foodObject;
    private Transform snakeObject;

    private bool rewinding = false;
    private int numUpdates = 0;
    private bool gamePaused = false;

    // Awake is called before first frame update and before start
    void Awake() {
        instance = this;
        // pauses game immediatly
        pauseGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        // pauses and unpases the game
        if (Input.GetKeyUp(KeyCode.Space)) {
            if (gamePaused) {
                resumeGame();
            } else {
                pauseGame();
            }
        }

        // rewinding game
        if (Input.GetKeyDown(KeyCode.Return) && numUpdates > 0) {
            startRewinding();
        } else if (Input.GetKeyUp(KeyCode.Return)) {
            stopRewinding();
        }

        if (numUpdates < 0 && rewinding) {
            Debug.Log("Num updates < 0");
            resetNumUpdates();
            stopRewinding();
        }
    }

    public void pauseGame() {
        Time.timeScale = 0.0f;
        gamePaused = true;
    }

    public void resumeGame() {
        Time.timeScale = 1.0f;
        gamePaused = false;
    }

    void startRewinding() {
        rewinding = true;
        Debug.Log("Rewinding");
    }

    void stopRewinding() {
        rewinding = false;
        pauseGame();
        Debug.Log("Rewinding Stopped");
        Debug.Log(numUpdates);
    }

    // creates all game objects required to play game
    void init() {
        createBoard();
        createFood();
        createSnake();
    }

    void createBoard() {
        gameArea = new GameObject();
        gameArea.name = "GameArea";
        for (int i = 0; i < 4; i++) {
            Transform wall = Instantiate(wallPrefab, gameArea.transform);
            switch (i) {
                case 0:
                    wall.localScale = new Vector3(1, gameHeight+2, 1);
                    wall.position = new Vector3(-((gameWidth/2) + 1), 0, 0);
                    wall.name = "Wall - Left";
                    break;
                case 1:
                    wall.localScale = new Vector3(gameWidth+2, 1, 1);
                    wall.position = new Vector3(0, (gameHeight/2) + 1, 0);
                    wall.name = "Wall - Top";
                    break;
                case 2:
                    wall.localScale = new Vector3(1, gameHeight+2, 1);
                    wall.position = new Vector3((gameWidth/2) + 1, 0, 0);
                    wall.name = "Wall - Right";
                    break;
                case 3:
                    wall.localScale = new Vector3(gameWidth+2, 1, 1);
                    wall.position = new Vector3(0, -((gameHeight/2) + 1), 0);
                    wall.name = "Wall - Bottom";
                    break;
            }
        }
    }

    // creates food object
    // if one already exists it is destroyed and recreated
    private void createFood() {
        if (foodObject != null) {
            Destroy(foodObject);
        }
        foodObject = Instantiate(foodPrefab);
        // stops snake from colliding with apple on start
        foodObject.transform.position = new Vector3(1, 1, 0);
    }

    // creates snake object
    // if one already exists it is destroyed and recreated
    private void createSnake() {
        if (snakeObject != null) {
            Destroy(snakeObject);
        }
        // creates snake head game object
        GameObject snakeHead = Instantiate(snakeHeadPrefab);
        // sets snake heads parent as the snake object
        // snake head parent is created when snake head is initialized
        snakeObject = snakeHead.transform.parent;
    }

    // get height of game board
    public int getGameHeight()
    {
        return this.gameHeight;
    }

    // get width of game board
    public int getGameWidth()
    {
        return this.gameWidth;
    }

    // get default fixed delta time of game
    public float getDefaultFixedDeltaTime()
    {
        return this.defaultFixedDeltaTime;
    }

    public bool isRewinding()
    {
        return this.rewinding;
    }

    public void setRewinding(bool rewinding)
    {
        this.rewinding = rewinding;
    }

    // increments num updates by one
    public void incrementNumUpdates() {
        numUpdates += 1;
    }

    public void decrementNumUpdates() {
        numUpdates -= 1;
    }

    // resets num updates to 0
    // also cleares any TimeBody position lists
    // player can't rewind further than when this was last called
    public void resetNumUpdates() {
        numUpdates = 0;

        TimeBody[] scripts = (TimeBody[]) FindObjectsOfType(typeof(TimeBody));
        foreach (TimeBody script in scripts) {
            script.resetPositions();
        }
    }

    public bool isGamePaused()
    {
        return this.gamePaused;
    }
}

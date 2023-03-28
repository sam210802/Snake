using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    Board board;
    public Board boardPropery {
        get {
            return board;
        }
    }
    public GameObject foodPrefab;
    public GameObject snakeHeadPrefab;

    private float defaultFixedDeltaTime = 0.16f;

    private GameObject foodObject;
    private Transform snakeObject;

    private bool rewinding = false;
    private int numUpdates = 0;
    private bool gamePaused = false;

    public static string currentLevel;

    // Awake is called before first frame update and before start
    void Awake() {
        instance = this;
        StartCoroutine(OptionsMenu.setLocale(OptionsMenu.loadLocalePrefs()));
        // pauses game immediatly
        pauseGame();

        // try load a level if current level isn't null
        if (currentLevel != null) {
            board = LevelSaveLoadManager.load(currentLevel);
        }
        // create default board if current level not an actual level
        if (board == null) {
            board = new Board("level_01", 9, 9);
        }
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
    }

    void stopRewinding() {
        rewinding = false;
        pauseGame();
    }

    // creates all game objects required to play game
    void init() {
        createSnake();
        createFood();
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
        Food.instance.RandomisePos();
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
        return board.gameHeightProperty;
    }

    // get width of game board
    public int getGameWidth()
    {
        return board.gameWidthProperty;
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

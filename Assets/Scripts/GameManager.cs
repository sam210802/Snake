using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

    private float fixedDeltaTime;

    private GameObject foodObject;
    private Transform snakeObject;

    private bool rewinding = false;
    private int numUpdates = 0;
    private bool gamePaused = false;

    // true when all board tiles are occupied
    // aka when snake length equals available tiles
    public bool boardFilled = false;
    bool won = false;

    static string currentLevel;
    static bool defaultLevel;

    // Awake is called before first frame update and before start
    void Awake() {
        instance = this;
        fixedDeltaTime = Time.fixedDeltaTime;
        StartCoroutine(OptionsMenu.setLocale(OptionsMenu.loadLocalePrefs()));

        // pauses game immediatly
        pauseGame();

        // try load a level if current level isn't null
        if (currentLevel != null) {
            if (defaultLevel) {
                board = LevelSaveLoadManager.loadDefault(int.Parse(currentLevel));
            } else {
                board = LevelSaveLoadManager.load(currentLevel);
            }
        }
        // create default board if current level not an actual level
        if (board == null) {
            board = new Board("default_level", 9, 9);
        }
        board.createBoard();
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

        if (GameUI.instance.scoreProperty >= board.scoreToWinProperty && won == false) {
            Debug.Log("Congratulations you won");
            int currentMaxLevel = PlayerPrefs.GetInt("MaxLevelUnlocked", 1);
            int newMaxLevel = int.Parse(Regex.Match(board.levelNameProperty, "\\d+").ToString()) + 1;
            Debug.Log("New Max Level: " + newMaxLevel);
            PlayerPrefs.SetInt("MaxLevelUnlocked", Mathf.Max(currentMaxLevel, newMaxLevel));
            won = true;
        }

        if (boardFilled) {
            Debug.Log("Congratulations you fully ocmpleted this level");
            boardFilled = false;
            pauseGame();
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
    public float getFixedDeltaTime()
    {
        return this.fixedDeltaTime;
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

    public static void setCurrentLevel(string levelName, bool isDefaultLevel) {
        currentLevel = levelName + ".json";
        defaultLevel = isDefaultLevel;
    }

    public static void setCurrentLevel(int levelNumber) {
        currentLevel = levelNumber.ToString("00");
        defaultLevel = true;
    }
}

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

    private bool gameStarted = false;

    private bool rewinding = false;

    // Awake is called before first frame update and before start
    void Awake() {
        instance = this;
        // pauses game immediatly
        Time.timeScale = 0;
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
            if (gameStarted == false) {
                foodObject.GetComponent<Food>().RandomisePos();
                resumeGame();
                gameStarted = true;
            } else if (Time.timeScale == 1) {
                pauseGame();
            } else if (Time.timeScale == 0) {
                resumeGame();
            }
        }

        // rewing game
        if (Input.GetKeyDown(KeyCode.Return)) {
            startRewinding();
        } else if (Input.GetKeyUp(KeyCode.Return)) {
            stopRewinding();
        }
    }

    public void pauseGame() {
        Time.timeScale = 0;
    }

    public void resumeGame() {
        Time.timeScale = 1;
    }

    void startRewinding() {
        rewinding = true;
    }

    void stopRewinding() {
        rewinding = false;
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
        // stops snake from colliding with appl eon start
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

    // get if game has started yet
    public bool isGameStarted()
    {
        return this.gameStarted;
    }

    public bool isRewinding()
    {
        return this.rewinding;
    }

    public void setRewinding(bool rewinding)
    {
        this.rewinding = rewinding;
    }
}

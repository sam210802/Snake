using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCreatorManager : MonoBehaviour
{
    // odd only as game area has to be centered
    // default values on start
    private int gameWidth = 9;
    private int gameHeight = 9;
    private string levelName = "defaultLevel";

    int offset = 3;

    bool createdBoard = false;
    bool createdGrid = false;

    private GameObject gameArea;
    private GameObject gridArea;
    List<Transform> walls;
    public Transform wallPrefab;

    public ToolTip toolTipScript;
    public TransformTip transformTipScript;

    private string saveLocation;

    [SerializeField]
    Transform gridPrefab;

    [SerializeField]
    TMP_InputField widthInput;
    
    [SerializeField]
    TMP_InputField heightInput;

    [SerializeField]
    TMP_InputField levelNameInput;

    void Awake() {
        walls = new List<Transform>();
        saveLocation = Application.persistentDataPath + "/Data/Levels";
    }

    // Start is called before the first frame update
    void Start()
    {
        widthInput.text = gameWidth.ToString();
        heightInput.text = gameHeight.ToString();
        levelNameInput.text = levelName;
        createBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable() {
        createBoard();
    }

    void createBoard() {
        if (createdBoard) {
            updateBoard();
            return;
        }

        gameArea = new GameObject();
        gameArea.name = "GameArea";
        Transform wall;

        for (int i = 0; i < 4; i++) {
            wall = Instantiate(wallPrefab, gameArea.transform);
            switch (i) {
                case 0:
                    wall.localScale = new Vector3(1, gameHeight+offset, 1);
                    wall.position = new Vector3(-((gameWidth/2) + 1), 0, 0);
                    wall.name = "Wall - Left";
                    break;
                case 1:
                    wall.localScale = new Vector3(gameWidth+offset, 1, 1);
                    wall.position = new Vector3(0, (gameHeight/2) + 1, 0);
                    wall.name = "Wall - Top";
                    break;
                case 2:
                    wall.localScale = new Vector3(1, gameHeight+offset, 1);
                    wall.position = new Vector3((gameWidth/2) + 1, 0, 0);
                    wall.name = "Wall - Right";
                    break;
                case 3:
                    wall.localScale = new Vector3(gameWidth+offset, 1, 1);
                    wall.position = new Vector3(0, -((gameHeight/2) + 1), 0);
                    wall.name = "Wall - Bottom";
                    break;
            }
        }
        createdBoard = true;

        createGrid();
    }

    void createGrid()
    {
        destroyGrid();

        if (!gridArea) gridArea = new GameObject();
        gridArea.name = "GridArea";
        Transform grid;

        for (int i = 0; i < gameHeight; i++) {
            grid = Instantiate(gridPrefab, gridArea.transform);
            grid.position = new Vector3(0, (gameHeight/2)-i-0.5f, 1);
            grid.localScale = new Vector3(gameWidth+1, 0.05f, 0.05f);
            grid.name = String.Format("Grid - X ({0})", i);
        }

        for (int i = 0; i < gameWidth; i++) {
            grid = Instantiate(gridPrefab, gridArea.transform);
            grid.position = new Vector3((gameWidth/2)-i-0.5f, 0, 1);
            grid.localScale = new Vector3(0.05f, gameHeight+1, 0.05f);
            grid.name = String.Format("Grid - Y ({0})", i);
        }

        createdGrid = true;
    }

    void destroyGrid() {
        if (!createdGrid) return;

        Destroy(gridArea);
        gridArea = null;

        createdGrid = false;
    }

    void updateBoard() {
        if (!createdBoard) {
            createBoard();
            return;
        }

        Transform wall;

        for (int i = 0; i < 4; i++) {
            wall = gameArea.GetComponent<Transform>().GetChild(i).transform;
            switch (i) {
                case 0:
                    wall.localScale = new Vector3(1, gameHeight+offset, 1);
                    wall.position = new Vector3(-((gameWidth/2) + 1), 0, 0);
                    break;
                case 1:
                    wall.localScale = new Vector3(gameWidth+offset, 1, 1);
                    wall.position = new Vector3(0, (gameHeight/2) + 1, 0);
                    break;
                case 2:
                    wall.localScale = new Vector3(1, gameHeight+offset, 1);
                    wall.position = new Vector3((gameWidth/2) + 1, 0, 0);
                    break;
                case 3:
                    wall.localScale = new Vector3(gameWidth+offset, 1, 1);
                    wall.position = new Vector3(0, -((gameHeight/2) + 1), 0);
                    break;
            }
        }

        createGrid();
    }

    public void addWall() {
        Transform wall = Instantiate(wallPrefab, gameArea.transform);
        wall.name = "Wall - " + walls.Count;
        wall.position = new Vector3(0, 0, 0);
        wall.tag = "Hoverable";
        walls.Add(wall);
    }

    public void addWall(String name, Vector3 localScale, Vector3 position, String tag) {
        Transform wall = Instantiate(wallPrefab, gameArea.transform);
        wall.name = name;
        wall.localScale = localScale;
        wall.position = position;
        wall.tag = tag;
        walls.Add(wall);
    }

    public void save() {
        Directory.CreateDirectory(saveLocation);
        String fileLocation = saveLocation + "/" + levelName + ".json";
        if (!File.Exists(fileLocation)) {
            string dataString = JsonUtility.ToJson(toJson());
            File.WriteAllText(fileLocation, dataString);
        }
    }

    public void load(String fileName) {
        String fileLocation = saveLocation + "/" + fileName;
        if (!File.Exists(fileLocation)) return;

        fromJson(File.ReadAllText(fileLocation));
    }

    public LevelData toJson() {
        LevelData data = new LevelData();
        data.width = gameWidth;
        data.height = gameHeight;

        foreach (Transform wall in walls) {
            data.walls.Add(wall.GetComponent<Wall>().toJson());
        }
        
        return data;
    }

    public void fromJson(String file) {
        LevelData data = JsonUtility.FromJson<LevelData>(file);

        setGameWidth(data.width);
        setGameHeight(data.height);

        createBoard();

        // delete old walls
        foreach (Transform child in walls) {
            Destroy(child.gameObject);
        }
        walls.Clear();

        // create new walls
        foreach (WallData wall in data.walls) {
            addWall(wall.name, wall.scale, wall.position, wall.tag);
        }
    }

    public void backToMainMenu() {
        save();
        SceneManager.LoadScene(0);
    }

    public void setGameWidth(int width) {
        gameWidth = width;
        updateBoard();
    }

    // parameter has to be a string so editor allows for dynamic onValueChange
    public void setGameWidth(string width) {
        int.TryParse(width, out gameWidth);
        updateBoard();
    }

    public void setGameHeight(int height) {
        gameHeight = height;
        updateBoard();
    }

    // parameter has to be a string so editor allows for dynamic onValueChange
    public void setGameHeight(string height) {
        int.TryParse(height, out gameHeight);
        updateBoard();
    }

    public void setLevelName(string name) {
        levelName = name;
    }

    public string getSaveLocation()
    {
        return this.saveLocation;
    }
}

[Serializable]
public class LevelData {
    public int width;
    public int height;
    public List<WallData> walls = new List<WallData>();
}

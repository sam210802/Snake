using System;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    GameObject gameArea;
    GameObject gridArea;
    string gridPrefabLocation = "Grid";
    GameObject gridPrefab;
    Wall wallLeft, wallRight, wallTop, wallBottom;
    public Wall wallLeftPropery {
        get {
            return wallLeft;
        }
    }
    public Wall wallRightPropery {
        get {
            return wallRight;
        }
    }
    public Wall wallTopPropery {
        get {
            return wallTop;
        }
    }
    public Wall wallBottomPropery {
        get {
            return wallBottom;
        }
    }
    // any extra walls not including boundry walls
    List<Wall> walls = new List<Wall>();
    public List<Wall> wallsProperty {
        get {
            return walls;
        }
    }

    // odd only as game area has to be centered
    // default values on start
    // max value to avoid game freezing/crashing
    int gameWidth = 9;
    int maxWidth = 100;
    public int gameWidthProperty {
        get {
            return gameWidth;
        } set {
            gameWidth = Mathf.Min(maxWidth, value);
            updateBoard();
        }
    }

    int gameHeight = 9;
    int maxHeight = 100;
    public int gameHeightProperty {
        get {
            return gameHeight;
        } set {
            gameHeight = Mathf.Min(maxHeight, value);
            updateBoard();
        }
    }

    string levelName = "deafultLevel";
    public string levelNameProperty {
        get {
            return levelName;
        } set {
            levelName = value;
        }
    }

    bool editMode;

    static int offset = 2;

    public Board(LevelData levelData, bool editMode = false) {
        gridPrefab = Resources.Load(gridPrefabLocation) as GameObject;
        levelNameProperty = levelData.name;
        gameWidth = levelData.width;
        gameHeight = levelData.height;
        this.editMode = editMode;

        createBoard();
        
        if (levelData.walls != null) addWalls(levelData.walls);
    }

    public Board(string levelName, int width, int height, List<WallData> walls = null, bool editMode = false) : this(new LevelData(levelName, width, height, walls), editMode) {
        levelNameProperty = levelName;
        gameWidth = width;
        gameHeight = height;
    }

    // should only be called once
    void createBoard() {
        gameArea = new GameObject();
        gameArea.name = "GameArea";

        for (int i = 0; i < 4; i++) {
            Wall wall = new Wall();
            switch (i) {
                case 0:
                    wall.scaleProperty = new Vector3(1, gameHeightProperty+offset, 1);
                    wall.positionProperty = new Vector3(-((gameWidthProperty/2) + 1), 0, 0);
                    wall.nameProperty = "Wall - Left";
                    wallLeft = wall;
                    break;
                case 1:
                    wall.scaleProperty = new Vector3(gameWidthProperty+offset, 1, 1);
                    wall.positionProperty = new Vector3(0, (gameHeightProperty/2) + 1, 0);
                    wall.nameProperty = "Wall - Top";
                    wallTop = wall;
                    break;
                case 2:
                    wall.scaleProperty = new Vector3(1, gameHeightProperty+offset, 1);
                    wall.positionProperty = new Vector3((gameWidthProperty/2) + 1, 0, 0);
                    wall.nameProperty = "Wall - Right";
                    wallRight = wall;
                    break;
                case 3:
                    wall.scaleProperty = new Vector3(gameWidthProperty+offset, 1, 1);
                    wall.positionProperty = new Vector3(0, -((gameHeightProperty/2) + 1), 0);
                    wall.nameProperty = "Wall - Bottom";
                    wallBottom = wall;
                    break;
            }
            wall.transform.parent = gameArea.transform;
        }

        if (editMode) createGrid();
    }

    public void createGrid()
    {
        // easier to start over than update existing grid
        if (gridArea) {
            destroyGrid();
        }

        gridArea = new GameObject();
        gridArea.name = "GridArea";
        Transform grid;

        for (int i = 0; i < gameHeightProperty; i++) {
            grid = GameObject.Instantiate(gridPrefab.transform, gridArea.transform);
            grid.position = new Vector3(0, (gameHeightProperty/2)-i-0.5f, -1);
            grid.localScale = new Vector3(gameWidthProperty+1, 0.05f, 0.05f);
            grid.name = String.Format("Grid - X ({0})", i);
        }

        for (int i = 0; i < gameWidthProperty; i++) {
            grid = GameObject.Instantiate(gridPrefab.transform, gridArea.transform);
            grid.position = new Vector3((gameWidthProperty/2)-i-0.5f, 0, -1);
            grid.localScale = new Vector3(0.05f, gameHeightProperty+1, 0.05f);
            grid.name = String.Format("Grid - Y ({0})", i);
        }
    }

    void updateBoard() {
        // update left wall
        wallLeft.scaleProperty = new Vector3(1, gameHeightProperty+offset, 1);
        wallLeft.positionProperty = new Vector3(-((gameWidthProperty/2) + 1), 0, 0);

        // update top wall
        wallTop.scaleProperty = new Vector3(gameWidthProperty+offset, 1, 1);
        wallTop.positionProperty = new Vector3(0, (gameHeightProperty/2) + 1, 0);

        // update right wall
        wallRight.scaleProperty = new Vector3(1, gameHeightProperty+offset, 1);
        wallRight.positionProperty = new Vector3((gameWidthProperty/2) + 1, 0, 0);

        // update bottom wall
        wallBottom.scaleProperty = new Vector3(gameWidthProperty+offset, 1, 1);
        wallBottom.positionProperty = new Vector3(0, -((gameHeightProperty/2) + 1), 0);

        if (editMode) createGrid();
    }

    void destroyGrid() {
        foreach (Transform child in gridArea.transform) {
                GameObject.Destroy(child.gameObject);
            }
            GameObject.Destroy(gridArea);
            gridArea = null;
    }

    public void addWall(WallData wallData) {
        Wall wall = new Wall(wallData, editMode);
        wall.transform.parent = gameArea.transform;
        walls.Add(wall);
    }

    public void addNewWall() {
        addWall(new WallData("Wall - " + walls.Count));
    }

    public void addWalls(List<WallData> wallDataList) {
        foreach (WallData wallData in wallDataList) {
            addWall(wallData);
        }
    }

    public void removeWall(string name) {
        foreach (Wall wall in wallsProperty) {
            GameObject.Destroy(wall.transform);
            walls.Remove(wall);
        }
    }

    public void removeAllWalls() {
        foreach (Wall wall in wallsProperty) {
            GameObject.Destroy(wall.transform);
        }
        walls.Clear();
    }

    public void deleteBoard() {
        Debug.Log("Delete");
        if (gameArea) {
            Debug.Log(1);
            GameObject.Destroy(gameArea);
            gameArea = null;
        }
        if (gridArea) {
            Debug.Log(2);
            GameObject.Destroy(gridArea);
            gridArea = null;
        }
        removeAllWalls();
    }

    public LevelData toData() {
        List<WallData> walls = new List<WallData>();
        foreach (Wall wall in wallsProperty) {
            walls.Add(wall.toJson());
        }
        LevelData data = new LevelData(levelNameProperty, gameWidthProperty, gameHeightProperty, walls);
        
        return data;
    }
}

[Serializable]
public class LevelData {
    public string name;
    public int width;
    public int height;
    public List<WallData> walls = new List<WallData>();

    public LevelData(string name, int width, int height, List<WallData> walls = null) {
        this.name = name;
        this.width = width;
        this.height = height;
        this.walls = walls;
    }
}
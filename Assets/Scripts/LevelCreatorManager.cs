using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCreatorManager : MonoBehaviour
{
    // even only
    int gameWidth = 10;
    int gameHeight = 10;

    int offset = 3;

    bool created = false;

    private GameObject gameArea;
    List<Transform> walls;
    public Transform wallPrefab;

    public ToolTip toolTipScript;

    void Awake() {
        walls = new List<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
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
        if (created) {
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
        created = true;
    }

    void updateBoard() {
        if (!created) {
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
    }

    public void addWall() {
        Transform wall = Instantiate(wallPrefab, gameArea.transform);
        wall.name = "Wall - " + walls.Count;
        wall.position = new Vector3(0, 0, 0);
        wall.tag = "Hoverable";
        walls.Add(wall);
    }

    // parameter has to be a string so editor allows for dynamic onValueChange
    public void setGameWidth(string width) {
        gameWidth = Int32.Parse(width);
        updateBoard();
    }

    // parameter has to be a string so editor allows for dynamic onValueChange
    public void setGameHeight(string height) {
        gameHeight = Int32.Parse(height);
        updateBoard();
    }
}

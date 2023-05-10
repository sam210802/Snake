using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public static Food instance;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TimeBody>().Record();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // sets the transforms position this scipts attatched to
    // to a random free position within the game area
    public void RandomisePos() {
        List<Vector2> freePositions = getFreePositions();
        if (freePositions.Count == 0) {
            GameManager.instance.boardFilled = true;
            GameManager.instance.pauseGameNoGUI();
        } else {
            Vector2 newPos = freePositions[UnityEngine.Random.Range(0, freePositions.Count)];
            this.transform.position = newPos;
        }
    }

    List<Vector2> getFreePositions() {
        Board board = GameManager.instance.boardPropery;
        int xPosMin = (int) board.wallLeftPropery.positionProperty.x;
        int xPosMax = (int) board.wallRightPropery.positionProperty.x;
        int yPosMin = (int) board.wallBottomPropery.positionProperty.y;
        int yPosMax = (int) board.wallTopPropery.positionProperty.y;

        List<Transform> snakeSegments = Snake.instance.segmentsProperty;
        List<Wall> walls = board.wallsProperty;

        List<Vector2> occupiedPositions = new List<Vector2>();
        foreach (Transform snakeSegment in snakeSegments) {
            occupiedPositions.Add(new Vector2(snakeSegment.position.x, snakeSegment.position.y));
        }

        int wallXPosMin, wallXPosMax, wallYPosMin, wallYPosMax;
        float temp;
        foreach (Wall wall in walls) {
            temp = wall.positionProperty.x - Mathf.Abs(wall.scaleProperty.x/2);
            wallXPosMin = (int) Mathf.Ceil(temp);

            temp = wall.positionProperty.x + Mathf.Abs(wall.scaleProperty.x/2);
            wallXPosMax = (int) Mathf.Floor(temp);

            temp = wall.positionProperty.y - Mathf.Abs(wall.scaleProperty.y/2);
            wallYPosMin = (int) Mathf.Ceil(temp);

            temp = wall.positionProperty.y + Mathf.Abs(wall.scaleProperty.y/2);
            wallYPosMax = (int) Mathf.Floor(temp);


            for (int i = wallXPosMin; i <= wallXPosMax; i++) {
                for (int j = wallYPosMin; j <= wallYPosMax; j++) {
                    occupiedPositions.Add(new Vector2(i, j));
                }
            }
        }

        List<Vector2> positions = new List<Vector2>();
        Vector2 position;
        for (int i = xPosMin + 1; i < xPosMax; i++) {
            for (int j = yPosMin + 1; j < yPosMax; j++) {
                position = new Vector2(i, j);
                if (!occupiedPositions.Contains(position)) {
                    positions.Add(position);
                }
            }
        }

        return positions;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private LevelCreatorManager levelCreatorManager;

    private Vector3 mousePosOffset;

    void Start() {
        levelCreatorManager = GameObject.FindObjectOfType<LevelCreatorManager>();
    }

    void OnMouseDown() {
        if (!levelCreatorManager) return;

        mousePosOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    void OnMouseUp() {
        if (!levelCreatorManager) return;

        float offset = 0.5f;

        // snap to grid
        transform.position = new Vector2((float) Math.Round(transform.position.x, 0, MidpointRounding.AwayFromZero), (float) Math.Round(transform.position.y, 0, MidpointRounding.AwayFromZero));
        // need to add 0.5 to pos if scale is even else will end up 0.5 out of alignment
        if (transform.localScale.x % 2 == 0 && transform.position.x % 1 == 0) {
            if (transform.position.x < 0) offset *= -1; // this needs to be done because of awayFromZero rounding
            transform.position = new Vector2(transform.position.x - offset, transform.position.y);
        }
        if (transform.localScale.y % 2 == 0 && transform.position.y % 1 == 0) {
            if (transform.position.y < 0) offset *= -1; // this needs to be done because of awayFromZero rounding
            transform.position = new Vector2(transform.position.x, transform.position.y - offset);
        }
        // attatch this wall to the tooltip object
        levelCreatorManager.toolTipScript.setAttatchedObject(gameObject);
        levelCreatorManager.transformTipScript.attatchedObjectProperty = gameObject;
    }

    void OnMouseDrag() {
        if (!levelCreatorManager) return;

        transform.position = GetMouseWorldPos() + mousePosOffset;
    }

    private Vector3 GetMouseWorldPos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public WallData toJson() {
        WallData data = new WallData();
        data.name = gameObject.name;
        data.tag = tag;
        data.scale = transform.localScale;
        data.position = transform.position;

        return data;
    }
}

[Serializable]
public class WallData {
    public string name;
    public string tag;
    public Vector3 scale;
    public Vector3 position;
}

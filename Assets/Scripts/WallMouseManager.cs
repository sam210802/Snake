using System;
using UnityEngine;

public class WallMouseManager : MonoBehaviour
{
    private Vector3 mousePosOffset;

    void OnMouseDown() {
        if (!LevelCreatorManager.instance) return;

        mousePosOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    void OnMouseUp() {
        if (!LevelCreatorManager.instance) return;

        float offset = 0.5f;

        // snap to grid
        transform.position = new Vector2((float) Math.Round(transform.position.x, 0, MidpointRounding.AwayFromZero),
        (float) Math.Round(transform.position.y, 0, MidpointRounding.AwayFromZero));
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
        LevelCreatorManager.instance.toolTipScript.setAttatchedObject(gameObject);
        LevelCreatorManager.instance.transformTipScript.attatchedObjectProperty = gameObject;
    }

    void OnMouseDrag() {
        if (!LevelCreatorManager.instance) return;

        transform.position = GetMouseWorldPos() + mousePosOffset;
    }

    private Vector3 GetMouseWorldPos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}

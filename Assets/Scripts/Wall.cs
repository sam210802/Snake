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

        levelCreatorManager.toolTipScript.setAttatchedObject(gameObject);

        // TODO
        // snap wall position to grid
    }

    void OnMouseDrag() {
        if (!levelCreatorManager) return;

        transform.position = GetMouseWorldPos() + mousePosOffset;
    }

    private Vector3 GetMouseWorldPos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}

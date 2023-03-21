using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTip : MonoBehaviour
{
    private LevelCreatorManager levelCreatorManager;

    private Vector3 mousePos;
    private float originalWidth;

    private GameObject attatchedObject;

    void Start() {
        levelCreatorManager = GameObject.FindObjectOfType<LevelCreatorManager>();
    }

    public void setAttatchedObject(GameObject gameObject) {
        attatchedObject = gameObject;
        // the gameobject the script is attatched to
        this.gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(200, 200, 0));
        this.gameObject.transform.position = 
            new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
        this.gameObject.SetActive(true);
    }

    void OnMouseDown() {
        if (!levelCreatorManager) return;

        mousePos = Input.mousePosition;
        originalWidth = attatchedObject.transform.localScale.x;
    }

    void OnMouseDrag() {
        if (!levelCreatorManager) return;

        float newWidth = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - Camera.main.ScreenToWorldPoint(mousePos).x + originalWidth;
        int newWidthRounded = Mathf.RoundToInt(newWidth);

        Transform parent = this.gameObject.transform.Find("TransformTipX");
        if (newWidthRounded <= 0) {
            parent.localRotation = Quaternion.Euler(0, 0, 180);
            newWidth *= -1;
        } else {
            parent.localRotation = Quaternion.Euler(0, 0, 0);
        }

        levelCreatorManager.toolTipScript.setObjectWidth(newWidthRounded.ToString());
        Transform body = parent.GetChild(0).transform;
        Transform arrow = parent.GetChild(1).transform;

        float oldScale = body.localScale.x;
        body.localScale = new Vector2(newWidth - originalWidth, body.localScale.y);
        float newScale = body.localScale.x;
        body.localPosition = new Vector3(body.localPosition.x + ((newScale - oldScale) / 2), body.localPosition.y, body.localPosition.z);

        arrow.localPosition = new Vector3(arrow.localPosition.x + (newScale - oldScale), arrow.localPosition.y, arrow.localPosition.z);

        Debug.Log("new - Old: " + (newScale - oldScale));
    }

    void OnMouseUp() {
        reset();
    }

    void reset() {
        Transform parent = this.gameObject.transform.Find("TransformTipX");
        Transform body = parent.GetChild(0);
        Transform arrow = parent.GetChild(1);

        // resset rotation
        parent.localRotation = Quaternion.Euler(0, 0, 0);

        // reset scale
        body.localScale = new Vector2(1, 1);
        arrow.localScale = new Vector2(1, 1);

        // reset position
        body.localPosition = new Vector3(1, 0, 0);
        arrow.localPosition = new Vector3(1.79f, 0, 0);
    }
}

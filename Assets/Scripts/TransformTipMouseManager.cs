using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTipMouseManager : MonoBehaviour
{   
    [SerializeField]
    TransformTip transformTipScript;

    Vector3 initialMousePos;
    float originalSize, newSize, mousePosChange;
    Transform body, arrow;

    Quaternion originalRotation;
    Vector2 bodyOriginalScale, arrowOriginalScale;
    Vector3 bodyOriginalPosition, arrowOriginalPosition;

    void Start() {
        body = transform.GetChild(0);
        arrow = transform.GetChild(1);

        originalRotation = transform.localRotation;
        
        bodyOriginalScale = body.localScale;
        arrowOriginalScale = arrow.localScale;

        bodyOriginalPosition = body.position;
        arrowOriginalPosition = arrow.position;
    }

    void OnMouseDown() {
        if (!transformTipScript.levelCreatorManager) return;

        // store initial mouse pos location
        initialMousePos = Input.mousePosition;
        // store original size of the attatched object
        if (gameObject.name == "TransformTipX") {
            originalSize = transformTipScript.attatchedObject.transform.localScale.x;
        } else {
            originalSize = transformTipScript.attatchedObject.transform.localScale.y;
        }
    }

    void OnMouseDrag() {
        if (!transformTipScript.levelCreatorManager) return;

        // calculate new attatched object size
        // set attatched object size to calculated size
        if (gameObject.name == "TransformTipX") {
            mousePosChange = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - Camera.main.ScreenToWorldPoint(initialMousePos).x;
            newSize = mousePosChange + originalSize;
            transformTipScript.levelCreatorManager.toolTipScript.setObjectWidth((int) Math.Round(newSize, 0, MidpointRounding.AwayFromZero));
        } else {
            mousePosChange = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - Camera.main.ScreenToWorldPoint(initialMousePos).y;
            newSize = mousePosChange + originalSize;
            transformTipScript.levelCreatorManager.toolTipScript.setObjectHeight((int) Math.Round(newSize, 0, MidpointRounding.AwayFromZero));

        }

        calculatePos();
    }

    void OnMouseUp() {
        reset();
    }

    void calculatePos() {
        float oldScale, newScale, scaleDifference;

        oldScale = body.localScale.x;
        body.localScale = new Vector2(Mathf.Abs(mousePosChange), body.localScale.y);
        newScale = body.localScale.x;

        Vector3 newAngle = transform.eulerAngles;
        // negative or possitive
        if (mousePosChange < 0) {
            newAngle.z = 180;
        } else {
            newAngle.z = 0;
        }
        transform.eulerAngles = newAngle;

        scaleDifference = newScale - oldScale;
        body.localPosition = new Vector3((body.localScale.x + 1)/2, body.localPosition.y, body.localPosition.z);
        arrow.localPosition = new Vector3(body.localPosition.x*2, arrow.localPosition.y, arrow.localPosition.z);
    }

    void reset() {
        // resset rotation
        transform.localRotation = originalRotation;

        // reset scale
        body.localScale = new Vector2(1, 1);
        arrow.localScale = new Vector2(1, 1);

        // reset position
        body.localPosition = new Vector3(1, 0, 0);
        arrow.localPosition = new Vector3(1.79f, 0, 0);
    }
}

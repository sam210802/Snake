using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTipMouseManager : MonoBehaviour
{   
    [SerializeField]
    TransformTip transformTipScript;

    Vector3 initialMousePos;
    float originalSize, newSize;
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
        float oldScale, newScale, scaleDifference;

        // calculate new attatched object size
        // set attatched object size to calculated size
        if (gameObject.name == "TransformTipX") {
            newSize = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - Camera.main.ScreenToWorldPoint(initialMousePos).x + originalSize;
            transformTipScript.levelCreatorManager.toolTipScript.setObjectWidth(Math.Round(newSize, 0, MidpointRounding.AwayFromZero).ToString());
        } else {
            newSize = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - Camera.main.ScreenToWorldPoint(initialMousePos).y + originalSize;
            transformTipScript.levelCreatorManager.toolTipScript.setObjectHeight(Math.Round(newSize, 0, MidpointRounding.AwayFromZero).ToString());

        }

        if (newSize < 0) {
            transform.localRotation = Quaternion.Euler(originalRotation.eulerAngles.x, originalRotation.eulerAngles.y, originalRotation.eulerAngles.z + 180);
            newSize *= -1;
        } else {
            transform.localRotation = originalRotation;
        }

        oldScale = body.localScale.x;
        body.localScale = new Vector2(newSize - originalSize, body.localScale.y);
        newScale = body.localScale.x;

        scaleDifference = newScale - oldScale;
        body.localPosition = new Vector3(body.localPosition.x + (scaleDifference / 2), body.localPosition.y, body.localPosition.z);
        arrow.localPosition = new Vector3(arrow.localPosition.x + scaleDifference, arrow.localPosition.y, arrow.localPosition.z);
    }

    void OnMouseUp() {
        reset();
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

using System;
using UnityEngine;

public class TransformTipMouseManager : MonoBehaviour
{   
    [SerializeField]
    TransformTip transformTipScript;

    Vector3 initialMousePos;
    float originalSize, newSize, mousePosChange;
    Transform body, head;

    Quaternion originalRotation;
    Vector2 bodyOriginalScale, arrowOriginalScale;
    Vector3 bodyOriginalPosition, arrowOriginalPosition;

    void Start() {
        body = transform.GetChild(0);
        head = transform.GetChild(1);

        originalRotation = transform.localRotation;
        
        bodyOriginalScale = body.localScale;
        arrowOriginalScale = head.localScale;

        bodyOriginalPosition = body.position;
        arrowOriginalPosition = head.position;
    }

    void OnMouseDown() {
        if (!transformTipScript.levelCreatorManager) return;

        // store initial mouse pos location
        initialMousePos = Input.mousePosition;
        // store original size of the attatched object
        if (gameObject.name == "TransformTipX") {
            originalSize = transformTipScript.attatchedObjectProperty.transform.localScale.x;
        } else {
            originalSize = transformTipScript.attatchedObjectProperty.transform.localScale.y;
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

    // calculate position, scale, and rotation of transform tip
    void calculatePos() {
        Vector3 newAngle = transform.eulerAngles;
        if (mousePosChange < 0) {
            newAngle.z = 180;
        } else {
            newAngle.z = 0;
        }
        // arrow rotation
        transform.eulerAngles = newAngle;
        // arrow body scale
        body.localScale = new Vector2(Mathf.Abs(mousePosChange), body.localScale.y);
        // arrow body position
        body.localPosition = new Vector3((body.localScale.x + 1)/2, body.localPosition.y, body.localPosition.z);
        // arrow head position
        head.localPosition = new Vector3(body.localPosition.x*2, head.localPosition.y, head.localPosition.z);
    }

    void reset() {
        // resset rotation
        transform.localRotation = originalRotation;

        // reset scale
        body.localScale = bodyOriginalScale;
        head.localScale = arrowOriginalScale;

        // reset position
        body.localPosition = bodyOriginalPosition;
        head.localPosition = arrowOriginalPosition;
    }
}

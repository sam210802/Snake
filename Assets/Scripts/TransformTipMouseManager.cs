using System;
using UnityEngine;

public class TransformTipMouseManager : MonoBehaviour
{   
    Vector3 initialMousePos;
    float originalSize, newSize, mousePosChange;
    Transform body, head;

    Vector3 originalRotation;
    Vector2 bodyOriginalScale, arrowOriginalScale;
    Vector3 bodyOriginalPosition, arrowOriginalPosition;

    void Start() {
        body = transform.GetChild(0);
        head = transform.GetChild(1);

        originalRotation = transform.localEulerAngles;
        
        bodyOriginalScale = body.localScale;
        arrowOriginalScale = head.localScale;

        bodyOriginalPosition = body.localPosition;
        arrowOriginalPosition = head.localPosition;
    }

    void OnMouseDown() {
        if (!LevelCreatorManager.instance) return;

        // store initial mouse pos location
        initialMousePos = Input.mousePosition;
        // store original size of the attatched object
        if (gameObject.name == "TransformTipX") {
            originalSize = TransformTip.instance.attatchedObjectProperty.transform.localScale.x;
        } else {
            originalSize = TransformTip.instance.attatchedObjectProperty.transform.localScale.y;
        }
    }

    void OnMouseDrag() {
        if (!LevelCreatorManager.instance) return;

        // calculate new attatched object size
        // set attatched object size to calculated size
        if (gameObject.name == "TransformTipX") {
            mousePosChange = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - Camera.main.ScreenToWorldPoint(initialMousePos).x;
            newSize = mousePosChange + originalSize;
            LevelCreatorManager.instance.toolTipScript.setObjectWidth((int) Math.Round(newSize, 0, MidpointRounding.AwayFromZero));
        } else {
            mousePosChange = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - Camera.main.ScreenToWorldPoint(initialMousePos).y;
            newSize = mousePosChange + originalSize;
            LevelCreatorManager.instance.toolTipScript.setObjectHeight((int) Math.Round(newSize, 0, MidpointRounding.AwayFromZero));

        }

        calculatePos();
    }

    void OnMouseUp() {
        reset();
    }

    // calculate position, scale, and rotation of transform tip
    void calculatePos() {
        Vector3 newAngle = transform.localEulerAngles;
        if (mousePosChange < 0) {
            newAngle.z = originalRotation.z + 180;
        } else {
            newAngle.z = originalRotation.z;
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
        transform.localEulerAngles = originalRotation;

        // reset scale
        body.localScale = bodyOriginalScale;
        head.localScale = arrowOriginalScale;

        // reset position
        body.localPosition = bodyOriginalPosition;
        head.localPosition = arrowOriginalPosition;
    }
}

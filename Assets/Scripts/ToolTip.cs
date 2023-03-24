using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    [SerializeField]
    private TMP_Text toolTipTitleText;
    [SerializeField]
    private TMP_InputField toolTipWidthText;
    [SerializeField]
    private TMP_InputField toolTipHeightText;

    private GameObject attatchedObject;

    public void setTitle(string text) {
        toolTipTitleText.text = text;
    }

    public void setWidthText(string text) {
        toolTipWidthText.text = text;
    }

    public void setHeightText(string text) {
        toolTipHeightText.text = text;
    }

    public void setAttatchedObject(GameObject gameObject) {
        attatchedObject = gameObject;
        setTitle(attatchedObject.name);
        setWidthText(attatchedObject.transform.localScale.x + "");
        setHeightText(attatchedObject.transform.localScale.y + "");
        // the gameobject the script is attatched to
        this.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        this.gameObject.SetActive(true);
    }

    public void setObjectWidth(int width) {
        if (width == 0) {
            return;
        }

        float oldScaleX = attatchedObject.transform.localScale.x;

        // changes width of object to user inputted width
        attatchedObject.transform.localScale = 
            new Vector2(width, attatchedObject.transform.localScale.y);

        float newScaleX = attatchedObject.transform.localScale.x;

        float scaleDifference = newScaleX - oldScaleX;

        // keeps the walls position but extends in the x/right direction
        attatchedObject.transform.position = new Vector3(attatchedObject.transform.position.x + (scaleDifference / 2),
                                                attatchedObject.transform.position.y,
                                                attatchedObject.transform.position.z);

        // updates tooltip text to new width
        setWidthText(attatchedObject.transform.localScale.x.ToString());
    }

    // parameter has to be string for on change to be dynamic
    public void setObjectWidth(string stringWidth) {
        int width = (int) attatchedObject.transform.localScale.x;
        int.TryParse(stringWidth, out width);
        setObjectWidth(width);
    }

    public void setObjectHeight(int height) {
        if (height == 0) {
            return;
        }

        float oldScaleY = attatchedObject.transform.localScale.y;
    
        // changes height of object to user inputted height
        attatchedObject.transform.localScale = 
            new Vector2(attatchedObject.transform.localScale.x, height);
    
        float newScaleY = attatchedObject.transform.localScale.y;

        float scaleDifference = newScaleY - oldScaleY;
    
        // keeps the walls position but extends in the y/up direction
        attatchedObject.transform.position = new Vector3(attatchedObject.transform.position.x,
                                                attatchedObject.transform.position.y + (scaleDifference / 2),
                                                attatchedObject.transform.position.z);

        // updates tooltip text to new height
        setHeightText(attatchedObject.transform.localScale.y.ToString());
    }

    // parameter has to be string for on change to be dynamic
    public void setObjectHeight(string stringHeight) {
        int height = (int) attatchedObject.transform.localScale.y;
        int.TryParse(stringHeight, out height);
        setObjectHeight(height);
    }
}

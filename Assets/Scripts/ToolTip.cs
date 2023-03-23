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

    // parameter has to be string for on change to be dynamic
    public void setObjectWidth(string stringWidth) {
        int width = Int32.Parse(stringWidth);
        int invert = 1;
        if (width == 0) {
            return;
        } else if (width < 0) {
            invert = -1;
        }

        float oldScaleX = attatchedObject.transform.localScale.x;

        // changes width of object to user inputted width
        attatchedObject.transform.localScale = 
            new Vector2(width * invert, attatchedObject.transform.localScale.y);

        float newScaleX = attatchedObject.transform.localScale.x;

        float scaleDifference = (newScaleX - oldScaleX) * invert;

        // keeps the walls position but extends in the x/right direction
        attatchedObject.transform.position = new Vector3(attatchedObject.transform.position.x + (scaleDifference / 2),
                                                attatchedObject.transform.position.y,
                                                attatchedObject.transform.position.z);

        // updates tooltip text to new width
        setWidthText(attatchedObject.transform.localScale.x.ToString());
    }

    // parameter has to be string for on change to be dynamic
    public void setObjectHeight(string stringHeight) {
        int height = Int32.Parse(stringHeight);
        int invert = 1;
        if (height == 0) {
            return;
        } else if (height < 0) {
            invert = -1;
        }

        float oldScaleY = attatchedObject.transform.localScale.y;
    
        // changes height of object to user inputted height
        attatchedObject.transform.localScale = 
            new Vector2(attatchedObject.transform.localScale.x, height * invert);
    
        float newScaleY = attatchedObject.transform.localScale.y;

        float scaleDifference = (newScaleY - oldScaleY) * invert;
    
        // keeps the walls position but extends in the y/up direction
        attatchedObject.transform.position = new Vector3(attatchedObject.transform.position.x,
                                                attatchedObject.transform.position.y + (scaleDifference / 2),
                                                attatchedObject.transform.position.z);

        // updates tooltip text to new height
        setHeightText(attatchedObject.transform.localScale.y.ToString());
    }
}

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
        // gameobject script is attatched to
        this.gameObject.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        this.gameObject.SetActive(true);
    }

    // parameter has to be string for on change to be dynamic
    public void setObjectWidth(string width) {
        if (Int32.Parse(width) <= 0) {
            return;
        }

        float oldScaleX = attatchedObject.transform.localScale.x;

        // changes width of object to user inputted width
        attatchedObject.transform.localScale = 
            new Vector2(Int32.Parse(width), attatchedObject.transform.localScale.y);

        float newScaleX = attatchedObject.transform.localScale.x;

        // keeps the walls position but extends in the x/right direction
        attatchedObject.transform.position = new Vector3(attatchedObject.transform.position.x + ((newScaleX - oldScaleX) / 2),
                                                attatchedObject.transform.position.y,
                                                attatchedObject.transform.position.z);
    }

    // parameter has to be string for on change to be dynamic
    public void setObjectHeight(string height) {
        if (Int32.Parse(height) <= 0) {
            return;
        }

        float oldScaleY = attatchedObject.transform.localScale.y;
    
        // changes height of object to user inputted height
        attatchedObject.transform.localScale = 
            new Vector2(attatchedObject.transform.localScale.x, Int32.Parse(height));
    
        float newScaleY = attatchedObject.transform.localScale.y;
    
        // keeps the walls position but extends in the y/up direction
        attatchedObject.transform.position = new Vector3(attatchedObject.transform.position.x,
                                                attatchedObject.transform.position.y + ((newScaleY - oldScaleY) / 2),
                                                attatchedObject.transform.position.z);
    }
}

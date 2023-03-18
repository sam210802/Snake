using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContentSizeFitterV2 : MonoBehaviour
{
    [SerializeField]
    int minWidth = 100;
    [SerializeField]
    int maxWidth = 300;
    [SerializeField]
    int minHeight = 100;
    [SerializeField]
    int maxHeight = 100;

    enum Fill {Width, Height, Even};
    [SerializeField]
    Fill fillMode;

    TMP_Text text;
    RectTransform rect;

    void Awake() {
        rect = GetComponent<RectTransform>();
        text = GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rect.sizeDelta = new Vector2(minWidth, minHeight);
    }

    // Update is called once per frame
    void Update()
    {
        updateSize();
    }

    void updateSize() {
        if (!text.isTextOverflowing) return;

        // expand rect transform depending on fill mode
        if (fillMode.Equals(Fill.Width)) {
            if (rect.sizeDelta.x < maxWidth) {
                rect.sizeDelta = new Vector2(Mathf.Min(rect.sizeDelta.x + 10, maxWidth), rect.sizeDelta.y);
            } else if (rect.sizeDelta.x == maxWidth) {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, Mathf.Min(rect.sizeDelta.y + 10, maxHeight));
            }
        } else if (fillMode.Equals(Fill.Height)) {
            if (rect.sizeDelta.y < maxHeight) {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, Mathf.Min(rect.sizeDelta.y + 10, maxHeight));
            } else if (rect.sizeDelta.y == maxHeight) {
                rect.sizeDelta = new Vector2(Mathf.Min(rect.sizeDelta.x + 10, maxWidth), rect.sizeDelta.y);
            }
        } else if (fillMode.Equals(Fill.Even)) {
            rect.sizeDelta = new Vector2(Mathf.Min(rect.sizeDelta.x + 10, maxWidth), Mathf.Min(rect.sizeDelta.y + 10, maxHeight));
        }
    }
}

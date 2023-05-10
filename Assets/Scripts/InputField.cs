using UnityEngine;
using TMPro;

public class InputField : MonoBehaviour
{
    [SerializeField]
    GameObject textObject;
    TMP_Text text;
    RectTransform textRect;

    [SerializeField]
    GameObject inputFieldObject;
    TMP_InputField inputField;
    RectTransform inputFieldRect;

    RectTransform rect;

    void Awake() {
        rect = GetComponent<RectTransform>();
        text = textObject.GetComponent<TMP_Text>();
        textRect = textObject.GetComponent<RectTransform>();
        inputField = inputFieldObject.GetComponent<TMP_InputField>();
        inputFieldRect = inputFieldObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3((-rect.sizeDelta.x/2)+(textRect.sizeDelta.x/2), 0, 0);
        textRect.anchoredPosition = pos;

        pos = new Vector3((rect.sizeDelta.x/2)-(inputFieldRect.sizeDelta.x/2), 0, 0);
        inputFieldRect.anchoredPosition = pos;
    }
}

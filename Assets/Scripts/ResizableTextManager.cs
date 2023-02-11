using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

// this class searches for any text with the "Text - Resizable" tag
// and text with this tag is scaled according to the user input text size
public class ResizableTextManager : MonoBehaviour
{
    // location where default text sizes are stored
    static string textSizeFile = "Assets/Data/textSize.json";
    static string currentTextSize = "X-LARGE";

    [SerializeField]
    TMP_Text text;

    void Awake() {
        updateTextSize();
    }

    void Start() {

    }

    public void updateTextSize() {
        // return if file doesn't exist
        if (!File.Exists(textSizeFile)) return;

        string fileContent = File.ReadAllText(textSizeFile);
        Dictionary<string, float> textSize = JsonConvert.DeserializeObject<Dictionary<string, float>>(fileContent);

        text.fontSize = (int) (text.fontSize * textSize[currentTextSize]);
        Debug.Log(text.fontSize);
    }
}

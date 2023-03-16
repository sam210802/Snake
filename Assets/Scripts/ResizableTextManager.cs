using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// this class searches for any text with the "Text - Resizable" tag
// and text with this tag is scaled according to the user input text size
public class ResizableTextManager : MonoBehaviour
{
    // location where default text sizes are stored
    static string textSizeFile = "Assets/Data/textSize.json";
    
    [SerializeField]
    int defaultFontSize = 24;

    string currentFontSize;
    string currentText;

    [SerializeField]
    TMP_Text text;

    [SerializeField]
    private RectTransform parentLayout;

    bool active = false;

    void Awake() {
        currentFontSize = OptionsMenu.loadTextPrefs();
        currentText = text.text;
    }

    void OnEnable() {
        StartCoroutine(coroutines());

    }

    void Update() {
        // updates text font size if saved font size changed
        if (currentFontSize != OptionsMenu.loadTextPrefs()) {
            updateTextSizeCoroutine();
            currentFontSize = OptionsMenu.loadTextPrefs();
        }
        // updates text font size if text changed
        if (currentText != text.text) {
            updateTextSizeCoroutine();
            currentText = text.text;
        }
    }

    private void updateTextSizeCoroutine() {
        if (active) return;
        StartCoroutine(coroutines());
    }

    // ensures text size is updated before the layout can be rebuilt
    public IEnumerator coroutines() {
        active = true;
        yield return StartCoroutine(updateTextSize());
        yield return StartCoroutine(forceLayoutRebuild());
        active = false;
    }

    IEnumerator updateTextSize() {
        // break if file doesn't exist
        if (!File.Exists(textSizeFile)) yield break;

        string fileContent = File.ReadAllText(textSizeFile);
        Dictionary<string, float> textSize = JsonConvert.DeserializeObject<Dictionary<string, float>>(fileContent);

        text.fontSize = defaultFontSize * textSize[OptionsMenu.loadTextPrefs()];
        yield return null;
    }

    IEnumerator forceLayoutRebuild() {
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentLayout);
        yield return null;
    }
}

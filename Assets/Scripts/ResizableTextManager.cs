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
    private int defaultFontSize = 24;

    public int defaultFontSizePropery {
        get {
            return defaultFontSize;
        } set {
            defaultFontSize = value;
            updateTextSize();
        }
    }

    string currentFontSize;
    string currentText;

    [SerializeField]
    TMP_Text text;

    [SerializeField]
    private List<RectTransform> parentLayouts;

    // true if text or font changed since last update
    public bool textUpdated = false;
    public bool forceUpdate = false;

    void Awake() {
        currentFontSize = OptionsMenu.loadTextPrefs();
        currentText = text.text;
    }

    void OnEnable() {
        updateTextSize();

    }

    void Update() {
        textUpdated = false;

        // forces text to update
        if (forceUpdate) {
            updateTextSize();
            forceUpdate = false;
        }
        // updates text font size if saved font size changed
        if (currentFontSize != OptionsMenu.loadTextPrefs()) {
            updateTextSize();
            currentFontSize = OptionsMenu.loadTextPrefs();
        }
        // updates text font size if text changed
        if (currentText != text.text) {
            updateTextSize();
            currentText = text.text;
        }
    }

    public void updateTextSize() {
        // break if file doesn't exist
        if (!File.Exists(textSizeFile)) return;

        string fileContent = File.ReadAllText(textSizeFile);
        Dictionary<string, float> textSize = JsonConvert.DeserializeObject<Dictionary<string, float>>(fileContent);

        text.fontSize = defaultFontSize * textSize[OptionsMenu.loadTextPrefs()];

        forceLayoutRebuild();
    }

    void forceLayoutRebuild() {
        foreach (RectTransform parent in parentLayouts) {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
        }
    }

    public void addParentLayout(RectTransform parent) {
        parentLayouts.Add(parent);
    }
}

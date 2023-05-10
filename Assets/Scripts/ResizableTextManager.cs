using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TextPrefs {XSMALL=0, SMALL=1, MEDIUM=2, LARGE=3, XLARGE=4};

public class ResizableTextManager : MonoBehaviour
{
    // location where default text sizes are stored
    static string textSizeFile = "textSize";
    
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

    TextPrefs currentFontSize;
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
        // testing purposes
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
        TextAsset file = Resources.Load<TextAsset>(textSizeFile);

        // break if file doesn't exist
        if (file == null) return;

        Dictionary<string, float> textSize = JsonConvert.DeserializeObject<Dictionary<string, float>>(file.ToString());

        text.fontSize = defaultFontSize * textSize[OptionsMenu.loadTextPrefs().ToString()];

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

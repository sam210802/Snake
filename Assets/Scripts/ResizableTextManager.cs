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
    static int defaultFontSize = 24;

    [SerializeField]
    TMP_Text text;

    [SerializeField]
    private RectTransform parentLayout;

    void Awake() {
        StartCoroutine(coroutines());

    }

    void OnEnable() {
        StartCoroutine(coroutines());

    }

    void Start() {

    }

    public void startCoroutines() {
        StartCoroutine(coroutines());
    }

    public IEnumerator coroutines() {
        yield return StartCoroutine(updateTextSize());
        yield return StartCoroutine(forceLayoutRebuild());
    }

    private IEnumerator updateTextSize() {
        // break if file doesn't exist
        if (!File.Exists(textSizeFile)) yield break;

        string fileContent = File.ReadAllText(textSizeFile);
        Dictionary<string, float> textSize = JsonConvert.DeserializeObject<Dictionary<string, float>>(fileContent);

        text.fontSize = defaultFontSize * textSize[OptionsMenu.loadTextPrefs()];
        yield return null;
    }

    public static IEnumerator updateAllText() {
        ResizableTextManager[] scripts = FindObjectsOfType<ResizableTextManager>();
        foreach (ResizableTextManager script in scripts) {
            script.startCoroutines();
        }
        yield return null;
    }

    private IEnumerator forceLayoutRebuild() {
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentLayout);
        yield return null;
    }
}

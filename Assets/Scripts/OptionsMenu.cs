using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startCoroutines(TMP_Dropdown dropdown) {
        StartCoroutine(coroutines(dropdown));
    }

    private IEnumerator coroutines(TMP_Dropdown dropdown) {
        yield return StartCoroutine(saveTextPrefs(dropdown));
        yield return StartCoroutine(ResizableTextManager.updateAllText());
    }

    public static IEnumerator saveTextPrefs(TMP_Dropdown dropdown) {
        PlayerPrefs.SetString("TextSize", dropdown.options[dropdown.value].text);
        yield return null;
    }

    public static string loadTextPrefs() {
        return PlayerPrefs.GetString("TextSize", "MEDIUM");
    }
}

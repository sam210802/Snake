using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class OptionsMenu : MonoBehaviour {

    [SerializeField]
    private TMP_Dropdown textSizeDropdown;
    [SerializeField]
    private TMP_Dropdown languageDropdown;

    void Start() {
        var listOfOptions = textSizeDropdown.options.Select(OptionsMenu => OptionsMenu.text).ToList();
        textSizeDropdown.value = listOfOptions.IndexOf(loadTextPrefs());

        listOfOptions = languageDropdown.options.Select(OptionsMenu => OptionsMenu.text).ToList();
        languageDropdown.value = loadLocalePrefs();
    }

    public void saveTextPrefsCoroutine(TMP_Dropdown dropdown) {
        StartCoroutine(saveTextPrefs(dropdown.options[dropdown.value].text));
    }

    private static IEnumerator saveTextPrefs(string value) {
        PlayerPrefs.SetString("TextSize", value);
        yield return null;
    }

    public static string loadTextPrefs() {
        return PlayerPrefs.GetString("TextSize", "MEDIUM");
    }

    public void setLocaleCoroutine(int localeID) {
        StartCoroutine(setLocale(localeID));
    }

    // sets locale then saves new locale to storage
    public static IEnumerator setLocale(int localeID) {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        yield return saveLocalePrefs(localeID);
    }

    private static IEnumerator saveLocalePrefs(int localeID) {
        PlayerPrefs.SetInt("LocaleID", localeID);
        yield return null;
    }

    public static int loadLocalePrefs() {
        return PlayerPrefs.GetInt("LocaleID", 0);
    }
}

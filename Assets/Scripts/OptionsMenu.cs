using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Utilities.Localization;

public class OptionsMenu : MonoBehaviour {

    [SerializeField]
    private TMP_Dropdown textSizeDropdown;
    [SerializeField]
    LocalizeDropdown textSizeDropdownLocale;
    [SerializeField]
    private TMP_Dropdown languageDropdown;
    [SerializeField]
    LocalizeDropdown languageDropdownLocale;

    void Start() {
        int index;
        index = (int) loadTextPrefs();
        textSizeDropdown.value = index;
        textSizeDropdownLocale.selectedOptionIndex = index;

        index = loadLocalePrefs();
        languageDropdown.value = index;
    }

    public void saveTextPrefs(TMP_Dropdown dropdown) {
        StartCoroutine(saveTextPrefs((TextPrefs) dropdown.value));
    }

    static IEnumerator saveTextPrefs(TextPrefs textPref) {
        PlayerPrefs.SetString("TextSize", textPref.ToString());
        yield return null;
    }

    public static TextPrefs loadTextPrefs() {
        TextPrefs textPref = TextPrefs.MEDIUM;
        Enum.TryParse(PlayerPrefs.GetString("TextSize", TextPrefs.MEDIUM.ToString()), out textPref);
        return textPref;

    }

    public void startLocaleCoroutine(int localeID) {
        StartCoroutine(setLocale(localeID));
    }

    // sets locale then saves new locale to storage
    public static IEnumerator setLocale(int localeID) {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        saveLocalePrefs(localeID);
        yield return null;
    }

    private static void saveLocalePrefs(int localeID) {
        PlayerPrefs.SetInt("LocaleID", localeID);
    }

    public static int loadLocalePrefs() {
        return PlayerPrefs.GetInt("LocaleID", 0);
    }
}

public enum TextPrefs {XSMALL=0, SMALL=1, MEDIUM=2, LARGE=3, XLARGE=4};
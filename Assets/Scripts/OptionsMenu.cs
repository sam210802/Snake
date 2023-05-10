using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
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
    [SerializeField]
    Slider mainVolumeSlider;
    [SerializeField]
    Slider musicVolumeSlider;
    [SerializeField]
    Slider SFXVolumeSlider;

    void Start() {
        int index;
        index = (int) loadTextPrefs();
        textSizeDropdown.value = index;
        textSizeDropdownLocale.selectedOptionIndex = index;

        index = loadLocalePrefs();
        languageDropdown.value = index;

        mainVolumeSlider.value = PlayerPrefs.GetFloat("Main_Volume", 1.0f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFX_Volume", 1.0f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("Music_Volume", 1.0f);
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

    public static void SetMainVolume(float volume) {
        PlayerPrefs.SetFloat("Main_Volume", volume);
        AudioManager.ChangeMusicVolume();
        AudioManager.ChangeSFXVolume();
    }

    public static void SetMusicVolume(float volume) {
        PlayerPrefs.SetFloat("Music_Volume", volume);
        AudioManager.ChangeMusicVolume();
    }

    public static void SetSFXVolume(float volume) {
        PlayerPrefs.SetFloat("SFX_Volume", volume);
        AudioManager.ChangeSFXVolume();
    }
}
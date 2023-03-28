using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Awake() {
        StartCoroutine(OptionsMenu.setLocale(OptionsMenu.loadLocalePrefs()));
    }

    public void PlayGame() {
        LevelLoader.LoadGame();
    }

    public void PlayLevelCreator() {
        LevelLoader.LoadLevelCreator();
    }

    public void Exit() {
        LevelLoader.Exit();
    }
}

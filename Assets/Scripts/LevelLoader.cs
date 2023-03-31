using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    // main menu = 0
    // game = 1
    // level creator = 2

    public static void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

    public static void LoadGame() {
        SceneManager.LoadScene(1);
    }

    public static void LoadLevelCreator() {
        SceneManager.LoadScene(2);
    }

    public static void Exit() {
        Debug.Log("Quit");
        Application.Quit();
    }
}

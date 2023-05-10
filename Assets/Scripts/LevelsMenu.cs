using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsMenu : MonoBehaviour
{

    [SerializeField]
    GameObject levelButtonPrefab;

    [SerializeField]
    Transform defaultLevelsGridLayout;

    [SerializeField]
    Transform customLevelsGridLayout;

    void OnEnable() {
        resetDefaultLevelsGridLayout();
        foreach (Board board in LevelSaveLoadManager.loadAllDefault()) {
            int levelNumber;
            int.TryParse(Regex.Match(board.levelNameProperty, "\\d+").ToString(), out levelNumber);
            if (PlayerPrefs.GetInt("MaxLevelUnlocked", 1) >= levelNumber) {
                newButton(levelNumber, defaultLevelsGridLayout);
            }
        }

        resetCustomLevelsGridLayout();
        foreach (Board board in LevelSaveLoadManager.loadAll()) {
            newButton(board.levelNameProperty, customLevelsGridLayout);
        }
    }

    void resetDefaultLevelsGridLayout() {
        foreach (Transform child in defaultLevelsGridLayout) {
            Destroy(child.gameObject);
        }
    }

    void resetCustomLevelsGridLayout() {
        foreach (Transform child in customLevelsGridLayout) {
            Destroy(child.gameObject);
        }
    }

    public void playLevel(string levelName) {
        GameManager.setCurrentLevel(levelName, false);
        LevelLoader.LoadGame();
    }

    public void playLevel(int levelNumber) {
        GameManager.setCurrentLevel(levelNumber);
        LevelLoader.LoadGame();
    }

    GameObject newButton(string levelName, Transform transform) {
        GameObject button = Instantiate(levelButtonPrefab, transform);
        button.GetComponent<Button>().onClick.AddListener(() => playLevel(levelName));
        button.GetComponentInChildren<TMP_Text>().text = levelName;
        return button;
    }

    GameObject newButton(int levelNumber, Transform transform) {
        GameObject button = Instantiate(levelButtonPrefab, transform);
        button.GetComponent<Button>().onClick.AddListener(() => playLevel(levelNumber));
        button.GetComponentInChildren<TMP_Text>().text = levelNumber.ToString("00");
        return button;
    }
}

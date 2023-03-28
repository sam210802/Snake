using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsMenu : MonoBehaviour
{

    [SerializeField]
    GameObject levelButtonPrefab;

    [SerializeField]
    Transform gridLayout;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable() {
        resetGridLayout();
        foreach (Board board in LevelSaveLoadManager.loadAll()) {
            newButton(board.levelNameProperty, gridLayout);
        }
    }

    void resetGridLayout() {
        foreach (Transform child in gridLayout) {
            Destroy(child.gameObject);
        }
    }

    public void playLevel(string levelName) {
        GameManager.currentLevel = levelName + ".json";
        LevelLoader.LoadGame();
    }

    GameObject newButton(string levelName, Transform transform) {
        GameObject button = Instantiate(levelButtonPrefab, transform);
        button.GetComponent<Button>().onClick.AddListener(() => playLevel(levelName));
        button.GetComponentInChildren<TMP_Text>().text = levelName;
        return button;
    }
}

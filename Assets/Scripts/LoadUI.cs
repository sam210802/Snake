using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadUI : MonoBehaviour
{
    [SerializeField]
    GameObject textPrefab;

    [SerializeField]
    GameObject buttonPrefab;

    void OnEnable() {
        GameObject titleText = Instantiate(textPrefab, transform);
        titleText.GetComponent<TMP_Text>().text = "Levels";
        titleText.GetComponent<ResizableTextManager>().addParentLayout(transform.parent.GetComponent<RectTransform>());

        if (Directory.Exists(LevelSaveLoadManager.getSaveLocation())) {
            DirectoryInfo info = new DirectoryInfo(LevelSaveLoadManager.getSaveLocation());
            FileInfo[] fileInfo = info.GetFiles();
            foreach (FileInfo file in fileInfo) {
                if (file.Extension == ".json") {
                    createButton(file.Name);
                }
            }
        }
        GameObject exitButton = Instantiate(buttonPrefab, transform);
        exitButton.GetComponentInChildren<TMP_Text>().text = "Exit";
        exitButton.GetComponent<Button>().onClick.AddListener(() => exit());
    }

    void createButton(string name) {
        GameObject button = Instantiate(buttonPrefab, transform);
        button.GetComponentInChildren<TMP_Text>().text = name.Split(".")[0];
        button.GetComponentInChildren<ResizableTextManager>().defaultFontSizePropery = 16;
        button.GetComponent<Button>().onClick.AddListener(() => {
            LevelCreatorManager.instance.newBoard(name);
            exit();});
    }

    void exit() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadUI : MonoBehaviour
{
    [SerializeField]
    LevelCreatorManager levelCreatorManager;

    [SerializeField]
    GameObject buttonPrefab;

    void OnEnable() {
        DirectoryInfo info = new DirectoryInfo(levelCreatorManager.getSaveLocation());
        FileInfo[] fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo) {
            if (file.Extension == ".json") {
                createButton(file.Name);
            }
        }
        GameObject exitButton = Instantiate(buttonPrefab, transform);
        exitButton.GetComponentInChildren<TMP_Text>().text = "Exit";
        exitButton.GetComponent<Button>().onClick.AddListener(() => exit());
    }

    void createButton(string name) {
        GameObject button = Instantiate(buttonPrefab, transform);
        button.GetComponentInChildren<TMP_Text>().text = name;
        button.GetComponent<Button>().onClick.AddListener(() => {
            levelCreatorManager.load(name);
            exit();});
    }

    void exit() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        gameObject.SetActive(false);
    }
}

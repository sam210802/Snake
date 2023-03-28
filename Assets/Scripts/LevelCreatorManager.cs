using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCreatorManager : MonoBehaviour
{
    Board board;
    public static LevelCreatorManager instance;

    int defaultWidth = 9;
    int defaultHeight = 9;

    public ToolTip toolTipScript;
    public TransformTip transformTipScript;

    [SerializeField]
    Transform gridPrefab;

    [SerializeField]
    TMP_InputField widthInput;
    
    [SerializeField]
    TMP_InputField heightInput;

    [SerializeField]
    TMP_InputField levelNameInput;

    void Awake() {
        instance = this;
        StartCoroutine(OptionsMenu.setLocale(OptionsMenu.loadLocalePrefs()));
    }

    // Start is called before the first frame update
    void Start()
    {
        newBoard();
    }

    public void newBoard(string levelName = null) {
        Debug.Log("New Board");
        if (board != null) {
            Debug.Log(1);
            board.deleteBoard();
        }
        if (levelName != null) {
            Debug.Log("Level Name: " + levelName);
            board = LevelSaveLoadManager.load(levelName, true);
        }
        if (board == null) {
            Debug.Log(3);
            board = new Board("level_01", defaultHeight, defaultWidth, null, true);
        }

        widthInput.text = board.gameWidthProperty.ToString();
        heightInput.text = board.gameHeightProperty.ToString();
        levelNameInput.text = board.levelNameProperty;
    }

    public void addWall() {
        board.addNewWall();
    }

    public void save() {
        LevelSaveLoadManager.save(board);
    }

    public void backToMainMenu() {
        LevelSaveLoadManager.save(board);
        SceneManager.LoadScene(0);
    }

    public void setGameWidth(int width) {
        board.gameWidthProperty = width;
        // since width is capped we need to reset input text
        widthInput.text = board.gameWidthProperty.ToString();
    }

    // parameter has to be a string so editor allows for dynamic onValueChange
    public void setGameWidth(string width) {
        int newWidth = board.gameWidthProperty;
        int.TryParse(width, out newWidth);
        board.gameWidthProperty = newWidth;
        // since width is capped we need to reset input text
        widthInput.text = board.gameWidthProperty.ToString();
    }

    public void setGameHeight(int height) {
        board.gameHeightProperty = height;
        // since height is capped we need to reset input text
        heightInput.text = board.gameHeightProperty.ToString();
    }

    // parameter has to be a string so editor allows for dynamic onValueChange
    public void setGameHeight(string height) {
        int newHeight = board.gameHeightProperty;
        int.TryParse(height, out newHeight);
        board.gameHeightProperty = newHeight;
        // since height is capped we need to reset input text
        heightInput.text = board.gameHeightProperty.ToString();
    }

    public void setLevelName(string name) {
        board.levelNameProperty = name;
    }

    public void updateBoard(Board board) {
        this.board.deleteBoard();
        this.board = board;
    }
}

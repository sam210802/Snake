using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    [SerializeField]
    private TMP_Text scoreText;
    public int scoreProperty {
        get {
            return int.Parse(scoreText.text);
        }
    }

    [SerializeField]
    private TMP_Text highScoreText;
    public int highScoreProperty {
        get {
            return int.Parse(highScoreText.text);
        }
    }

    void Awake() {
        instance = this;
    }

    public void updateScore() {
        scoreText.text = Snake.instance.segmentsProperty.Count.ToString();
        updateHighScore();
    }

    public void updateHighScore() {
        setHighScore();
        highScoreText.text = PlayerPrefs.GetInt(GameManager.instance.boardPropery.levelNameProperty + "_HighScore", 0).ToString();
    }

    // sets high score to score if score higher than old high score
    public void setHighScore() {
        int highScore = PlayerPrefs.GetInt(GameManager.instance.boardPropery.levelNameProperty + "_HighScore", 0);
        int score = 0;
        int.TryParse(scoreText.text, out score);
        highScore = Mathf.Max(score, highScore);
        PlayerPrefs.SetInt(GameManager.instance.boardPropery.levelNameProperty + "_HighScore", highScore);
    }
}

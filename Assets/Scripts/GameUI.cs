using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text highScoreText;

    void Awake() {
        instance = this;
    }

    public void updateScore() {
        scoreText.text = scoreText.text.Split(":")[0] + ": " + Snake.instance.segmentsProperty.Count;
        updateHighScore();
    }

    public void updateHighScore() {

        highScoreText.text = highScoreText.text.Split(":")[0] + ": "  + PlayerPrefs.GetInt("HighScore", 0);
    }

    // sets high score to score if score higher than old high score
    public void setHighScore() {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        int score = 0;
        int.TryParse(scoreText.text, out score);
        highScore = Mathf.Max(score, highScore);
        PlayerPrefs.SetInt("HighScore", highScore);
    }
}

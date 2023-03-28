using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    private TMP_Text scoreText;
    private TMP_Text highScoreText;
    private GameObject snake;

    void Awake() {
        instance = this;
    }

    public void updateScore() {
        setSnakeObject();
        setScoreTextObject();

        scoreText.text = snake.transform.childCount.ToString();
        updateHighScore();
    }

    public void updateHighScore() {
        setHighScoreTextObject();

        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
    }

    // sets high score to score if score higher than old high score
    public void setHighScore() {
        setScoreTextObject();
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        int score = 0;
        int.TryParse(scoreText.text, out score);
        highScore = Mathf.Max(score, highScore);
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    void setScoreTextObject() {
        if (scoreText == null) scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TMP_Text>();
    }

    void setHighScoreTextObject() {
        if (highScoreText == null) highScoreText = GameObject.FindGameObjectWithTag("HighScore").GetComponent<TMP_Text>();
    }

    void setSnakeObject() {
        if (snake == null) snake = GameObject.FindGameObjectWithTag("Snake");
    }
}

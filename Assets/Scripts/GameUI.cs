using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private TMP_Text scoreText;
    private TMP_Text highScoreText;
    private GameObject snake;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        updateScore();
        updateHighScore();
    }

    void updateScore() {
        if (snake == null) snake = GameObject.FindGameObjectWithTag("Snake");
        if (scoreText == null) scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TMP_Text>();

        scoreText.text = snake.transform.childCount.ToString();
    }

    void updateHighScore() {
        if (highScoreText == null) highScoreText = GameObject.FindGameObjectWithTag("HighScore").GetComponent<TMP_Text>();

        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0);
    }

    public void setHighScore() {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        // saves score if higher than current high score
        PlayerPrefs.SetInt("HighScore", Mathf.Max(Int32.Parse(scoreText.text), highScore));
    }
}

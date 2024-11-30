using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;

    void Start()
    {
        // Retrieve the player's score from PlayerPrefs
        int playerScore = PlayerPrefs.GetInt("PlayerScore", 0);

        // Retrieve the high score from PlayerPrefs
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Display the scores
        scoreText.text = playerScore.ToString();
        highScoreText.text = highScore.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Text scoreUI;
    public int score;


    public void UpdateScore(int newScore)
    {
        score = newScore;
        scoreUI.text = "Score: " + score.ToString(); // Update the score UI
    }

private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Enforce singleton pattern
        }
    }



    public void PlayerLose()
    {
        Debug.Log("Player has lost the game!");

        // Retrieve the current high score from PlayerPrefs (default to 0 if not set)
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Check if the current score is higher than the saved high score
        if (score > highScore)
        {
            // Update the high score in PlayerPrefs
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save(); // Save the new high score
        }

        // Save the current score for this game
        PlayerPrefs.SetInt("PlayerScore", score);
        PlayerPrefs.Save(); // Save the score to disk

        // Load the Game Over scene
        SceneManager.LoadScene(2);
    }


}

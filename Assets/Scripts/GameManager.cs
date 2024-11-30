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
        scoreUI.text = score.ToString(); // Update the score UI
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
        // Save the score to PlayerPrefs
        PlayerPrefs.SetInt("PlayerScore", score);
        PlayerPrefs.Save(); // Save to disk
                            // Load the Game Over scene (or another scene that shows the score)
        SceneManager.LoadScene(2);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene loading
using UnityEngine.SocialPlatforms.Impl; // Provides implementation for social platforms (e.g., leaderboards)
using UnityEngine.UI; // For working with UI elements like Text

public class GameManager : MonoBehaviour
{
    // Static instance for Singleton pattern to ensure there's only one GameManager
    public static GameManager Instance { get; private set; }

    // Reference to the UI Text component that displays the player's score
    public Text scoreUI;

    // Current score of the player
    public int score;

    // Updates the player's score and reflects it in the UI
    public void UpdateScore(int newScore)
    {
        score = newScore; // Update the internal score variable
        scoreUI.text = "Score: " + score.ToString(); // Update the displayed score text
    }

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Check if there's already an instance of GameManager
        if (Instance == null)
        {
            Instance = this; // Set this as the active instance
        }
        else
        {
            Destroy(gameObject); // Destroy any duplicate GameManager instances
        }
    }

    // Handles the player's loss and manages high scores and scene transition
    public void PlayerLose()
    {
        Debug.Log("Player has lost the game!"); // Log a message for debugging purposes

        // Retrieve the saved high score from PlayerPrefs (default is 0)
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // If the player's current score exceeds the high score, update it
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score); // Save the new high score
            PlayerPrefs.Save(); // Ensure the new high score is written to disk
        }

        // Save the player's current score to PlayerPrefs
        PlayerPrefs.SetInt("PlayerScore", score);
        PlayerPrefs.Save(); // Persist the player's score to disk

        // Load the Game Over scene (assumed to have an index of 2 in the build settings)
        SceneManager.LoadScene(2);
    }
}

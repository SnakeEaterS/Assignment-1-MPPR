using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    // Public variable to set the wait time in the Inspector
    public float waitTime = 140f;

    public void StartGame()
    {
        // Start the coroutine to wait for the specified time before loading the scene
        StartCoroutine(WaitAndStartGame(waitTime));
    }

    private IEnumerator WaitAndStartGame(float waitTime)
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(waitTime);

        // Load the scene after the delay
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        // Start the coroutine to wait for the specified time before exiting
        StartCoroutine(WaitAndExitGame(waitTime));
    }

    private IEnumerator WaitAndExitGame(float waitTime)
    {
        // Wait for the specified amount of time before quitting
        yield return new WaitForSeconds(waitTime);

        // Exits the game
        Application.Quit();
    }
}

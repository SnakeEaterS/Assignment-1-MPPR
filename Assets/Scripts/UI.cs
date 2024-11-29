using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI : MonoBehaviour
{
    public void StartGame()
        //Starts the game by loading the scene.
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
        //Exits the game.
    {
        Application.Quit();

    }
}


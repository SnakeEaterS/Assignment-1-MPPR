using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        // Implement loss logic (e.g., show Game Over screen)
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For working with the UI Slider component

public class HealthBar : MonoBehaviour
{
    // Reference to the UI Slider component that represents the health bar
    [SerializeField] private Slider slider;

    /// <summary>
    /// Updates the health bar to reflect the player's current health.
    /// </summary>
    /// <param name="currentValue">The player's current health.</param>
    /// <param name="maxValue">The player's maximum health.</param>
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        // Set the slider's value to the normalized health percentage (current health / max health)
        slider.value = currentValue / maxValue;
    }
}

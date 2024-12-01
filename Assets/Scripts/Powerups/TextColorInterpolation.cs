using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColorInterpolation : MonoBehaviour // Color interpolation for UI Text
{
    public Text uiText; // Reference to the UI Text
    private Shooting shooting;  // Reference to the Shooting script
    private Color defaultColor; // To store the original color of the sprite

    void Start()
    {
        shooting = Shooting.Instance;
        if (uiText != null)
        {
            uiText.gameObject.SetActive(false); // Hides text upon start (if it exists)
        }
        defaultColor = uiText.color;
    }

    void Update()
    {
        if (shooting.powerUpActive)
        {
            uiText.gameObject.SetActive(true); // Show the power-up text when power-up is active

            // Calculate t based on PingPong, which creates an oscillation from 0 to 1
            float t = Mathf.PingPong(Time.time, 1);

            // Linear interpolation formula for the RGB channels are as follows:
            // Interpolated Value = (1 - t) * Start Value + t * End Value

            // Interpolate between yellow and white, manually interpolate each color channel using interpolation formula
            float r = (1 - t) * 1 + t * 1;  // Yellow has r=1, white has r=1
            float g = (1 - t) * 1 + t * 1;  // Yellow has g=1, white has g=1
            float b = (1 - t) * 0 + t * 1;  // Yellow has b=0, white has b=1

            // Create a new color using the interpolated RGB
            Color lerpedColor = new Color(r, g, b);

            // Apply the color interpolation to the material/sprite
            uiText.color = lerpedColor;
        }
        else
        {
            uiText.gameObject.SetActive(false);
            uiText.color = defaultColor; // Resets the text color back to deafult
        }
    }
}


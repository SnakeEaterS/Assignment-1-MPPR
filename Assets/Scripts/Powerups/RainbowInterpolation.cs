using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowInterpolation : MonoBehaviour // Color Interpolation for Player
{
    new Renderer renderer;
    public float speed = 0.5f; // Speed factor to control the interpolation speed (smaller = slower)
    private Color defaultColor; // To store the original color of the sprite

    private Shooting shooting;  // Reference to the Shooting script

    void Start()
    {
        renderer = GetComponent<Renderer>();
        shooting = Shooting.Instance;
        defaultColor = renderer.material.color;
    }

    void Update()
    {
        if (shooting.powerUpActive)
        {
            // Calculate t based on PingPong to create an oscillation from 0 to 1
            float t = Mathf.PingPong(Time.time * speed, 1);

            // Linear interpolation formula to calculate hue from 0 to 1
            float hueStart = 0f; // Red
            float hueEnd = 1f;   // Loop back to red
            float hue = (1 - t) * hueStart + t * hueEnd; // Interpolated Value = (1 - t) * Start value of hue + t * End value of hue;

            // Use HSVToRGB to convert hue into an RGB color
            // Set saturation and value to 1 for full color brightness
            Color rainbowColor = Color.HSVToRGB(hue, 1, 1);

            // Apply the interpolated rainbow color to the material
            renderer.material.color = rainbowColor;
        }
        else
        {
            renderer.material.color = defaultColor; // Resets the sprite back to deafult color once the power-up duration is over
        }
    }
}

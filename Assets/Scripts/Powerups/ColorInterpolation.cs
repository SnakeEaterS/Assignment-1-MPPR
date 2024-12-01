using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorInterpolation : MonoBehaviour // Color Interpolation for Power-up & Power-up Bullet
{
    new Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
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
        renderer.material.color = lerpedColor;
    }
}


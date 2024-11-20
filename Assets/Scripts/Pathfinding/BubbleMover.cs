using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleMover : MonoBehaviour
{
    public BezierCurve bezierCurve;
    public float speed = 2f; // Speed of the sprite
    private float t = 0f;    // Progress along the curve

    void Update()
    {
        if (bezierCurve != null)
        {
            // Increment t based on speed and time
            t += speed * Time.deltaTime;

            // Clamp t between 0 and 1
            t = Mathf.Clamp01(t);

            // Update sprite position
            transform.position = bezierCurve.GetPoint(t);

            // Optional: Rotate sprite to face direction
            if (t < 1)
            {
                Vector2 direction = bezierCurve.GetPoint(t + 0.01f) - (Vector2)transform.position;
                if (direction != Vector2.zero)
                    transform.up = direction; // Align sprite's up direction with movement
            }
        }
        
    }

}

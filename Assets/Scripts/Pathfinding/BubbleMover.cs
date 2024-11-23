using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BubbleMover : MonoBehaviour
{
    public BezierCurve bezierCurve; // Reference to the BezierCurve script
    public float speed = 20f;       // Speed of movement along the curve
    private float t = 0f;          // Progress along the curve (0 to 1)

    void Update()
    {
        if (bezierCurve == null || bezierCurve.controlPoints == null || bezierCurve.controlPoints.Length < 4)
        {
            Debug.LogWarning("BezierCurve or control points are not set properly.");
            return;
        }
        
        // Move along the curve by incrementing t
        t += speed * Time.deltaTime / bezierCurve.controlPoints.Length;

        // Clamp t between 0 and 1 to stay within the curve
        if (t > 1f)
        {
            t = 0f; // Reset to loop back to the start of the curve
        }

        // Get the position on the curve and update the object's position
        Vector3 positionOnCurve = bezierCurve.GetPoint(t);
        transform.position = positionOnCurve;
    }
}
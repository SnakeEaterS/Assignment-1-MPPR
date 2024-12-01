using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform[] controlPoints; // Array to hold control points for the B?zier curve.

    void Awake()
    {
        // Find all GameObjects in the scene with the tag "Point".
        GameObject[] pointObjects = GameObject.FindGameObjectsWithTag("Point");

        // Sort the GameObjects by their numeric suffix in natural order (e.g., P1, P2, ..., P12).
        System.Array.Sort(pointObjects, (a, b) =>
        {
            return ExtractNumber(a.name).CompareTo(ExtractNumber(b.name));
        });

        // Initialize the controlPoints array with the sorted transforms.
        controlPoints = new Transform[pointObjects.Length];
        for (int i = 0; i < pointObjects.Length; i++)
        {
            controlPoints[i] = pointObjects[i].transform;
        }
    }

    // Helper method to extract the numeric part of a string, e.g., from "P1" it extracts "1".
    private int ExtractNumber(string name)
    {
        // Use a regular expression to find the first numeric part of the string.
        string numberPart = System.Text.RegularExpressions.Regex.Match(name, @"\d+").Value;
        return int.Parse(numberPart); // Convert the extracted number to an integer.
    }

    public Vector3 GetPoint(float t)
    {
        // Check if there are enough control points (at least 4) to calculate a cubic B?zier curve.
        if (controlPoints == null || controlPoints.Length < 4)
        {
            Debug.LogWarning("Insufficient control points to calculate B?zier curve.");
            return Vector3.zero; // Return zero if there are not enough control points.
        }

        // Calculate the number of B?zier segments based on control points (every 4 points make 1 segment).
        int numSegments = (controlPoints.Length - 1) / 3;
        int segmentIndex = Mathf.Clamp(Mathf.FloorToInt(t * numSegments), 0, numSegments - 1); // Get the current segment index.
        float segmentT = (t * numSegments) - segmentIndex; // Calculate the local t value within the segment.

        // Get the indices of the control points for the current segment.
        int p0Index = segmentIndex * 3;
        int p1Index = Mathf.Clamp(p0Index + 1, 0, controlPoints.Length - 1);
        int p2Index = Mathf.Clamp(p1Index + 1, 0, controlPoints.Length - 1);
        int p3Index = Mathf.Clamp(p2Index + 1, 0, controlPoints.Length - 1);

        // Get the positions of the control points.
        Vector3 p0 = controlPoints[p0Index].position;
        Vector3 p1 = controlPoints[p1Index].position;
        Vector3 p2 = controlPoints[p2Index].position;
        Vector3 p3 = controlPoints[p3Index].position;

        // Calculate B?zier blending factors for cubic interpolation.
        float u = 1 - segmentT;
        float tt = segmentT * segmentT;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * segmentT;

        // Return the final interpolated point on the B?zier curve.
        return (uuu * p0) + (3 * uu * segmentT * p1) + (3 * u * tt * p2) + (ttt * p3);
    }

    public float GetLength(int numSamples = 100)
    {
        float length = 0f; // Initialize the length variable.
        Vector3 previousPoint = GetPoint(0f); // Get the point at the start of the curve (t=0).

        // Sample the curve at regular intervals to estimate its length.
        for (int i = 1; i <= numSamples; i++)
        {
            float t = (float)i / numSamples; // Calculate the t value for this sample.
            Vector3 currentPoint = GetPoint(t); // Get the point on the curve at t.
            length += Vector3.Distance(previousPoint, currentPoint); // Add the distance between the current and previous points.
            previousPoint = currentPoint; // Update the previous point.
        }

        return length; // Return the total estimated length of the curve.
    }

    void OnDrawGizmos()
    {
        if (controlPoints != null && controlPoints.Length >= 4) // Ensure there are enough control points to draw the curve.
        {
            Vector3 previousPoint = controlPoints[0].position; // Start with the first control point.

            // Draw the curve in small steps from t=0 to t=1.
            for (float t = 0; t <= 1; t += 0.05f)
            {
                Vector3 point = GetPoint(t); // Get the point on the curve at this t.
                Gizmos.color = Color.green; // Set the Gizmo color to green.
                Gizmos.DrawLine(previousPoint, point); // Draw a line from the previous point to the current point.
                previousPoint = point; // Update the previous point.
            }
        }
    }
}

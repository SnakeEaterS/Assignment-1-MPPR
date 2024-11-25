using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform[] controlPoints; // Array of control points

    void Awake()
    {
        // Find all GameObjects with the tag "Point"
        GameObject[] pointObjects = GameObject.FindGameObjectsWithTag("Point");

        // Sort using natural sorting
        System.Array.Sort(pointObjects, (a, b) =>
        {
            return ExtractNumber(a.name).CompareTo(ExtractNumber(b.name));
        });

        // Initialize the controlPoints array with the sorted Transform components
        controlPoints = new Transform[pointObjects.Length];
        for (int i = 0; i < pointObjects.Length; i++)
        {
            controlPoints[i] = pointObjects[i].transform;
        }
    }

    // Helper method to extract the numeric part of a string
    private int ExtractNumber(string name)
    {
        // Assumes the name has a format like "P1", "P2", ..., "P12"
        string numberPart = System.Text.RegularExpressions.Regex.Match(name, @"\d+").Value;
        return int.Parse(numberPart);
    }

    public Vector3 GetPoint(float t)
    {
        if (controlPoints == null || controlPoints.Length < 4)
        {
            Debug.LogWarning("Insufficient control points to calculate Bézier curve.");
            return Vector3.zero;
        }

        int numSegments = (controlPoints.Length - 1) / 3;
        int segmentIndex = Mathf.Clamp(Mathf.FloorToInt(t * numSegments), 0, numSegments - 1);
        float segmentT = (t * numSegments) - segmentIndex;

        int p0Index = segmentIndex * 3;
        int p1Index = Mathf.Clamp(p0Index + 1, 0, controlPoints.Length - 1);
        int p2Index = Mathf.Clamp(p1Index + 1, 0, controlPoints.Length - 1);
        int p3Index = Mathf.Clamp(p2Index + 1, 0, controlPoints.Length - 1);

        Vector3 p0 = controlPoints[p0Index].position;
        Vector3 p1 = controlPoints[p1Index].position;
        Vector3 p2 = controlPoints[p2Index].position;
        Vector3 p3 = controlPoints[p3Index].position;

        float u = 1 - segmentT;
        float tt = segmentT * segmentT;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * segmentT;

        return (uuu * p0) + (3 * uu * segmentT * p1) + (3 * u * tt * p2) + (ttt * p3);
    }

    public float GetLength(int numSamples = 100)
    {
        float length = 0f;
        Vector3 previousPoint = GetPoint(0f);
        for (int i = 1; i <= numSamples; i++)
        {
            float t = (float)i / numSamples;
            Vector3 currentPoint = GetPoint(t);
            length += Vector3.Distance(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
        return length;
    }

    void OnDrawGizmos()
    {
        if (controlPoints != null && controlPoints.Length >= 4)
        {
            Vector3 previousPoint = controlPoints[0].position;
            for (float t = 0; t <= 1; t += 0.05f)
            {
                Vector3 point = GetPoint(t);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(previousPoint, point);
                previousPoint = point;
            }
        }
    }
}

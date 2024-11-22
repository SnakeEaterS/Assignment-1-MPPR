using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform[] controlPoints; // Array of control points

    private Transform P0, P1, P2, P3;

    void Awake()
    {
        // Find all GameObjects with the tag "Point"
        GameObject[] pointObjects = GameObject.FindGameObjectsWithTag("Point");

        // Initialize the controlPoints array with the found Transform components
        controlPoints = new Transform[pointObjects.Length];
        for (int i = 0; i < pointObjects.Length; i++)
        {
            controlPoints[i] = pointObjects[i].transform;
        }
    }

    public Vector3 GetPoint(float t)
    {
        int numSegments = (controlPoints.Length - 1) / 3;
        int segmentIndex = Mathf.FloorToInt(t * numSegments);
        float segmentT = (t * numSegments) - segmentIndex;

        int p0Index = segmentIndex * 3;
        int p1Index = p0Index + 1;
        int p2Index = p1Index + 1;
        int p3Index = p2Index + 1;

        Vector2 p0 = controlPoints[p0Index].position;
        Vector2 p1 = controlPoints[p1Index].position;
        Vector2 p2 = controlPoints[p2Index].position;
        Vector2 p3 = controlPoints[p3Index].position;

        float u = 1 - segmentT;
        float tt = segmentT * segmentT;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * segmentT;

        Vector2 point2D = (uuu * p0) + (3 * uu * segmentT * p1) + (3 * u * tt * p2) + (ttt * p3);

        // Create a Vector3 with the Z-axis set to -1
        Vector3 point = new Vector3(point2D.x, point2D.y, -1);

        return point;
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

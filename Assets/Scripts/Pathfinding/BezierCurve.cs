using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Transform P0, P1, P2, P3;

    public Vector2 GetPoint(float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 point = (uuu * (Vector2)P0.position) + (3 * uu * t * (Vector2)P1.position) + (3 * u * tt * (Vector2)P2.position) + (ttt * (Vector2)P3.position);

        return point;
    }

    void OnDrawGizmos()
    {
        if (P0 != null && P1 != null && P2 != null && P3 != null)
        {
            Vector2 previousPoint = P0.position;
            for (float t = 0; t <= 1; t += 0.05f)
            {
                Vector2 point = GetPoint(t);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(previousPoint, point);
                previousPoint = point;
            }
        }
    }
}

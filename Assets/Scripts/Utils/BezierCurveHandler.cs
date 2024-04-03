using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fish.Utils
{
    /// <summary>
    /// Function only used to calculate Bezier curves
    /// </summary>
    public static class BezierCurveHandler
    {
        /// <summary>
        /// Function that will return a point on a bezier curve formed by the inputted four points
        /// </summary>
        /// <param name="startPoint">Start point of the curve (t=0)</param>
        /// <param name="p1">First point separated from the curve</param>
        /// <param name="p2">Second point separated from the curve</param>
        /// <param name="endPoint">End point of the curve (t=1)</param>
        /// <param name="t">Time</param>
        /// <returns>A vector2 containing the position on the curve</returns>
        public static Vector2 GetPointOnBezierCurve(Vector2 startPoint, Vector2 p1, Vector2 p2, Vector2 endPoint, float t)
        {
            float u = 1f - t;
            float t2 = t * t;
            float u2 = u * u;
            float u3 = u2 * u;
            float t3 = t2 * t;

            Vector2 result =
                (u3) * startPoint +
                (3f * u2 * t) * p1 +
                (3f * u * t2) * p2 +
                (t3) * endPoint;

            return result;
        }

    }
}

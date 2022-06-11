using Microsoft.Xna.Framework;

namespace Roguecraft.Engine.Helpers;

/// <summary>
/// Common mathematical functions with vectors
/// </summary>
public static class VectorMath
{
    public static bool AreCollinear(this Vector2 origin, Vector2 pointA, Vector2 pointB)
    {
        var area = origin.X * (pointA.Y - pointB.Y) +
                   pointA.X * (pointB.Y - origin.Y) +
                   pointB.X * (origin.Y - pointA.Y);

        return Math.Abs(area) < 1f;
    }

    public static Vector2 ClampMagnitude(this Vector2 vector, float maxLength)
    {
        float sqrMagnitude = vector.LengthSquared();
        if (sqrMagnitude <= maxLength * maxLength)
        {
            return vector;
        }
        float mag = MathF.Sqrt(sqrMagnitude);
        //these intermediate variables force the intermediate result to be
        //of float precision. without this, the intermediate result can be of higher
        //precision, which changes behavior.
        float normalized_x = vector.X / mag;
        float normalized_y = vector.Y / mag;
        return new Vector2(normalized_x * maxLength, normalized_y * maxLength);
    }

    /// <summary>
    /// Returns a slightly shortened version of the vector:
    /// p * (1 - f) + q * f
    /// </summary>
    public static Vector2 Interpolate(Vector2 p, Vector2 q, float f)
    {
        return new Vector2(p.X * (1.0f - f) + q.X * f, p.Y * (1.0f - f) + q.Y * f);
    }

    /// <summary>
    /// Returns if the point is 'left' of the line p1-p2
    /// </summary>
    public static bool LeftOf(Vector2 p1, Vector2 p2, Vector2 point)
    {
        float cross = (p2.X - p1.X) * (point.Y - p1.Y)
                    - (p2.Y - p1.Y) * (point.X - p1.X);

        return cross < 0;
    }

    /// <summary>
    /// Computes the intersection point of the line p1-p2 with p3-p4
    /// </summary>
    public static Vector2 LineLineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        // From http://paulbourke.net/geometry/lineline2d/
        var s = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X))
                / ((p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y));
        return new Vector2(p1.X + s * (p2.X - p1.X), p1.Y + s * (p2.Y - p1.Y));
    }

    public static Vector2 MaxDistance(this Vector2 vector, Vector2 other, float maxLength)
    {
        var d = vector - other;
        return d.ClampMagnitude(maxLength) + other;
    }
}
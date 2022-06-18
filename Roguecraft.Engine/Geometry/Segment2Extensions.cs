using MonoGame.Extended;

namespace Roguecraft.Engine.Geometry;

public static class Segment2Extensions
{
    public static bool Intersects(this Segment2 segment, CircleF circle)
    {
        var closest = segment.ClosestPointTo(circle.Center);
        return circle.Contains(closest);
    }

    public static bool Intersects(this Segment2 segment, RectangleF rectangle)
    {
        var closest = segment.ClosestPointTo(rectangle.Center);
        return rectangle.Contains(closest);        
    }
}
using MonoGame.Extended;

namespace Roguecraft.Engine.Visibility;

public static class Segment2Extensions
{
    public static bool Intersects(this Segment2 segment, CircleF circle)
    {
        var closest = segment.ClosestPointTo(circle.Center);
        return circle.Contains(closest);
    }
}
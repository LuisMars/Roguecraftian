using MonoGame.Extended;

namespace Roguecraft.Engine.Helpers;

public static class RectangleFExtensions
{
    public static bool Contains(this RectangleF rect, RectangleF other)
    {
        return (rect.X <= other.X) &&
               (rect.Y <= other.Y) &&
               ((other.X + other.Width) <= rect.Right) &&
               ((other.Y + other.Height) <= rect.Bottom);
    }

    public static bool Contains(this RectangleF rect, CircleF other)
    {
        return (rect.X <= other.Position.X - other.Radius) &&
               (rect.Y <= other.Position.Y - other.Radius) &&
               ((other.Position.X + other.Radius) <= rect.Right) &&
               ((other.Position.Y + other.Radius) <= rect.Bottom);
    }
}
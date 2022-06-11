using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Roguecraft.Engine.Helpers;

public static class ShapeHelpers
{
    public static bool Contains(this IShapeF shape, Vector2 position)
    {
        return shape switch
        {
            CircleF circle => circle.Contains(position),
            RectangleF rectangle => rectangle.Contains(position),
            _ => throw new NotImplementedException()
        };
    }
}
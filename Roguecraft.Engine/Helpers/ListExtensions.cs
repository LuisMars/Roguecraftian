using Microsoft.Xna.Framework;

namespace Roguecraft.Engine.Helpers;

public static class ListExtensions
{
    public static Vector2 Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Vector2> selector)
    {
        var sum = new Vector2();
        foreach (var item in source.Select(selector))
        {
            sum += item;
        }
        return sum;
    }
}
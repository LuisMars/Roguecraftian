using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Roguecraft.Engine.Helpers;

public static class MathUtils
{
    private static readonly Random _random = new();

    public static T Choice<T>(this List<T> list)
    {
        return list[_random.Next(list.Count - 1)];
    }

    public static float RandomNormal()
    {
        var u = 0f;
        var v = 0f;
        while (u == 0)
        {
            u = (float)_random.NextDouble(); //Converting [0,1) to (0,1)
        }

        while (v == 0)
        {
            v = (float)_random.NextDouble();
        }

        var num = MathF.Sqrt(-2.0f * MathF.Log(u)) * MathF.Cos(2.0f * MathF.PI * v);
        num = num / 10f + 0.5f; // Translate to 0 -> 1
        if (num > 1 || num < 0)
        {
            return RandomNormal(); // resample between 0 and 1
        }

        return num;
    }

    public static Vector2 RandomVector(int size)
    {
        return new Vector2((RandomNormal() - 0.5f) * size, (RandomNormal() - 0.5f) * size);
    }

    internal static float Layer(Transform2 transform)
    {
        return 0.5f + transform.Position.Y / 100000f;
    }
}
using Microsoft.Xna.Framework;
using Warband.Engines.Random;

namespace Roguecraft.Engine.Random;

public class RandomGenerator : XXHash
{
    private int RandomIndex { get; set; }

    public float Float()
    {
        return Value(RandomIndex++);
    }

    public int Next(int min, int max)
    {
        return Range(min, max + 1, RandomIndex++);
    }

    public int Next(int max)
    {
        return Next(0, max);
    }

    public float RandomNormal()
    {
        var u = 0f;
        var v = 0f;
        while (u == 0)
        {
            u = Value(RandomIndex++); //Converting [0,1) to (0,1)
        }

        while (v == 0)
        {
            v = Value(RandomIndex++);
        }

        var num = MathF.Sqrt(-2.0f * MathF.Log(u)) * MathF.Cos(2.0f * MathF.PI * v);
        num = num / 10f + 0.5f; // Translate to 0 -> 1
        if (num > 1 || num < 0)
        {
            return RandomNormal(); // resample between 0 and 1
        }

        return num;
    }

    public Vector2 RandomVector(int size)
    {
        return new Vector2((RandomNormal() - 0.5f) * size, (RandomNormal() - 0.5f) * size);
    }
}
using MonoGame.Extended.Collections;

namespace Roguecraft.Engine.Procedural.RoomDecorators;

public abstract class ReplacementRule
{
    public abstract char[,] Source { get; }
    public abstract char[,] Target { get; }

    public bool TryApply(Random random, char[,] map)
    {
        foreach (var (x, y) in RandomizeOrder(random, map))
        {
            if (TryApplyAllConfigurations(map, x, y))
            {
                return true;
            }
        }

        return false;
    }

    private char[,] FlipDiagonal(char[,] map)
    {
        var copy = new char[map.GetLength(0), map.GetLength(1)];
        for (var x = 0; x < map.GetLength(0); x++)
        {
            for (var y = 0; y < map.GetLength(1); y++)
            {
                copy[x, y] = map[y, x];
            }
        }
        return copy;
    }

    private char[,] MirrorX(char[,] map)
    {
        var copy = new char[map.GetLength(0), map.GetLength(1)];
        for (var x = 0; x < map.GetLength(0); x++)
        {
            for (var y = 0; y < map.GetLength(1); y++)
            {
                copy[x, y] = map[map.GetLength(0) - (x + 1), y];
            }
        }
        return copy;
    }

    private char[,] MirrorY(char[,] map)
    {
        var copy = new char[map.GetLength(0), map.GetLength(1)];
        for (var x = 0; x < map.GetLength(0); x++)
        {
            for (var y = 0; y < map.GetLength(1); y++)
            {
                copy[x, y] = map[x, map.GetLength(1) - (y + 1)];
            }
        }
        return copy;
    }

    private IEnumerable<(int X, int Y)> RandomizeOrder(Random random, char[,] map)
    {
        var list = new List<(int X, int Y)>();
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                list.Add((i, j));
            }
        }
        return list.Shuffle(random);
    }

    private bool TryApplyAllConfigurations(char[,] map, int i, int j)
    {
        var applied = false;
        for (int bit = 0; bit <= 0b111; bit++)
        {
            var mirrorX = (bit & 0b001) != 0;
            var mirrorY = (bit & 0b010) != 0;
            var flipDiagonal = (bit & 0b100) != 0;
            applied = applied || TryApplyAt(map, i, j, mirrorX, mirrorY, flipDiagonal);
        }
        return applied;
    }

    private bool TryApplyAt(char[,] map, int i, int j, bool mirrorX, bool mirrorY, bool flipDiagonal)
    {
        var source = Source;
        var target = Target;

        if (flipDiagonal)
        {
            source = FlipDiagonal(source);
            target = FlipDiagonal(target);
        }
        if (mirrorX)
        {
            source = MirrorX(source);
            target = MirrorX(target);
        }
        if (mirrorY)
        {
            source = MirrorY(source);
            target = MirrorY(target);
        }
        if (i + Source.GetLength(0) > map.GetLength(0) ||
            j + Source.GetLength(1) > map.GetLength(1))
        {
            return false;
        }
        for (var x = 0; x < source.GetLength(0); x++)
        {
            for (var y = 0; y < source.GetLength(1); y++)
            {
                if (map[i + x, j + y] == source[x, y] || source[x, y] == '*')
                {
                    continue;
                }
                return false;
            }
        }
        for (var x = 0; x < Source.GetLength(0); x++)
        {
            for (var y = 0; y < Source.GetLength(1); y++)
            {
                if (target[x, y] == '*')
                {
                    continue;
                }
                map[i + x, j + y] = target[x, y];
            }
        }
        return true;
    }
}
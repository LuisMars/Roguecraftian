namespace Roguecraft.Engine.Helpers;

public class RandomGenerator
{
    private readonly Random _random = new Random();

    public int FromRange(int min, int max)
    {
        return _random.Next(min, max + 1);
    }
}
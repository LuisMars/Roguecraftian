namespace Roguecraft.Engine.Random;

public static class ListExtensions
{
    public static T Choice<T>(this List<T> list, RandomGenerator random)
    {
        return list[random.Next(list.Count - 1)];
    }

    public static IList<T> Shuffle<T>(this IList<T> list, RandomGenerator random)
    {
        var num = list.Count;
        while (num > 1)
        {
            num--;
            var index = random.Next(num);
            T value = list[index];
            list[index] = list[num];
            list[num] = value;
        }

        return list;
    }
}
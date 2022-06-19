namespace Roguecraft.Engine.Random.Dice;

public class DiceRoller
{
    private readonly RandomGenerator _random;

    public DiceRoller(RandomGenerator random)
    {
        _random = random;
    }

    private int RandomIndex { get; set; }

    public int Roll(DiceRoll roll)
    {
        var result = roll.Bonus;
        for (var i = 0; i < roll.NumberOfDice; i++)
        {
            result += 1 + _random.Range(0, roll.Sides, RandomIndex++);
        }
        return result;
    }
}
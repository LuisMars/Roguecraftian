namespace Roguecraft.Engine.Random;

public class DiceRoll
{
    public DiceRoll(int numberOfDice, int diceSides, int bonus = 0)
    {
        Sides = diceSides;
        NumberOfDice = numberOfDice;
        Bonus = bonus;
    }

    public DiceRoll(string asString)
    {
        var split = asString.ToLower().Split('d');
        NumberOfDice = int.Parse(split[0]);
        if (split[1].IndexOf('+') != -1)
        {
            var secondSplit = split[1].Split('+');
            Sides = int.Parse(secondSplit[0]);
            Bonus = int.Parse(secondSplit[1]);
        }
        else if (split[1].IndexOf('-') != -1)
        {
            var secondSplit = split[1].Split('-');
            Sides = int.Parse(secondSplit[0]);
            Bonus = -int.Parse(secondSplit[1]);
        }
        else
        {
            Sides = int.Parse(split[1]);
        }
    }

    public int Bonus { get; set; }
    public int NumberOfDice { get; set; }
    public int Sides { get; set; }

    public DiceRoll Copy(int extraBonus = 0)
    {
        return new DiceRoll(NumberOfDice, Sides, Bonus + extraBonus);
    }

    public override string ToString()
    {
        return $"{ NumberOfDice }d{ Sides }{ Bonus:+0;-#;#}";
    }
}
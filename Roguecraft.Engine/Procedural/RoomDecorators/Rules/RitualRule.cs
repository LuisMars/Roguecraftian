namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class RitualRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { '*', 'W', '*', '*', '*', '*' , '*' },
        { '*', 'F', '*', '*', '*', '*' , '*' },
        { '*', 'F', 'F', 'F', 'F', '*' , '*' },
        { '*', 'F', 'F', 'F', 'F', '*' , '*' },
        { '*', 'F', 'F', 'F', 'F', '*' , '*' },
        { '*', 'F', 'F', 'F', 'F', '*' , '*' },
        { '*', '*', '*', '*', '*', '*' , '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { '*', '*', '*', '*', '*', '*', '*' },
        { '*', '*', '*', '*', '*', '*', '*' },
        { '*', 'R', 'R', 'R', 'R', '*', '*' },
        { '*', 'R', 'R', 'R', 'R', '*', '*' },
        { '*', 'R', 'R', 'R', 'R', '*', '*' },
        { '*', 'R', 'R', 'R', 'R', '*', '*' },
        { '*', '*', '*', '*', '*', '*', '*' },
    };
}
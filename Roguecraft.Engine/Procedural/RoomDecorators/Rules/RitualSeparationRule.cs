namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class RitualSeparationRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { '*', 'W', 'W', 'W', 'W', '*' },
        { '*', 'R', 'R', 'R', 'R', '*' },
        { '*', 'R', 'R', 'R', 'R', '*' },
        { '*', 'R', 'R', 'R', 'R', '*' },
        { '*', 'R', 'R', 'R', 'R', '*' },
        { '*', 'F', 'F', 'F', 'F', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { '*', '*', '*', '*', '*', '*' },
        { '*', '*', '*', '*', '*', '*' },
        { '*', 'R', 'R', 'R', 'R', '*' },
        { '*', 'R', 'R', 'R', 'R', '*' },
        { '*', 'R', 'R', 'R', 'R', '*' },
        { '*', 'R', 'R', 'R', 'R', '*' },
    };
}
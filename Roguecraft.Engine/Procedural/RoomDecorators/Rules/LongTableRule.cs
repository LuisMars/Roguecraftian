namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class LongTableRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { '*', 'F', 'C', 'C', 'F', '*' },
        { '*', 'C', 'T', 'T', 'C', '*' },
        { '*', 'C', 'T', 'T', 'C', '*' },
        { '*', 'F', 'F', 'F', 'F', '*' },
        { '*', 'F', 'C', 'C', 'F', '*' },
        { '*', 'C', 'T', 'T', 'C', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { '*', 'F', 'C', 'C', 'F', '*' },
        { '*', 'C', 'T', 'T', 'C', '*' },
        { '*', 'C', 'T', 'T', 'C', '*' },
        { '*', 'C', 'T', 'T', 'C', '*' },
        { '*', 'C', 'T', 'T', 'C', '*' },
        { '*', 'C', 'T', 'T', 'C', '*' },
    };
}
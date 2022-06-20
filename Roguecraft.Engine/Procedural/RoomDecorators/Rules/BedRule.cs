namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class BedRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W', 'W' },
        { 'F', 'F', 'F', 'F' },
        { '*', 'F', 'F', '*' },
        { '*', '*', '*', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W', 'W' },
        { '*', 'Z', 'Z', '*' },
        { '*', 'F', 'F', '*' },
        { '*', '*', '*', '*' },
    };
}
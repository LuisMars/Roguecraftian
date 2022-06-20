namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class ArmorStandRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'F', 'F', 'F' },
        { 'F', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { '*', 'A', '*' },
        { 'F', 'F', 'F' },
    };
}
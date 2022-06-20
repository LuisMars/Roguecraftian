namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class BarrelRowReductionRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'J', 'F', 'J' },
        { 'J', 'F', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'J', 'J', 'J' },
        { 'F', 'F', '*' },
    };
}
namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class BarrelGrowRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W', '*' },
        { '*', 'J', '*', '*' },
        { 'F', 'F', 'F', '*' },
        { 'F', 'F', 'F', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W', '*' },
        { '*', 'J', '*', '*' },
        { 'F', 'J', 'F', '*' },
        { 'F', 'F', 'F', '*' },
    };
}
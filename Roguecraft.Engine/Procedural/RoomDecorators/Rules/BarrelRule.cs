namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class BarrelRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { '*', 'F', '*' },
        { 'F', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { '*', 'J', '*' },
        { 'F', 'F', 'F' },
    };
}
namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class ChestRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', '*' },
        { 'W', 'F', 'F' },
        { 'W', 'W', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', '*' },
        { 'W', '$', 'F' },
        { 'W', 'W', '*' },
    };
}
namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class RoomShapeRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'W', 'F', 'F' },
        { 'W', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'W', 'W', 'F' },
        { 'W', 'F', 'F' },
    };
}
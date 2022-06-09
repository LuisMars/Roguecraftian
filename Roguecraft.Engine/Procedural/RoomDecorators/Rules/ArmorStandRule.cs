namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class ArmorStandRule : ReplacementRule
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'B', 'B', 'B' },
        { 'F', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'B', 'A', 'B' },
        { 'F', 'F', 'F' },
    };
}
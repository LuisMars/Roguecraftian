namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class StairsRule : ReplacementRule
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W' },
        { 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W' },
        { 'S', 'W' },
    };
}
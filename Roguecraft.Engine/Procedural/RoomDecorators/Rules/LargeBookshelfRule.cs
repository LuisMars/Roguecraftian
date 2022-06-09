namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class LargeBookshelfRule : ReplacementRule
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'B', 'F', 'B' },
        { 'F', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'B', 'B', 'F' },
        { 'F', 'F', 'F' },
    };
}
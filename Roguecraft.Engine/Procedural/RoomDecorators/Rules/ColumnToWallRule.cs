namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class ColumnToWallRule : ReplacementRule
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W', 'W' },
        { 'F', 'W', 'F', 'F' },
        { 'F', 'F', 'F', 'F' },
        { 'F', 'F', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W', 'W' },
        { 'F', 'W', 'F', 'F' },
        { 'F', 'W', 'F', 'F' },
        { 'F', 'F', 'F', 'F' },
    };
}
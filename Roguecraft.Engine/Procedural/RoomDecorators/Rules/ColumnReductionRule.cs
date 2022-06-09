namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class ColumnReductionRule : ReplacementRule
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'W', 'F', 'W' },
        { 'F', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'F', 'W', 'F' },
        { 'F', 'F', 'F' },
    };
}
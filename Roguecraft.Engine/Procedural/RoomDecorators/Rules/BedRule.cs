namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class BedRule : ReplacementRule
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W', 'W' },
        { 'W', 'F', 'F', 'F' },
        { 'W', 'F', 'F', 'F' },
        { 'W', '*', '*', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W', 'W' },
        { 'W', 'Z', 'Z', 'F' },
        { 'W', 'F', 'F', 'F' },
        { 'W', '*', '*', '*' },
    };
}
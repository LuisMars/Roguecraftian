namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class DeskRule : ReplacementRule
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'Z', 'F', 'F' },
        { '*', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'Z', 'T', 'F' },
        { '*', 'F', 'F' },
    };
}
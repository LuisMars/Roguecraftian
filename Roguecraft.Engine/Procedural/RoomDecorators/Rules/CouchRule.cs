namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class CouchRule : ReplacementRule
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W', 'W' },
        { 'F', 'F', 'F', 'F' },
        { '*', 'F', 'F', '*' },
        { '*', '*', '*', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W', 'W' },
        { '*', 'c', 'c', '*' },
        { '*', 'F', 'F', '*' },
        { '*', '*', '*', '*' },
    };
}
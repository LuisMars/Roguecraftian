namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class RitualRule : ReplacementRule
{
    public override char[,] Source { get; } = new char[,] {
        { 'F', 'F', 'F', 'F', 'F', 'F' },
        { 'F', 'F', 'F', 'F', 'F', 'F' },
        { 'F', 'F', 'F', 'F', 'F', 'F' },
        { 'F', 'F', 'F', 'F', 'F', 'F' },
        { 'F', 'F', 'F', 'F', 'F', 'F' },
        { 'F', 'F', 'F', 'F', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'F', 'F', 'F', 'F', 'F', 'F' },
        { 'F', 'R', 'R', 'R', 'R', 'F' },
        { 'F', 'R', 'R', 'R', 'R', 'F' },
        { 'F', 'R', 'R', 'R', 'R', 'F' },
        { 'F', 'R', 'R', 'R', 'R', 'F' },
        { 'F', 'F', 'F', 'F', 'F', 'F' },
    };
}
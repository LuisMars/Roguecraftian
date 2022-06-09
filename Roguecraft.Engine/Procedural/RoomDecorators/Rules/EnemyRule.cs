namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class EnemyRule : ReplacementRule
{
    public override char[,] Source { get; } = new char[,] {
        { 'F', 'F', 'F' },
        { 'F', 'F', 'F' },
        { 'F', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'F', 'F', 'F' },
        { 'F', 'E', 'F' },
        { 'F', 'F', 'F' },
    };
}
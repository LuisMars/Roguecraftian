namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class ReduceEnemiesRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'F', 'F', 'F' },
        { 'E', 'F', 'E' },
        { 'F', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'F', 'F', 'F' },
        { 'F', 'E', 'F' },
        { 'F', 'F', 'F' },
    };
}
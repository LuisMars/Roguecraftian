namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class EnemyPewRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'F', 'F' },
        { 'P', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'F', 'F' },
        { 'ꝓ', 'E' },
    };
}
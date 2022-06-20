namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class ReduceChestRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { '$', 'W', '$' },
        { '*', '*', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { '$', 'W', 'W' },
        { '*', '*', '*' },
    };
}
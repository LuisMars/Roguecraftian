namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class TorchRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', '*' },
        { 'W', 'F', '*' },
        { '*', '*', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', '*' },
        { 'W', 't', '*' },
        { '*', '*', '*' },
    };
}
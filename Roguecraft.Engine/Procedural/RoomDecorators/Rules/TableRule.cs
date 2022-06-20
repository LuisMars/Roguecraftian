namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class TableRule : ReplacementRuleBase
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
        { 'F', 'F', 'F', 'F', 'F', 'F' },
        { 'F', 'F', 'T', 'T', 'F', 'F' },
        { 'F', 'F', 'T', 'T', 'F', 'F' },
        { 'F', 'F', 'F', 'F', 'F', 'F' },
        { 'F', 'F', 'F', 'F', 'F', 'F' },
    };
}
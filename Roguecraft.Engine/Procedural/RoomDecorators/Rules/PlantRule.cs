namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class PlantRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { 'F', 'F', 'F' },
        { 'F', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W' },
        { '*', 'p', '*' },
        { 'F', 'F', 'F' },
    };
}
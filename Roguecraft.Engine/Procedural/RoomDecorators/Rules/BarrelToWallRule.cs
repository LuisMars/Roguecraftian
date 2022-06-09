namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class BarrelToWallRule : ReplacementRule
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', 'W', 'W' },
        { 'F', 'J', 'F', 'F' },
        { 'F', 'F', 'F', 'F' },
        { 'F', 'F', 'F', 'F' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'W', 'W', 'W', 'W' },
        { 'F', 'J', 'J', 'F' },
        { 'F', 'F', 'F', 'F' },
        { 'F', 'F', 'F', 'F' },
    };
}
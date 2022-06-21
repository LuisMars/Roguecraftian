namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class PodiumRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'W', 'W', '*', '*', '*', '*' },
        { 'W', 'F', 'F', 'F', 'F', '*' },
        { '*', 'F', 'F', 'F', 'F', '*' },
        { '*', 'F', 'F', 'F', 'F', '*' },
        { '*', 'F', 'F', 'F', 'F', '*' },
        { '*', 'F', 'F', 'F', 'F', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { '*', '*', '*', '*', '*', '*' },
        { '*', 'F', 'F', 'F', 'F', '*' },
        { '*', 'F', 'P', 'P', 'F', '*' },
        { '*', 'F', 'ꝓ', 'ꝓ', 'F', '*' },
        { '*', 'F', 'ꝓ', 'ꝓ', 'F', '*' },
        { '*', 'F', 'ꝓ', 'ꝓ', 'F', '*' },
    };
}
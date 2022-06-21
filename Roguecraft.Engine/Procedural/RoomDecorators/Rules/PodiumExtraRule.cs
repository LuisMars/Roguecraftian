namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class PodiumExtraRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { '*', '*', '*', '*', '*', '*' },
        { 'F', 'F', 'F', 'F', 'F', '*' },
        { 'P', 'P', 'F', 'F', 'F', '*' },
        { 'ꝓ', 'ꝓ', 'F', 'F', 'F', '*' },
        { 'ꝓ', 'ꝓ', 'F', 'F', 'F', '*' },
        { 'ꝓ', 'ꝓ', 'F', 'F', 'F', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { '*', '*', '*', '*', '*',  '*' },
        { 'F', 'F', 'F', 'F', 'F',  '*' },
        { 'P', 'P', 'F', 'P', 'P',  '*' },
        { 'ꝓ', 'ꝓ', 'F', 'ꝓ', 'ꝓ',  '*' },
        { 'ꝓ', 'ꝓ', 'F', 'ꝓ', 'ꝓ',  '*' },
        { 'ꝓ', 'ꝓ', 'F', 'ꝓ', 'ꝓ',  '*' },
    };
}
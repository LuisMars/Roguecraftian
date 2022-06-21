namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class PewRule : ReplacementRuleBase
{
    public override char[,] Source { get; } = new char[,] {
        { 'F', 'ꝓ', 'ꝓ', 'F', '*' },
        { 'F', 'ꝓ', 'ꝓ', 'F', '*' },
        { 'F', 'ꝓ', 'ꝓ', 'F', '*' },
        { 'F', 'F', 'F', 'F', '*' },
        { 'F', 'F', 'F', 'F', '*' },
    };

    public override char[,] Target { get; } = new char[,] {
        { 'F', 'ꝓ', 'ꝓ', 'F', '*'  },
        { 'F', 'ꝓ', 'ꝓ', 'F', '*'  },
        { 'F', 'ꝓ', 'ꝓ', 'F', '*'  },
        { 'F', 'ꝓ', 'ꝓ', 'F', '*'  },
        { 'F', 'F', 'F', 'F', '*'  },
    };
}
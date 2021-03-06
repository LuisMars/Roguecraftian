namespace Roguecraft.Engine.Procedural.RoomDecorators.Rules;

public class StartRule : ReplacementRuleBase
{
    public StartRule()
    {
        MaxOccurences = 1;
    }

    public override char[,] Source { get; } = new char[,] {
        { 'F' }
    };

    public override char[,] Target { get; } = new char[,] {
        { 'S' }
    };
}
using Roguecraft.Engine.Actions;

namespace Roguecraft.Engine.Components;

public class Stats
{
    public Attack DefaultAttack { get; set; }
    public int MaxHealth { get; set; } = 1;
    public int Speed { get; set; } = 1;
}
using Roguecraft.Engine.Actions;

namespace Roguecraft.Engine.Components;

public class Stats
{
    public AttackAction DefaultAttack { get; set; }
    public int MaxHealth { get; set; } = 1;
    public float Speed { get; set; } = 1;
}
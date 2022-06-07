using Roguecraft.Engine.Actions.Combat;

namespace Roguecraft.Engine.Components;

public class Stats
{
    public int MaxHealth { get; set; } = 1;
    public float Speed { get; set; } = 1;
    public AttackAction UnarmedAttack { get; set; }
}
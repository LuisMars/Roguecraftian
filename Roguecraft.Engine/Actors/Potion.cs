using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Actors;

public class Potion : Item
{
    protected override void OnPickUp(Creature creature)
    {
    }

    protected override void OnQuickAction(Creature creature)
    {
        creature.Health = Math.Min(creature.Stats.MaxHealth, creature.Health + 2);
        creature.Timers[TimerType.Heal].Reset();
    }
}
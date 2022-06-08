using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Actions.Effects;

public class HealAction : GameAction
{
    public HealAction() : this(null)
    {
    }

    public HealAction(Creature creature) : base(creature)
    {
        EngeryCost = 500;
    }

    protected override void OnPerform(float deltaTime)
    {
        Creature.Health = Math.Min(Creature.Stats.MaxHealth, Creature.Health + 2);
        Creature.Timers[TimerType.Heal].Reset();
    }
}
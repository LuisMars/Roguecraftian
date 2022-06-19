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
        RestoredHealth = 2;
    }

    public int RestoredHealth { get; }

    public override bool TryPrepare(bool useMouse)
    {
        return Creature.Energy == 0;
    }

    protected override void OnPerform(float deltaTime)
    {
        Creature.Health = Math.Min(Creature.Stats.MaxHealth, Creature.Health + RestoredHealth);
        Creature.Timers[TimerType.Heal].Reset();
    }
}
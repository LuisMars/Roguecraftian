using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.Combat;

public abstract class AttackAction : GameAction
{
    protected AttackAction(Creature actor) : base(actor)
    {
        EngeryCost = 1000;
    }

    public int MaxDamage { get; init; } = 1;
    public int MinDamage { get; init; } = 0;
    public Creature Target { get; private set; }

    public override bool TryPrepare()
    {
        if (Creature.Energy < 0)
        {
            return false;
        }
        var creatureEvent = Creature.AreaOfInfluence.FirstOrDefault<Creature>(x => !x.Other.IsSensor && x.Other.Actor != Creature);
        if (creatureEvent is null)
        {
            return false;
        }
        var creature = (Creature)creatureEvent.Other.Actor;
        BindTarget(creature);
        return true;
    }

    internal void BindTarget(Creature target)
    {
        Target = target;
    }

    protected abstract void OnAttack(float deltaTime);

    protected override void OnPerform(float deltaTime)
    {
        OnAttack(deltaTime);
        Target.HurtTimer.Reset(0.125f);
    }
}
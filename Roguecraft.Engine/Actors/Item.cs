using Roguecraft.Engine.Actions.Combat;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Actors;

public abstract class Item : Actor
{
    public void DefaultAction(Creature creature)
    {
        IsPickedUp = true;
        OnDefaultAction(creature);
    }

    public AttackAction GetAttack()
    {
        return OnGetAttack();
    }

    public void PickUp(Creature creature)
    {
        IsPickedUp = true;
        creature.Timers[TimerType.Pickup].Reset();
        creature.Inventory.Add(this);
        OnPickUp(creature);
    }

    protected virtual void OnDefaultAction(Creature creature)
    {
    }

    protected virtual AttackAction OnGetAttack()
    {
        return null;
    }

    protected virtual void OnPickUp(Creature creature)
    {
    }
}
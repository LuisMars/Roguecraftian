using Roguecraft.Engine.Actions.Combat;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Actors;

public abstract class Item : Actor
{
    public AttackAction GetAttack()
    {
        return OnGetAttack();
    }

    public void PickUp(Creature creature)
    {
        IsPickedUp = true;
        creature.Timers[TimerType.Pickup].Reset();
        OnPickUp(creature);
    }

    public void QuickAction(Creature creature)
    {
        IsPickedUp = true;
        creature.Timers[TimerType.Pickup].Reset();
        OnQuickAction(creature);
    }

    protected virtual AttackAction OnGetAttack()
    {
        return null;
    }

    protected abstract void OnPickUp(Creature creature);

    protected abstract void OnQuickAction(Creature creature);
}
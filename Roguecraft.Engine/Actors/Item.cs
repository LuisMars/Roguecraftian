using Roguecraft.Engine.Actions.Combat;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Random;
using Roguecraft.Engine.Simulation;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Actors;

public abstract class Item : Actor
{
    public void DefaultAction(Creature creature)
    {
        IsPickedUp = true;
        OnDefaultAction(creature);
    }

    public void Drop(Creature creture, RandomGenerator randomGenerator, CollisionService collisionService, ActorPool actorPool)
    {
        IsPickedUp = false;
        Position = creture.Position + randomGenerator.RandomVector((int)creture.Collision.Width);
        actorPool.AddLater(this);
        Collision.Update();
        collisionService.Insert(Collision);
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

    public virtual bool TryPrepare(Creature creature)
    {
        return true;
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
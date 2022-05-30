using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Timers;

namespace Roguecraft.Engine.Actors;

public abstract class Creature : Actor
{
    public Collision AreaOfInfluence { get; set; }
    public float Energy { get; set; }
    public int Health { get; set; }
    public ActionTimer HurtTimer { get; } = new();
    public Stats Stats { get; set; }
    public ToggleDoorAction ToggleDoorAction { get; set; }
    public WalkAction WalkAction { get; set; }

    public override void ClearSimulationData()
    {
        base.ClearSimulationData();
        AreaOfInfluence.Clear();
    }

    public abstract GameAction? OnTakeTurn(float deltaTime);

    public override GameAction? TakeTurn(float deltaTime)
    {
        UpdateTimers(deltaTime);
        Energy += Stats.Speed * deltaTime;
        Energy = Math.Min(0, Energy);
        return OnTakeTurn(deltaTime);
    }

    public override void UpdateSimulationData()
    {
        base.UpdateSimulationData();
        AreaOfInfluence.Update();
    }

    protected abstract bool CanAttack();

    protected abstract bool CanOpenDoor();

    protected abstract bool CanWalk(out Vector2 direction);

    protected bool TryAttack(out AttackAction attack)
    {
        attack = null;

        if (Energy < 0 || !CanAttack())
        {
            return false;
        }
        var creatureEvent = AreaOfInfluence.LastEvents.FirstOrDefault(x => !x.Other.IsSensor && x.Other.Actor is Creature creature && creature != this);
        if (creatureEvent is null)
        {
            return false;
        }
        var creature = (Creature)creatureEvent.Other.Actor;
        Stats.DefaultAttack.BindTarget(creature);
        attack = Stats.DefaultAttack;
        return true;
    }

    protected bool TryOpenDoor()
    {
        if (Energy < 0 || !CanOpenDoor())
        {
            return false;
        }

        var door = AreaOfInfluence.LastEvents.FirstOrDefault(e => e.Other.Actor is Door);
        var bodyDoor = Collision.LastEvents.FirstOrDefault(e => e.Other.Actor is Door);
        if (door is null || bodyDoor is not null)
        {
            return false;
        }
        ToggleDoorAction.BindTarget((Door)door.Other.Actor);
        return true;
    }

    protected bool TryWalk()
    {
        if (!CanWalk(out var direction))
        {
            WalkAction.Set(direction);
            return false;
        }

        WalkAction.Set(direction);

        return true;
    }

    private void UpdateTimers(float deltaTime)
    {
        HurtTimer?.Update(deltaTime);
    }
}
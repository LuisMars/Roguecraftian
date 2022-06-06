using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actions;

namespace Roguecraft.Engine.Actors;

public class Enemy : Creature
{
    public Hero Hero { get; set; }

    public override GameAction? OnTakeTurn(float deltaTime)
    {
        if (TryAttack(out var attack))
        {
            return attack;
        }
        if (TryFollowHero())
        {
            return OnFollowHero();
        }
        if (TryOpenDoor())
        {
            return ToggleDoorAction;
        }
        if (TryWalk())
        {
            return WalkAction;
        }

        return NullAction;
    }

    protected override bool CanAttack()
    {
        return AreaOfInfluence.InternalEvents.Any(x => x.Other.Actor == Hero);
    }

    protected override bool CanOpenDoor()
    {
        return true;
    }

    protected override bool CanWalk(out Vector2 direction)
    {
        direction = Vector2.Zero;
        return true;
    }

    private GameAction OnFollowHero()
    {
        WalkAction.Set(Hero.Position - Position);
        return WalkAction;
    }

    private bool TryFollowHero()
    {
        return Visibility.IsVisibleByPlayer;
    }
}
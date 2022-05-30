using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actions;

namespace Roguecraft.Engine.Actors;

public class Enemy : Creature
{
    public override GameAction? OnTakeTurn(float deltaTime)
    {
        Energy += Stats.Speed * deltaTime;
        Energy = Math.Min(0, Energy);
        if (TryAttack(out var attack))
        {
            return attack;
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
        return true;
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
}
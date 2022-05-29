using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using Roguecraft.Engine.Actions;

namespace Roguecraft.Engine.Actors;

public class Hero : Creature
{
    public override GameAction? OnTakeTurn(float deltaTime)
    {
        var state = KeyboardExtended.GetState();
        if (TryOpenDoor(state))
        {
            return ToggleDoorAction;
        }
        if (TryWalk(state))
        {
            return WalkAction;
        }
        return WalkAction;
    }

    private bool TryOpenDoor(KeyboardStateExtended state)
    {
        if (Energy < 0)
        {
            return false;
        }
        if (!state.WasKeyJustDown(Keys.Space))
        {
            return false;
        }
        var door = AreaOfInfluence.LastEvents.FirstOrDefault(e => e.Other.Actor is Door);
        if (door is null)
        {
            return false;
        }
        ToggleDoorAction.BindTarget((Door)door.Other.Actor);
        return true;
    }

    private bool TryWalk(KeyboardStateExtended state)
    {
        var direction = new Vector2();
        var moved = false;
        if (state.IsKeyDown(Keys.W))
        {
            moved = true;
            direction += new Vector2(0, -1);
        }
        if (state.IsKeyDown(Keys.S))
        {
            moved = true;
            direction += new Vector2(0, 1);
        }
        if (state.IsKeyDown(Keys.A))
        {
            moved = true;
            direction += new Vector2(-1, 0);
        }
        if (state.IsKeyDown(Keys.D))
        {
            moved = true;
            direction += new Vector2(1, 0);
        }

        WalkAction.Set(direction);

        return moved;
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using Roguecraft.Engine.Actions;

namespace Roguecraft.Engine.Actors;

public class Hero : Creature
{
    private KeyboardStateExtended KeyBoardState { get; set; }

    public override GameAction? OnTakeTurn(float deltaTime)
    {
        KeyBoardState = KeyboardExtended.GetState();
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
        return WalkAction;
    }

    protected override bool CanAttack()
    {
        return KeyBoardState.WasKeyJustDown(Keys.Space);
    }

    protected override bool CanOpenDoor()
    {
        return KeyBoardState.WasKeyJustDown(Keys.Space);
    }

    protected override bool CanWalk(out Vector2 direction)
    {
        direction = new Vector2();
        var moved = false;
        if (KeyBoardState.IsKeyDown(Keys.W))
        {
            moved = true;
            direction += new Vector2(0, -1);
        }
        if (KeyBoardState.IsKeyDown(Keys.S))
        {
            moved = true;
            direction += new Vector2(0, 1);
        }
        if (KeyBoardState.IsKeyDown(Keys.A))
        {
            moved = true;
            direction += new Vector2(-1, 0);
        }
        if (KeyBoardState.IsKeyDown(Keys.D))
        {
            moved = true;
            direction += new Vector2(1, 0);
        }
        return moved;
    }
}
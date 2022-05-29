using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Roguecraft.Engine.Actions;

namespace Roguecraft.Engine.Actors;

public class Hero : Creature
{
    public override GameAction? TakeTurn(float deltaTime)
    {
        var state = Keyboard.GetState();
        if (TryWalk(state))
        {
            return WalkAction;
        }
        return WalkAction;
    }

    private bool TryWalk(KeyboardState state)
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
using MonoGame.Extended.Input;
using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Input;

namespace Roguecraft.Engine.Actors;

public class Hero : Creature
{
    public InputManager InputManager { get; set; }

    public override GameAction? OnTakeTurn(float deltaTime)
    {
        return AvailableActions.GetNextAction(InputManager.State);
    }
}
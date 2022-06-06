using MonoGame.Extended.Input;
using Roguecraft.Engine.Actions;

namespace Roguecraft.Engine.Actors;

public class Hero : Creature
{
    private KeyboardStateExtended KeyBoardState { get; set; }

    public override GameAction? OnTakeTurn(float deltaTime)
    {
        KeyBoardState = KeyboardExtended.GetState();
        return AvailableActions.GetNextAction(KeyBoardState);
    }
}
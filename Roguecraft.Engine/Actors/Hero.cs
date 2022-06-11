using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Input;
using Roguecraft.Engine.Visibility;

namespace Roguecraft.Engine.Actors;

public class Hero : Creature
{
    public InputManager InputManager { get; set; }

    public override void CalculateVisibility(VisibilityService visibilityService)
    {
        Visibility.IsVisibleByHero = true;
    }

    public override GameAction? OnTakeTurn(float deltaTime)
    {
        return AvailableActions.GetNextAction(InputManager.State);
    }
}
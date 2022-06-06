using Roguecraft.Engine.Actions;

namespace Roguecraft.Engine.Actors;

public class Enemy : Creature
{
    public Hero Hero { get; set; }

    public override GameAction? OnTakeTurn(float deltaTime)
    {
        return AvailableActions.GetNextAction(this);
    }
}
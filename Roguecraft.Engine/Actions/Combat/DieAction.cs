using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.Combat;

public class DieAction : GameAction
{
    public DieAction(Creature creature) : base(creature)
    {
    }

    protected override void OnPerform(float deltaTime)
    {
        Creature.Die();
    }
}
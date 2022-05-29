using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions;

public class ToggleDoorAction : GameAction
{
    public ToggleDoorAction(Creature creature) : base(creature)
    {
    }

    public Door Target { get; private set; }

    internal void BindTarget(Door target)
    {
        Target = target;
    }

    protected override void OnPerform(float deltaTime)
    {
        Target.Toggle();
    }
}
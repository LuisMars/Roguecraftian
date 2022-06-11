using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Input;

namespace Roguecraft.Engine.Actions.Interaction;

public class ToggleDoorAction : GameAction
{
    public ToggleDoorAction(Creature creature) : this(creature, null)
    {
    }

    public ToggleDoorAction(Creature creature, InputManager inputManager) : base(creature, inputManager)
    {
        EngeryCost = 250;
    }

    public Door Target { get; private set; }

    public override bool TryPrepare(bool useMouse)
    {
        if (Creature.Energy < 0)
        {
            return false;
        }
        var door = GetSelected<Door>(useMouse, x => !x.Other.Actor.Collision.InternalEvents.Any(e => !e.Other.IsSensor && e.PenetrationVector.LengthSquared() > 100));
        if (door is null)
        {
            return false;
        }
        BindTarget((Door)door.Other.Actor);
        return true;
    }

    internal void BindTarget(Door target)
    {
        Target = target;
    }

    protected override void OnPerform(float deltaTime)
    {
        Target?.Toggle();
    }
}
using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.Interaction;

public class ToggleDoorAction : GameAction
{
    public ToggleDoorAction(Creature creature) : base(creature)
    {
        EngeryCost = 250;
    }

    public Door Target { get; private set; }

    public override bool TryPrepare()
    {
        if (Creature.Energy < 0)
        {
            return false;
        }

        var door = Creature.AreaOfInfluence.FirstOrDefault<Door>();
        if (door is null)
        {
            return false;
        }
        var isInsideDoor = Creature.Collision.FirstOrDefault<Door>(e => e.PenetrationVector.LengthSquared() > 100);
        if (isInsideDoor is not null)
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
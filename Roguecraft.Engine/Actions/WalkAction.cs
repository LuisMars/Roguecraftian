using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions;

public class WalkAction : GameAction
{
    public WalkAction(Creature actor) : base(actor)
    {
    }

    private Vector2 Direction { get; set; }

    public void Set(Vector2 direction)
    {
        if (direction.LengthSquared() != 0)
        {
            direction.Normalize();
        }

        Direction = Vector2.Lerp(Direction, direction, 0.09f);
        if (Direction.LengthSquared() != 0)
        {
            Direction.Normalize();
        }
    }

    protected override void OnPerform(float deltaTime)
    {
        Creature.Position += Direction * Creature.Speed * deltaTime;
        Creature.Angle = Direction.ToAngle();
    }
}
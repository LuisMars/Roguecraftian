using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions;

public class WalkAction : GameAction
{
    public WalkAction(Creature actor) : base(actor)
    {
        EngeryCost = 0;
    }

    public Vector2 CurrentPosition { get; set; }
    public Vector2 Direction { get; private set; }
    public Vector2 LastPosition { get; set; }

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
        var speed = Direction * Creature.Stats.Speed * deltaTime;
        Creature.Position += speed;
        CurrentPosition = Creature.Position;

        Creature.DistanceWalked += speed.Length();
        Creature.Angle = Direction.ToAngle();
    }
}
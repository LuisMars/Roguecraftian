using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Actions.Movement;

public class MoveAction : GameAction
{
    public MoveAction(Creature actor) : base(actor)
    {
        EngeryCost = 0;
    }

    public Vector2 Direction { get; protected set; }

    public virtual Vector2 GetDirection()
    {
        var direction = Vector2.Lerp(Direction, Creature.Direction, 0.85f);
        return direction;
    }

    protected override void OnPerform(float deltaTime)
    {
        var direction = GetDirection();
        Creature.Direction = direction;
        var speed = direction.NormalizedCopy() * Creature.Stats.Speed * deltaTime;
        Creature.Position += speed;

        Creature.DistanceWalked += speed.Length();
        Creature.Angle = direction.ToAngle();
    }
}
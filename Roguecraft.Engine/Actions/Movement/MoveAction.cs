using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Actions.Movement;

public class MoveAction : GameAction
{
    public MoveAction(Creature creature) : base(creature)
    {
        EngeryCost = 0;
        IgnoreOnMouse = true;
    }

    public Vector2 Direction { get; protected set; }
    protected bool SmoothDirection { get; set; }

    public virtual Vector2 GetDirection()
    {
        var direction = Vector2.Lerp(Direction, Creature.Direction, 0.85f);
        direction = direction.ClampMagnitude(1);
        return direction;
    }

    protected override void OnPerform(float deltaTime)
    {
        var direction = GetDirection();
        if (SmoothDirection)
        {
            direction = Vector2.Lerp(Creature.Direction, direction, deltaTime);
        }
        Creature.Direction = direction;

        var speed = direction * Creature.Stats.Speed * deltaTime;
        Creature.RealSpeed = speed;
        Creature.Position += speed;
        Creature.IsStill = false;
        Creature.StillFrames = 0;

        Creature.DistanceWalked += speed.Length();
        Creature.Angle = direction.ToAngle();
    }
}
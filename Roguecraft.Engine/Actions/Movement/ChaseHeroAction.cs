using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Actions.Movement;

public class ChaseHeroAction : MoveAction
{
    public ChaseHeroAction(Creature actor, Hero hero) : base(actor)
    {
        Hero = hero;
        SmoothDirection = true;
    }

    private float GreatestDistance { get; set; }
    private Hero Hero { get; }
    private bool IsTracking { get; set; }
    private Vector2 LastKnownPosition { get; set; }

    public override Vector2 GetDirection()
    {
        var direction = LastKnownPosition - Creature.Position;
        foreach (var collision in Creature.AreaOfInfluence.InternalEvents)
        {
            if (collision.Other.IsSensor || collision.Other.Actor == Creature)
            {
                continue;
            }
            if (collision.Other.Bounds is CircleF)
            {
                direction += Creature.Position - (Vector2)collision.Other.Bounds.Position;
            }
            if (collision.Other.Bounds is RectangleF rectangle)
            {
                direction += Creature.Position - (Vector2)rectangle.Center;
            }
        }

        direction /= Creature.Stats.Speed * 2;
        direction = direction.ClampMagnitude(1);

        return direction;
    }

    public override bool TryPrepare(bool useMouse)
    {
        if (Creature.AreaOfInfluence.Any<Hero>(x => !x.Other.IsSensor))
        {
            return false;
        }
        if (Creature.Visibility.IsVisibleByHero)
        {
            LastKnownPosition = Hero.Position;
            var distance = (LastKnownPosition - Creature.Position).LengthSquared();

            GreatestDistance = Math.Max(GreatestDistance, distance);
            return true;
        }
        var distanceSquared = (LastKnownPosition - Creature.Position).LengthSquared();

        IsTracking = distanceSquared > 100 * 100 && distanceSquared < GreatestDistance;
        return IsTracking;
    }
}
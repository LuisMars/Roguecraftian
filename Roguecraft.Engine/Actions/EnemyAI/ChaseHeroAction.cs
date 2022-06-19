using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actions.Movement;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Actions.EnemyAI;

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
            direction += collision.Other.Bounds switch
            {
                CircleF circle => Creature.Position - (Vector2)circle.Position,
                RectangleF rectangle => Creature.Position - (Vector2)rectangle.Center,
                _ => Vector2.Zero
            };
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
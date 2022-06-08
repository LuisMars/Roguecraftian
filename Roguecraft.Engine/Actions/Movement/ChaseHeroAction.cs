using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Actions.Movement;

public class ChaseHeroAction : MoveAction
{
    public ChaseHeroAction(Creature actor, Hero hero) : base(actor)
    {
        Hero = hero;
    }

    public Hero Hero { get; }
    public Vector2 LastKnownPosition { get; set; }

    public override Vector2 GetDirection()
    {
        var direction = LastKnownPosition - Creature.Position;
        direction /= Creature.Stats.Speed * 2;
        direction = direction.ClampMagnitude(1, out _);
        if (Creature.AreaOfInfluence.Any<Hero>())
        {
            direction = direction * 0.125f + new Vector2(direction.Y, -direction.X) * 0.875f;
        }
        return direction;
    }

    public override bool TryPrepare()
    {
        if (Creature.Visibility.IsVisibleByHero)
        {
            LastKnownPosition = Hero.Position;
            return true;
        }
        var distanceSquared = (LastKnownPosition - Creature.Position).LengthSquared();
        return distanceSquared > 100 * 100;
    }
}
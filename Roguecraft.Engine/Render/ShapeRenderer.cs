using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Core;

namespace Roguecraft.Engine.Render;

public class ShapeRenderer
{
    private readonly ActorPool _actorPool;

    public ShapeRenderer(ActorPool actorPool)
    {
        _actorPool = actorPool;
    }

    public void Render(SpriteBatch spriteBatch)
    {
        foreach (var actor in _actorPool.Actors)
        {
            var shape = actor.Collision?.Bounds ?? null;
            DrawShape(spriteBatch, actor, shape);

            if (actor is Creature creature)
            {
                var aoi = creature.AreaOfInfluence?.Bounds ?? null;

                DrawShape(spriteBatch, actor, aoi);
            }
        }
    }

    private static void DrawShape(SpriteBatch spriteBatch, Actor actor, IShapeF? shape)
    {
        if (shape is null)
        {
            return;
        }
        shape.Position = actor.Position;

        if (shape is CircleF circle)
        {
            spriteBatch.DrawCircle(circle, 10, Color.White);
        }
        if (shape is RectangleF rectangle)
        {
            spriteBatch.DrawRectangle(rectangle, Color.White);
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Visibility;

namespace Roguecraft.Engine.Render;

public class ShapeRenderer
{
    private readonly ActorPool _actorPool;
    private readonly VisibilityService _visibilityService;

    public ShapeRenderer(ActorPool actorPool, VisibilityService visibilityService)
    {
        _actorPool = actorPool;
        _visibilityService = visibilityService;
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
        foreach (var triangle in _visibilityService.Triangles)
        {
            spriteBatch.DrawLine(triangle.VertexA, triangle.VertexB, Color.Red, 5, 1);
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
            spriteBatch.DrawCircle(circle, 16, Color.White);
        }
        if (shape is RectangleF rectangle)
        {
            spriteBatch.DrawRectangle(rectangle, Color.White);
        }
    }
}
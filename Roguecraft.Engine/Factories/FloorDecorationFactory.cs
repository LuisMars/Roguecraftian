using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class FloorDecorationFactory : ActorFactoryBase<Wall>
{
    public FloorDecorationFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository) :
                  base(configuration, actorPool, collisionService, contentRepository)
    {
    }

    protected override Wall Create(Vector2 position, string? name = null)
    {
        var wall = new Wall
        {
            Name = name ?? "Floor decoration",
            Position = position
        };
        wall.Sprite = new ActorSprite(wall, ContentRepository.Ritual, Configuration.BloodColor.ToColor());

        var bounds = new RectangleF
        {
            Width = Configuration.WallSize * Size.X,
            Height = Configuration.WallSize * Size.Y,
        };

        wall.Collision = new Collision(wall, bounds)
        {
            IsFixed = true,
            IsTransparent = true,
            IsSensor = true
        };
        CollisionService.Insert(wall.Collision);
        return wall;
    }
}
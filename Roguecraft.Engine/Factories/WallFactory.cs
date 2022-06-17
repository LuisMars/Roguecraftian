using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class WallFactory : ActorFactoryBase<Wall>
{
    public WallFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository) :
                  base(configuration, actorPool, collisionService, contentRepository)
    {
    }

    protected override Wall Create(Vector2 position, string? name = null)
    {
        var wall = new Wall
        {
            Name = name ?? "Wall",
            Position = position
        };
        wall.Sprite = new ActorSprite(wall, ContentRepository.Wall, Configuration.WallColor.ToColor());

        var bounds = new RectangleF
        {
            Width = Configuration.WallSize,
            Height = Configuration.WallSize,
        };
        wall.Collision = new Collision(wall, bounds)
        {
            IsFixed = true
        };
        CollisionService.Insert(wall.Collision);
        return wall;
    }
}
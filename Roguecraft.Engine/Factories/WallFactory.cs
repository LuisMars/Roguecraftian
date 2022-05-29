using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class WallFactory : ActorFactoryBase<Wall>
{
    public WallFactory(ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository) : base(actorPool, collisionService, contentRepository)
    {
    }

    protected override Wall Create(Vector2 position, string? name = null)
    {
        var wall = new Wall
        {
            Image = ContentRepository.Wall,
            Name = name ?? "Wall",
            Position = position
        };

        wall.Collision = new Collision
        {
            Actor = wall,
            Bounds = new RectangleF
            {
                Width = 100,
                Height = 100,
            },
            IsFixed = true
        };
        CollisionService.Insert(wall.Collision);
        return wall;
    }
}
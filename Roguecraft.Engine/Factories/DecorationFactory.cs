using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class DecorationFactory : ActorFactoryBase<Wall>
{
    public DecorationFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository) :
                  base(configuration, actorPool, collisionService, contentRepository)
    {
    }

    protected override Wall Create(Vector2 position, string? name = null)
    {
        var wall = new Wall
        {
            Color = Configuration.WallColor.ToColor(),
            Texture = ContentRepository.Decoration,
            Name = name ?? "Wall",
            Position = position
        };

        wall.Collision = new Collision
        {
            Actor = wall,
            Bounds = new RectangleF
            {
                Width = Configuration.WallSize,
                Height = Configuration.WallSize,
            },
            IsFixed = true,
            IsTransparent = true,
        };
        CollisionService.Insert(wall.Collision);
        return wall;
    }
}
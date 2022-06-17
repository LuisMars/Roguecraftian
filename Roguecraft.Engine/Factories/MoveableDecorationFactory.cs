using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class MoveableDecorationFactory : ActorFactoryBase<Wall>
{
    public MoveableDecorationFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository) :
                  base(configuration, actorPool, collisionService, contentRepository)
    {
    }

    private TextureRegion2D? Texture { get; set; }

    public void AddBarrel(Vector2 position, string? name = null)
    {
        Texture = ContentRepository.Barrel;
        Add(position, name);
    }

    public void AddChair(Vector2 position, string? name = null)
    {
        Texture = ContentRepository.Chair;
        Add(position, "Chair");
    }

    public void AddTable(Vector2 position, Vector2 vector2, string? name = null)
    {
        Texture = ContentRepository.Table;
        Add(position, vector2, name);
    }

    protected override Wall Create(Vector2 position, string? name = null)
    {
        var wall = new Wall
        {
            Name = name ?? "Moveable decoration",
            Position = position
        };

        wall.Sprite = new ActorSprite(wall, Texture ?? ContentRepository.Chair, Configuration.WoodColor.ToColor());

        IShapeF bounds = new CircleF
        {
            Radius = Configuration.WallSize / 2f
        };
        if (Size != Vector2.One)
        {
            bounds = new RectangleF
            {
                Width = Configuration.WallSize * Size.X,
                Height = Configuration.WallSize * Size.Y,
            };
        }

        wall.Collision = new Collision(wall, bounds)
        {
            IsTransparent = true,
        };

        CollisionService.Insert(wall.Collision);
        return wall;
    }
}
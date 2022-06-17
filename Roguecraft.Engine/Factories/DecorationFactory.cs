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

public class DecorationFactory : ActorFactoryBase<Wall>
{
    public DecorationFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository) :
                  base(configuration, actorPool, collisionService, contentRepository)
    {
    }

    private Color? Color { get; set; }
    private TextureRotation Rotation { get; set; }
    private TextureRegion2D? Texture { get; set; }

    public void AddBed(Vector2 position, Vector2 size, TextureRotation rotated)
    {
        Color = Configuration.BedColor.ToColor();
        Texture = ContentRepository.Bed;
        Rotation = rotated;
        Add(position, size, "Bed");
    }

    public void AddBookshelf(Vector2 position, TextureRotation rotated)
    {
        Texture = ContentRepository.Bookshelf;
        Rotation = rotated;
        Add(position, Vector2.One, "Bookshelf");
    }

    public void AddCouch(Vector2 position, Vector2 size, TextureRotation rotated)
    {
        Color = Configuration.BedColor.ToColor();
        Texture = ContentRepository.Coach;
        Rotation = rotated;
        Add(position, size, "Bed");
    }

    public void AddGeneric(Vector2 position)
    {
        Texture = ContentRepository.Decoration;
        Add(position);
    }

    protected override Wall Create(Vector2 position, string? name = null)
    {
        var wall = new Wall
        {
            Name = name ?? "Decoration",
            Position = position,
        };
        wall.Sprite = new ActorSprite(wall, Texture ?? ContentRepository.Decoration, Color ?? Configuration.WoodColor.ToColor())
        {
            TextureRotation = Rotation
        };

        var bounds = new RectangleF
        {
            Width = Configuration.WallSize * Size.X,
            Height = Configuration.WallSize * Size.Y,
        };

        wall.Collision = new Collision(wall, bounds)
        {
            IsFixed = true,
            IsTransparent = true
        };
        CollisionService.Insert(wall.Collision);
        return wall;
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    private SpriteEffects? SpriteEffect { get; set; }
    private TextureRegion2D? Texture { get; set; }

    public void AddBed(Vector2 position, Vector2 size, SpriteEffects spriteEffect, TextureRotation rotated)
    {
        Color = Configuration.BedColor.ToColor();
        Texture = ContentRepository.Bed;
        SpriteEffect = spriteEffect;
        Rotation = rotated;
        Add(position, size, "Bed");
    }

    public void AddBookshelf(Vector2 position, Vector2 size, SpriteEffects spriteEffect, TextureRotation rotated)
    {
        Texture = ContentRepository.Bookshelf;
        SpriteEffect = spriteEffect;
        Rotation = rotated;
        Add(position, size, "Bookshelf");
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
            Color = Color ?? Configuration.WoodColor.ToColor(),
            Texture = Texture ?? ContentRepository.Decoration,
            Name = name ?? "Decoration",
            Position = position,
            SpriteEffects = SpriteEffect ?? SpriteEffects.None,
            TextureRotation = Rotation
        };

        wall.Collision = new Collision
        {
            Actor = wall,
            Bounds = new RectangleF
            {
                Width = Configuration.WallSize * Size.X,
                Height = Configuration.WallSize * Size.Y,
            },
            IsFixed = true,
            IsTransparent = true,
        };
        CollisionService.Insert(wall.Collision);
        return wall;
    }
}
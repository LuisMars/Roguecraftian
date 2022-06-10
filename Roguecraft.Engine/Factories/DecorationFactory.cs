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

    private SpriteEffects? SpriteEffect { get; set; }
    private TextureRegion2D? Texture { get; set; }

    public void AddBookshelf(Vector2 position, Vector2 size, SpriteEffects spriteEffect)
    {
        Add(position, size, "Bookshelf");
        Texture = ContentRepository.Bookshelf;
        SpriteEffect = spriteEffect;
    }

    protected override Wall Create(Vector2 position, string? name = null)
    {
        var wall = new Wall
        {
            Color = Configuration.WoodColor.ToColor(),
            Texture = Texture ?? ContentRepository.Decoration,
            Name = name ?? "Decoration",
            Position = position,
            SpriteEffects = SpriteEffect ?? SpriteEffects.None
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
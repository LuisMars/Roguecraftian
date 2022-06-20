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

public class FloorDecorationFactory : ActorFactoryBase<Wall>
{
    public FloorDecorationFactory(Configuration configuration, ActorPool actorPool, CollisionService collisionService, ContentRepository contentRepository) :
                  base(configuration, actorPool, collisionService, contentRepository)
    {
    }

    private Color Color { get; set; }
    private TextureRegion2D Texture { get; set; }
    private TextureRotation TextureRotation { get; set; } = TextureRotation.None;

    internal void AddRitual(Vector2 position, Vector2 size)
    {
        Texture = ContentRepository.Ritual;
        Color = Configuration.BloodColor.ToColor();
        Add(position, size, "Ritual");
    }

    internal void AddTorch(Vector2 position, TextureRotation rotation)
    {
        Texture = ContentRepository.Torch;
        Color = Configuration.WoodColor.ToColor();
        TextureRotation = rotation;
        Add(position, Vector2.One, "Torch");
    }

    protected override Wall Create(Vector2 position, string? name = null)
    {
        var wall = new Wall
        {
            Name = name ?? "Floor decoration",
            Position = position
        };
        wall.Sprite = new ActorSprite(wall, Texture, Color);
        wall.Sprite.TextureRotation = TextureRotation;
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
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Content;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Random;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Factories;

public class MoveableDecorationFactory : ActorFactoryBase<Wall>
{
    private readonly RandomGenerator _randomGenerator;

    public MoveableDecorationFactory(Configuration configuration,
                                     ActorPool actorPool,
                                     CollisionService collisionService,
                                     RandomGenerator randomGenerator,
                                     ContentRepository contentRepository) :
                  base(configuration, actorPool, collisionService, contentRepository)
    {
        _randomGenerator = randomGenerator;
    }

    private float Angle { get; set; } = 0;
    private TextureRotation Rotation { get; set; } = TextureRotation.None;
    private TextureRegion2D? Texture { get; set; }

    public void AddBarrel(Vector2 position)
    {
        Texture = ContentRepository.Barrel;
        Angle = _randomGenerator.Next(0, MathF.PI * 2);
        Add(position, "Barrel");
    }

    public void AddChair(Vector2 position)
    {
        Texture = ContentRepository.Chair;
        Add(position, "Chair");
    }

    public void AddTable(Vector2 position, Vector2 size)
    {
        Texture = ContentRepository.Table;
        Add(position, size, "Table");
    }

    public void AddWheelchair(Vector2 position, TextureRotation rotation)
    {
        Texture = ContentRepository.Wheelchair;
        Rotation = rotation;
        Add(position, Vector2.One, "Wheelchair");
    }

    protected override Wall Create(Vector2 position, string? name = null)
    {
        var wall = new Wall
        {
            Name = name ?? "Moveable decoration",
            Position = position,
            Angle = Angle
        };

        wall.Sprite = new ActorSprite(wall, Texture ?? ContentRepository.Chair, Configuration.WoodColor.ToColor())
        {
            TextureRotation = Rotation
        };
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
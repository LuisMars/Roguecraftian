using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.TextureAtlases;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Helpers;

namespace Roguecraft.Engine.Components;

public class ActorSprite
{
    public ActorSprite(Actor actor, TextureRegion2D texture, Color color)
    {
        Actor = actor;
        Texture = texture;
        Color = color;
    }

    public Actor Actor { get; set; }

    public float Angle => TextureRotation switch
    {
        TextureRotation.None => Actor.Angle,
        TextureRotation.Clockwise => Actor.Angle + MathF.PI * 0.5f,
        TextureRotation.AntiClockwise => Actor.Angle + MathF.PI * 0.5f,
        TextureRotation.HalfTurn => Actor.Angle,
        _ => throw new NotImplementedException(),
    };

    public Color Color { get; set; }

    public SpriteEffects Effects => TextureRotation switch
    {
        TextureRotation.None => SpriteEffects.None,
        TextureRotation.Clockwise => SpriteEffects.None,
        TextureRotation.AntiClockwise => SpriteEffects.FlipVertically,
        TextureRotation.HalfTurn => SpriteEffects.FlipVertically,
        _ => throw new NotImplementedException(),
    };

    public Vector2 Origin
    {
        get
        {
            if (Texture is null)
            {
                return Vector2.Zero;
            }
            if (Collision.Bounds is CircleF)
            {
                return new Vector2(Texture.Width * 0.5f);
            }

            if (TextureRotation is TextureRotation.None ||
                TextureRotation is TextureRotation.HalfTurn)
            {
                return Vector2.Zero;
            }

            if (Texture.Width == Texture.Height)
            {
                return new Vector2(0, Texture.Width);
            }
            return new Vector2(0, Texture.Height);
        }
    }

    public Vector2 Position
    {
        get
        {
            if (Collision.Bounds is RectangleF rectangle)
            {
                return rectangle.TopLeft;
            }
            if (Collision.Bounds is CircleF circle)
            {
                return circle.Center;
            }

            return Position;
        }
    }

    public Vector2 Scale
    {
        get
        {
            if (Texture is null)
            {
                return Vector2.One;
            }

            if (TextureRotation is TextureRotation.None || TextureRotation is TextureRotation.HalfTurn)
            {
                return new Vector2(Collision.Width / Texture.Width, Collision.Height / Texture.Height);
            }

            return new Vector2(Collision.Width / Texture.Height, Collision.Height / Texture.Width);
        }
    }

    public TextureRegion2D? Texture { get; set; }
    public TextureRotation TextureRotation { get; set; } = TextureRotation.None;
    public float Width => Texture?.Width ?? 0;
    private Collision Collision => Actor.Collision;
}
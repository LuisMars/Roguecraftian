using Microsoft.Xna.Framework;
using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Timers;
using Roguecraft.Engine.Visibility;
using System.Diagnostics;

namespace Roguecraft.Engine.Actors;

[DebuggerDisplay("{Name}: {Position}")]
public abstract class Actor
{
    public float Angle { get; set; }
    public Collision Collision { get; set; }

    //public Color Color { get; set; }

    //public float DrawingAngle => TextureRotation switch
    //{
    //    TextureRotation.None => Angle,
    //    TextureRotation.Clockwise => Angle + MathF.PI * 0.5f,
    //    TextureRotation.AntiClockwise => Angle + MathF.PI * 0.5f,
    //    TextureRotation.HalfTurn => Angle,
    //    _ => throw new NotImplementedException(),
    //};

    //public Vector2 DrawingPosition
    //{
    //    get
    //    {
    //        if (Collision.Bounds is RectangleF rectangle)
    //        {
    //            return rectangle.TopLeft;
    //        }
    //        if (Collision.Bounds is CircleF circle)
    //        {
    //            return circle.Center;
    //        }

    //        return Position;
    //    }
    //}

    public bool IsPickedUp { get; protected set; }
    public string Name { get; set; }

    //public Vector2 Origin
    //{
    //    get
    //    {
    //        if (Collision.Bounds is CircleF)
    //        {
    //            return new Vector2(Texture.Width * 0.5f);
    //        }

    //        if (TextureRotation is TextureRotation.None ||
    //            TextureRotation is TextureRotation.HalfTurn)
    //        {
    //            return Vector2.Zero;
    //        }
    //        if (Texture.Width == Texture.Height)
    //        {
    //            return new Vector2(0, Texture.Width);
    //        }
    //        return new Vector2(0, Texture.Height);
    //    }
    //}

    public Vector2 Position { get; set; }
    public ActorSprite Sprite { get; set; }

    //public Vector2 Scale
    //{
    //    get
    //    {
    //        if (Texture is null)
    //        {
    //            return Vector2.One;
    //        }

    //        if (TextureRotation is TextureRotation.None || TextureRotation is TextureRotation.HalfTurn)
    //        {
    //            return new Vector2(Collision.Width / Texture.Width, Collision.Height / Texture.Height);
    //        }

    //        return new Vector2(Collision.Width / Texture.Height, Collision.Height / Texture.Width);
    //    }
    //}

    //public SpriteEffects SpriteEffects => TextureRotation switch
    //{
    //    TextureRotation.None => SpriteEffects.None,
    //    TextureRotation.Clockwise => SpriteEffects.None,
    //    TextureRotation.AntiClockwise => SpriteEffects.FlipVertically,
    //    TextureRotation.HalfTurn => SpriteEffects.FlipVertically,
    //    _ => throw new NotImplementedException(),
    //};

    //public TextureRegion2D Texture { get; set; }
    //public TextureRotation TextureRotation { get; set; }
    public TimerManager Timers { get; } = new TimerManager();

    public VisibilityProperties Visibility { get; set; } = new();

    public void AfterUpdate(float deltaTime) => UpdateTimers(deltaTime);

    public virtual void CalculateVisibility(VisibilityService visibilityService)
    {
        var isVisible = Visibility.IsDectectedAsVisible ||
            (Collision.IsTransparent && visibilityService.IsVisible(Position, Collision.Bounds));
        Visibility.IsDectectedAsVisible = false;
        Visibility.IsVisibleByHero = isVisible;
        if (Collision.IsFixed && Visibility.IsVisibleByHero)
        {
            Visibility.TimesSeen++;
        }
    }

    public virtual void ClearSimulationData() => Collision?.Clear();

    public virtual GameAction? TakeTurn(float deltaTime) => null;

    public virtual void UpdateSimulationData() => Collision?.Update();

    private void UpdateTimers(float deltaTime) => Timers.Update(deltaTime);
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
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
    public Color Color { get; set; }
    public bool IsPickedUp { get; protected set; }
    public string Name { get; set; }

    public Vector2 Origin
    {
        get
        {
            return new Vector2(0.5f, 0.5f) * (Texture?.Width ?? 0);
        }
    }

    public Vector2 DrawingOffset
    {
        get
        {
            return (Collision?.Origin ?? Vector2.Zero) * (Texture?.Width ?? 0) * Scale;
        }
    }

    public Vector2 Position { get; set; }

    public Vector2 Scale
    {
        get
        {
            if (Texture is null)
            {
                return Vector2.One;
            }
            return new Vector2(Collision.Width / Texture.Width, Collision.Height / Texture.Height);
        }
    }

    public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
    public TextureRegion2D Texture { get; set; }
    public TimerManager Timers { get; } = new TimerManager();
    public VisibilityProperties Visibility { get; set; } = new();

    public void AfterUpdate(float deltaTime)
    {
        UpdateTimers(deltaTime);
    }

    public void CalculateVisibility(VisibilityService visibilityService)
    {
        var isVisible = visibilityService.IsVisible(Position, Collision.Bounds);

        Visibility.IsVisibleByHero = isVisible;
        if (Collision.IsFixed && Visibility.IsVisibleByHero)
        {
            Visibility.TimesSeen++;
        }
    }

    public virtual void ClearSimulationData()
    {
        Collision?.Clear();
    }

    public virtual GameAction? TakeTurn(float deltaTime)
    {
        return null;
    }

    public virtual void UpdateSimulationData()
    {
        Collision?.Update();
    }

    private void UpdateTimers(float deltaTime)
    {
        Timers.Update(deltaTime);
    }
}
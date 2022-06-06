using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;
using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Visibility;
using System.Diagnostics;

namespace Roguecraft.Engine.Actors;

[DebuggerDisplay("{Name}: {Position}")]
public abstract class Actor
{
    public float Angle { get; set; }
    public Collision Collision { get; set; }
    public Color Color { get; set; }
    public string Name { get; set; }
    public NullAction NullAction { get; init; }

    public Vector2 Origin
    {
        get
        {
            return Collision.Origin * Texture.Width;
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

    public TextureRegion2D Texture { get; set; }
    public VisibilityProperties Visibility { get; set; } = new();

    public virtual void AfterUpdate(float deltaTime)
    {
    }

    public virtual void ClearSimulationData()
    {
        Collision.Clear();
    }

    public virtual GameAction? TakeTurn(float deltaTime)
    {
        return null;
    }

    public virtual void UpdateSimulationData()
    {
        Collision.Update();
    }

    internal void CalculateVisibility(VisibilityService visibilityService)
    {
        var isVisible = visibilityService.IsVisible(Position, Collision.Bounds);

        Visibility.IsVisibleByPlayer = isVisible;
        if (Collision.IsFixed && Visibility.IsVisibleByPlayer)
        {
            Visibility.TimesSeen++;
        }
    }
}
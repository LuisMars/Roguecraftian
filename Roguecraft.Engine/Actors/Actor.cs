using Microsoft.Xna.Framework;
using MonoGame.Extended.TextureAtlases;
using Roguecraft.Engine.Actions;
using Roguecraft.Engine.Components;
using System.Diagnostics;

namespace Roguecraft.Engine.Actors;

[DebuggerDisplay("{Name}: {Position}")]
public abstract class Actor
{
    public TextureRegion2D Image;
    public float Angle { get; set; }
    public Collision Collision { get; set; }
    public string Name { get; set; }
    public NullAction NullAction { get; init; }

    public Vector2 Origin
    {
        get
        {
            return Collision.Origin * Image.Width;
        }
    }

    public Vector2 Position { get; set; }

    public Vector2 Scale
    {
        get
        {
            if (Image is null)
            {
                return Vector2.One;
            }
            return new Vector2(Collision.Width / Image.Width, Collision.Height / Image.Height);
        }
    }

    public float Speed { get; set; }

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
}
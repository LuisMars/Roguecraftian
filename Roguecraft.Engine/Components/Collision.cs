using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Components;

public class Collision
{
    public Actor Actor { get; set; }
    public IShapeF Bounds { get; set; }

    public float Height
    {
        get
        {
            return Bounds switch
            {
                RectangleF rectangle => rectangle.Height,
                CircleF circle => circle.Radius * 2,
                _ => 1
            };
        }
    }

    public bool IsFixed { get; set; }
    public bool IsSensor { get; set; }
    public HashSet<CollisionArgs> LastEvents { get; set; } = new();

    public Vector2 Origin
    {
        get
        {
            return Bounds switch
            {
                RectangleF => Vector2.Zero,
                CircleF => new Vector2(0.5f, 0.5f),
                _ => Vector2.Zero
            };
        }
    }

    public float Width
    {
        get
        {
            return Bounds switch
            {
                RectangleF rectangle => rectangle.Width,
                CircleF circle => circle.Radius * 2,
                _ => 1
            };
        }
    }

    internal List<CollisionArgs> InternalEvents { get; set; } = new();

    internal void AddEvent(CollisionArgs collisionInfo)
    {
        InternalEvents.Add(collisionInfo);
        LastEvents.Add(collisionInfo);
    }

    internal void Clear()
    {
        LastEvents.Clear();
    }

    internal void Update()
    {
        InternalEvents.Clear();
        Bounds.Position = Actor.Position;
    }
}
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

    public bool IsTransparent { get; set; }

    public Vector2 Origin
    {
        get
        {
            return Bounds switch
            {
                RectangleF => new Vector2(0.5f, 0.5f),
                CircleF => Vector2.Zero,
                _ => Vector2.Zero
            };
        }
    }

    public Vector2 PenetrationVector { get; set; }

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
    private HashSet<CollisionArgs> LastEvents { get; set; } = new();

    public bool Any<TActor>(Func<CollisionArgs, bool>? extraConditions = null) where TActor : Actor
    {
        return LastEvents.Any(e => e.Other.Actor is TActor && (extraConditions?.Invoke(e) ?? true));
    }

    public CollisionArgs? Closest<TActor>(Func<CollisionArgs, bool>? extraConditions = null) where TActor : Actor
    {
        var closest = LastEvents
            .Where(e => e.Other.Actor is TActor && (extraConditions?.Invoke(e) ?? true))
            .MinBy(e => (e.Other.Actor.Position - Actor.Position).LengthSquared());

        return closest;
    }

    public CollisionArgs? FirstOrDefault<TActor>(Func<CollisionArgs, bool>? extraConditions = null) where TActor : Actor
    {
        return LastEvents.FirstOrDefault(e => e.Other.Actor is TActor && (extraConditions?.Invoke(e) ?? true));
    }

    public IEnumerable<CollisionArgs> Where<TActor>(Func<CollisionArgs, bool>? extraConditions = null) where TActor : Actor
    {
        return LastEvents.Where(e => e.Other.Actor is TActor && (extraConditions?.Invoke(e) ?? true));
    }

    internal void AddEvent(CollisionArgs collisionInfo)
    {
        InternalEvents.Add(collisionInfo);
        LastEvents.Add(collisionInfo);
    }

    internal void Clear()
    {
        PenetrationVector = new Vector2();
        LastEvents.Clear();
    }

    internal void Update()
    {
        InternalEvents.Clear();
        Bounds.Position = Actor.Position;
    }
}
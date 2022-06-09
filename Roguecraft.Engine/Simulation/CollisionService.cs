using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Simulation.Quadtrees;

namespace Roguecraft.Engine.Simulation;

public class CollisionService
{
    private readonly ActorPool _actorPool;

    private readonly Dictionary<Collision, QuadtreeData> _targetDataDictionary = new();

    public CollisionService(ActorPool actorPool)
    {
        _actorPool = actorPool;
    }

    public IEnumerable<Collision> Collisions => _targetDataDictionary.Keys;
    private Quadtree CollisionTree { get; set; }

    public bool Contains(Collision target)
    {
        return _targetDataDictionary.ContainsKey(target);
    }

    public void Initialize(RectangleF bounds)
    {
        CollisionTree = new Quadtree(bounds);
    }

    public void Insert(Collision target)
    {
        if (_targetDataDictionary.ContainsKey(target))
        {
            return;
        }
        var quadtreeData = new QuadtreeData(target);
        _targetDataDictionary.Add(target, quadtreeData);
        CollisionTree.Insert(quadtreeData);
    }

    public IEnumerable<Actor> QueryActors(IShapeF area)
    {
        return CollisionTree.Query(area).Select(d => d.Target.Actor).Distinct();
    }

    public void Remove(Collision target)
    {
        if (!_targetDataDictionary.ContainsKey(target))
        {
            return;
        }
        _targetDataDictionary[target].RemoveFromAllParents();
        _targetDataDictionary.Remove(target);
        CollisionTree.Shake();
    }

    public void Update()
    {
        foreach (var actor in _actorPool.Actors)
        {
            actor.UpdateSimulationData();
        }

        foreach (var value in _targetDataDictionary.Values)
        {
            value.RemoveFromAllParents();
            var target = value.Target;
            if (!target.IsFixed)
            {
                foreach (var item in CollisionTree.Query(target.Bounds))
                {
                    var collisionInfo = new CollisionArgs
                    {
                        Other = item.Target,
                        PenetrationVector = CalculatePenetrationVector(value.Bounds, item.Bounds)
                    };
                    target.AddEvent(collisionInfo);
                    value.Bounds = value.Target.Bounds;
                }
            }

            CollisionTree.Insert(value);
        }

        CollisionTree.Shake();
        ReactToCollission();
    }

    internal void RemoveDead()
    {
        var toRemove = _targetDataDictionary.Where(x => x.Key.Actor.IsPickedUp);
        foreach (var dead in toRemove)
        {
            Remove(dead.Key);
        }
    }

    private static Vector2 CalculatePenetrationVector(IShapeF a, IShapeF b)
    {
        return (a, b) switch
        {
            { a: RectangleF rectA, b: RectangleF rectB } => PenetrationVector(rectA, rectB),
            { a: RectangleF rectA, b: CircleF circleB } => PenetrationVector(rectA, circleB),
            { a: CircleF circleA, b: RectangleF rectB } => PenetrationVector(circleA, rectB),
            { a: CircleF circleA, b: CircleF circleB } => PenetrationVector(circleA, circleB),
            _ => throw new NotSupportedException("Shapes must be either a CircleF or RectangleF")
        };
    }

    private static Vector2 PenetrationVector(RectangleF rect1, RectangleF rect2)
    {
        var rectangleF = RectangleF.Intersection(rect1, rect2);
        Vector2 result;
        if (rectangleF.Width < rectangleF.Height)
        {
            float x = (rect1.Center.X < rect2.Center.X) ? rectangleF.Width : (0f - rectangleF.Width);
            result = new Vector2(x, 0f);
        }
        else
        {
            float y = (rect1.Center.Y < rect2.Center.Y) ? rectangleF.Height : (0f - rectangleF.Height);
            result = new Vector2(0f, y);
        }

        return result;
    }

    private static Vector2 PenetrationVector(CircleF circ1, CircleF circ2)
    {
        if (!circ1.Intersects(circ2))
        {
            return Vector2.Zero;
        }

        var vector = Point2.Displacement(circ1.Center, circ2.Center);
        var value = (!(vector != Vector2.Zero)) ? (-Vector2.UnitY * (circ1.Radius + circ2.Radius)) : (vector.NormalizedCopy() * (circ1.Radius + circ2.Radius));
        return vector - value;
    }

    private static Vector2 PenetrationVector(CircleF circ, RectangleF rect)
    {
        var vector = rect.ClosestPointTo(circ.Center) - circ.Center;
        if (!rect.Contains(circ.Center) && !vector.Equals(Vector2.Zero))
        {
            return circ.Radius * vector.NormalizedCopy() - vector;
        }

        var value = Point2.Displacement(circ.Center, rect.Center);
        Vector2 value2;
        if (value == Vector2.Zero)
        {
            return Vector2.UnitY * (circ.Radius + rect.Height / 2f);
        }
        var vector2 = new Vector2(value.X, 0f);
        var vector3 = new Vector2(0f, value.Y);
        vector2.Normalize();
        vector3.Normalize();
        vector2 *= circ.Radius + rect.Width / 2f;
        vector3 *= circ.Radius + rect.Height / 2f;
        if (vector2.LengthSquared() < vector3.LengthSquared())
        {
            value2 = vector2;
            value.Y = 0f;
        }
        else
        {
            value2 = vector3;
            value.X = 0f;
        }

        return value - value2;
    }

    private static Vector2 PenetrationVector(RectangleF rect, CircleF circ)
    {
        return -PenetrationVector(circ, rect);
    }

    private static void ReactToCollission(Actor actor)
    {
        if (actor.Collision is null)
        {
            return;
        }
        var collision = actor.Collision;
        var events = actor.Collision.InternalEvents;
        if (collision.IsFixed || collision.IsSensor || !events.Any())
        {
            return;
        }

        actor.Position -= events.Where(x => !x.Other.IsSensor)
                                .Sum(x => x.PenetrationVector);
    }

    private void ReactToCollission()
    {
        foreach (var actor in _actorPool.Actors)
        {
            ReactToCollission(actor);
        }
    }
}
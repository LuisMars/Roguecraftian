using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Simulation;
using System.Collections.Concurrent;

namespace Roguecraft.Engine.Visibility;

internal class ExtendedVisibility
{
    private readonly CollisionService _collisionService;
    private readonly VisibilityComputer _visibilityComputer;

    public ExtendedVisibility(CollisionService collisionService)
    {
        _collisionService = collisionService;
        _visibilityComputer = new VisibilityComputer();
        Radius = 500;
        RadiusSquared = Radius * Radius;
    }

    public Vector2 Center { get; private set; }
    public float Radius { get; private set; }
    public float RadiusSquared { get; private set; }

    public List<TriangleF> Calculate(Actor sourceActor)
    {
        Center = sourceActor.Position;
        _visibilityComputer.Reset(Center, Radius);
        var collidables = _collisionService.Collisions;
        var radiusSquared = RadiusSquared;
        var viewBounds = new RectangleF(Center.X - Radius, Center.Y - Radius, Radius * 2, Radius * 2);

        foreach (var collidable in collidables)
        {
            AddOccluders(_visibilityComputer, sourceActor, collidable, radiusSquared, viewBounds, Center);
        }

        var points = _visibilityComputer.Compute();
        var triangles = new ConcurrentBag<TriangleF>();

        for (var i = 0; i < points.Count; i++)
        {
            AddTriangles(i, points, triangles, Center);
        }

        return triangles.ToList();
    }

    internal IEnumerable<Actor> QueryCandidates(float multiplier = 1)
    {
        var viewBounds = new CircleF(Center, Radius * multiplier);

        return _collisionService.QueryActors(viewBounds);
    }

    private static void AddOccluders(IVisibilityComputer visibilityComputer, Actor sourceActor, Collision collidable, float radiusSquared, RectangleF viewBounds, Vector2 origin)
    {
        if (sourceActor == collidable.Actor || collidable.IsSensor || collidable.IsTransparent)
        {
            return;
        }
        var shape = collidable.Bounds;
        if (shape is CircleF circle)
        {
            if ((origin - (Vector2)circle.Center).LengthSquared() > radiusSquared)
            {
                return;
            }
            circle.Radius *= 0.75f;
            var sides = 6f;
            for (int i = 0; i < sides / 2; i++)
            {
                var pointA = circle.BoundaryPointAt(MathHelper.ToRadians(i * 360 / sides));
                var pointB = circle.BoundaryPointAt(MathHelper.ToRadians((i + 3) * 360 / sides));
                visibilityComputer.AddLineOccluder(pointA, pointB);
            }
            return;
        }

        if (shape is RectangleF rect)
        {
            if ((origin - (Vector2)rect.Center).LengthSquared() > radiusSquared)
            {
                return;
            }
            visibilityComputer.AddLineOccluder(rect.TopLeft, rect.BottomRight);
            visibilityComputer.AddLineOccluder(rect.BottomLeft, rect.TopRight);
        }
    }

    private static void AddTriangles(int i, List<Vector2> points, ConcurrentBag<TriangleF> triangles, Vector2 origin)
    {
        if (i % 2 == 1)
        {
            return;
        }

        var diff1 = points[i] - points[i + 1];
        if (diff1.LengthSquared() == 0)
        {
            return;
        }

        triangles.Add(new TriangleF(points[i], points[i + 1], origin));
    }
}
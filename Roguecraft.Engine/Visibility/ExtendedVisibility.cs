using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Components;
using Roguecraft.Engine.Helpers;
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
    }

    public List<TriangleF> Init(Actor sourceActor, float radius = 1000)
    {
        var origin = sourceActor.Position;
        _visibilityComputer.Reset(origin, radius);
        var collidables = _collisionService.Collisions;
        var radiusSquared = radius * radius;
        var viewBounds = new RectangleF((int)(origin.X - radius), (int)(origin.Y - radius), (int)(radius * 2), (int)(radius * 2));

        Parallel.ForEach(collidables, (collidable) =>
        {
            AddOccluders(_visibilityComputer, sourceActor, collidable, radiusSquared, viewBounds, origin);
        });

        var points = _visibilityComputer.Compute();
        var triangles = new ConcurrentBag<TriangleF>();
        Parallel.For(0, points.Count, i =>
        {
            AddTriangles(i, points, triangles, origin);
        });

        return triangles.ToList();
    }

    private static void AddOccluders(IVisibilityComputer visibilityComputer, Actor sourceActor, Collision collidable, float radiusSquared, RectangleF viewBounds, Vector2 origin)
    {
        if (sourceActor == collidable.Actor)
        {
            return;
        }
        if (collidable.IsSensor)
        {
            return;
        }
        var shape = collidable.Bounds;
        if (shape is CircleF circle)
        {
            if (!viewBounds.Contains(circle))
            {
                return;
            }
            circle.Radius *= 0.75f;
            var sides = 6f;
            for (int i = 0; i < 3; i++)
            {
                var pointA = circle.BoundaryPointAt(MathHelper.ToRadians(i * 360 / sides));
                var pointB = circle.BoundaryPointAt(MathHelper.ToRadians((i + 3) * 360 / sides));
                visibilityComputer.AddLineOccluder(pointA, pointB);
            }
            return;
        }

        if (shape is RectangleF rect)
        {
            if (!viewBounds.Contains(rect))
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
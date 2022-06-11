using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Geometry;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Visibility;

public class VisibilityService
{
    private readonly CollisionService _collisionService;
    private readonly ExtendedVisibility _extendedVisibility;

    public VisibilityService(CollisionService collisionService)
    {
        _extendedVisibility = new ExtendedVisibility(collisionService);
        _collisionService = collisionService;
    }

    public int Count => Triangles.Count;

    public IEnumerable<TriangleF> Sorted
    {
        get
        {
            return Triangles.OrderBy(t => (t.VertexB - t.VertexC).ToAngle());
        }
    }

    public List<TriangleF> Triangles { get; private set; }

    public IEnumerable<Actor> InActionRange()
    {
        return _extendedVisibility.QueryCandidates(1.25f);
    }

    public IEnumerable<Actor> InVisibilityRange()
    {
        return _extendedVisibility.QueryCandidates();
    }

    internal bool IsVisible(Vector2 position, IShapeF shape)
    {
        if ((_extendedVisibility.Center - position).LengthSquared() > _extendedVisibility.RadiusSquared * 1.25f)
        {
            return false;
        }
        return Triangles.Any(t =>
        {
            if (shape is null)
            {
                return t.Contains(position);
            }
            return t.Intersects(shape);
        });
    }

    internal void Update(Actor sourceActor)
    {
        Triangles = _extendedVisibility.Calculate(sourceActor);
    }
}
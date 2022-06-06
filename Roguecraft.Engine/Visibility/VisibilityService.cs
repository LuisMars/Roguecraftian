using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Visibility;

public class VisibilityService
{
    private readonly ExtendedVisibility _extendedVisibility;

    public VisibilityService(CollisionService collisionService)
    {
        _extendedVisibility = new ExtendedVisibility(collisionService);
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

    internal bool IsVisible(Vector2 position, IShapeF shape)
    {
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
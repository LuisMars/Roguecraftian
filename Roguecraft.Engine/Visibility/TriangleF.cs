using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Roguecraft.Engine.Visibility;

public class TriangleF
{
    public TriangleF(Vector2 vertexA, Vector2 vertexB, Vector2 vertexC)
    {
        Segments[0] = new Segment2(vertexA, vertexB);
        Segments[1] = new Segment2(vertexB, vertexC);
        Segments[2] = new Segment2(vertexC, vertexA);
        VertexA = vertexA;
        VertexB = vertexB;
        VertexC = vertexC;
    }

    public Segment2[] Segments { get; set; } = new Segment2[3];
    public Vector2 VertexA { get; set; }
    public Vector2 VertexB { get; set; }
    public Vector2 VertexC { get; set; }
    public List<Vector2> Vertices => new() { VertexA, VertexB, VertexC };

    public bool Contains(Vector2 pt)
    {
        float d1, d2, d3;
        bool has_neg, has_pos;

        d1 = Sign(pt, VertexA, VertexB);
        d2 = Sign(pt, VertexB, VertexC);
        d3 = Sign(pt, VertexC, VertexA);

        has_neg = d1 < 0 || d2 < 0 || d3 < 0;
        has_pos = d1 > 0 || d2 > 0 || d3 > 0;

        return !(has_neg && has_pos);
    }

    public bool Intersects(RectangleF rectangle)
    {
        return Segments[0].Intersects(rectangle, out _) ||
               Segments[1].Intersects(rectangle, out _) ||
               Segments[2].Intersects(rectangle, out _);
    }

    public bool Intersects(CircleF circle)
    {
        return Segments[0].Intersects(circle) ||
               Segments[1].Intersects(circle) ||
               Segments[2].Intersects(circle);
    }

    public bool Intersects(IShapeF shape)
    {
        if (shape is CircleF circle)
        {
            return Intersects(circle);
        }
        if (shape is RectangleF rectangle)
        {
            return Intersects((CircleF)rectangle);
        }
        return false;
    }

    private float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
    }
}
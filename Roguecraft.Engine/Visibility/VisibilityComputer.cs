using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Helpers;
using System.Collections.Concurrent;

namespace Roguecraft.Engine.Visibility;

/// <summary>
/// Class which computes a mesh that represents which regions are
/// visibile from the Origin point given a set of occluders
/// </summary>
public class VisibilityComputer : IVisibilityComputer
{
    // These represent the map and light location:
    private readonly ConcurrentBag<EndPoint> _endpoints;

    private readonly EndPointComparer _radialComparer;
    private readonly ConcurrentBag<Segment> _segments;

    public VisibilityComputer()
    {
        _segments = new ConcurrentBag<Segment>();
        _endpoints = new ConcurrentBag<EndPoint>();
        _radialComparer = new EndPointComparer();
    }

    /// <summary>
    /// The Origin, or position of the observer
    /// </summary>
    public Vector2 Origin { get; private set; }

    /// <summary>
    /// The maxiumum view distance
    /// </summary>
    public float Radius { get; private set; }

    /// <summary>
    /// Add a line shaped occluder
    /// </summary>
    public void AddLineOccluder(Vector2 p1, Vector2 p2)
    {
        AddSegment(p1, p2);
    }

    /// <summary>
    /// Remove all occluders
    /// </summary>
    public void ClearOccluders()
    {
        _segments.Clear();
        _endpoints.Clear();

        LoadBoundaries();
    }

    /// <summary>
    /// Computes the visibility polygon and returns the vertices
    /// of the triangle fan (minus the center vertex)
    /// </summary>
    public List<Vector2> Compute()
    {
        var output = new List<Vector2>();
        var open = new LinkedList<Segment>();

        UpdateSegments();

        var endpoints = _endpoints.ToList();
        endpoints.Sort(_radialComparer);

        float currentAngle = 0;

        // At the beginning of the sweep we want to know which
        // segments are active. The simplest way to do this is to make
        // a pass collecting the segments, and make another pass to
        // both collect and process them. However it would be more
        // efficient to go through all the segments, figure out which
        // ones intersect the initial sweep line, and then sort them.
        for (int pass = 0; pass < 2; pass++)
        {
            foreach (var p in endpoints)
            {
                var currentOld = open.Count == 0 ? null : open.First.Value;

                if (p.Begin)
                {
                    // Insert into the right place in the list
                    var node = open.First;
                    while (node != null && SegmentInFrontOf(p.Segment, node.Value, Origin))
                    {
                        node = node.Next;
                    }

                    if (node == null)
                    {
                        open.AddLast(p.Segment);
                    }
                    else
                    {
                        open.AddBefore(node, p.Segment);
                    }
                }
                else
                {
                    open.Remove(p.Segment);
                }

                Segment currentNew = null;
                if (open.Any())
                {
                    currentNew = open.First.Value;
                }

                if (currentOld != currentNew)
                {
                    if (pass == 1)
                    {
                        AddTriangle(output, currentAngle, p.Angle, currentOld);
                    }
                    currentAngle = p.Angle;
                }
            }
        }

        return output;
    }

    internal void Reset(Vector2 origin, float radius)
    {
        Origin = origin;
        Radius = radius;
        ClearOccluders();
    }

    // Helper: do we know that segment a is in front of b?
    // Implementation not anti-symmetric (that is to say,
    // _segment_in_front_of(a, b) != (!_segment_in_front_of(b, a)).
    // Also note that it only has to work in a restricted set of cases
    // in the visibility algorithm; I don't think it handles all
    // cases. See http://www.redblobgames.com/articles/visibility/segment-sorting.html
    private static bool SegmentInFrontOf(Segment a, Segment b, Vector2 relativeTo)
    {
        // NOTE: we could slightly shorten the segments so that
        // intersections of the endpoints (common) don't count as
        // intersections in this algorithm

        var a1 = VectorMath.LeftOf(a.P2.Position, a.P1.Position, VectorMath.Interpolate(b.P1.Position, b.P2.Position, 0.05f));
        var a2 = VectorMath.LeftOf(a.P2.Position, a.P1.Position, VectorMath.Interpolate(b.P2.Position, b.P1.Position, 0.05f));
        var a3 = VectorMath.LeftOf(a.P2.Position, a.P1.Position, relativeTo);

        var b1 = VectorMath.LeftOf(b.P2.Position, b.P1.Position, VectorMath.Interpolate(a.P1.Position, a.P2.Position, 0.05f));
        var b2 = VectorMath.LeftOf(b.P2.Position, b.P1.Position, VectorMath.Interpolate(a.P2.Position, a.P1.Position, 0.05f));
        var b3 = VectorMath.LeftOf(b.P2.Position, b.P1.Position, relativeTo);
        // NOTE: this algorithm is probably worthy of a short article
        // but for now, draw it on paper to see how it works. Consider
        // the line A1-A2. If both B1 and B2 are on one side and
        // relativeTo is on the other side, then A is in between the
        // viewer and B. We can do the same with B1-B2: if A1 and A2
        // are on one side, and relativeTo is on the other side, then
        // B is in between the viewer and A.
        if (b1 == b2 && b2 != b3) return true;
        if (a1 == a2 && a2 == a3) return true;
        if (a1 == a2 && a2 != a3) return false;
        if (b1 == b2 && b2 == b3) return false;

        // If A1 != A2 and B1 != B2 then we have an intersection.
        // Expose it for the GUI to show a message. A more robust
        // implementation would split segments at intersections so
        // that part of the segment is in front and part is behind.

        //demo_intersectionsDetected.push([a.p1, a.p2, b.p1, b.p2]);
        return false;

        // NOTE: previous implementation was a.d < b.d. That's simpler
        // but trouble when the segments are of dissimilar sizes. If
        // you're on a grid and the segments are similarly sized, then
        // using distance will be a simpler and faster implementation.
    }

    // Add a segment, where the first point shows up in the
    // visualization but the second one does not. (Every endpoint is
    // part of two segments, but we want to only show them once.)
    private void AddSegment(Vector2 p1, Vector2 p2)
    {
        var segment = new Segment();
        var endPoint1 = new EndPoint();
        var endPoint2 = new EndPoint();

        endPoint1.Position = p1;
        endPoint1.Segment = segment;

        endPoint2.Position = p2;
        endPoint2.Segment = segment;

        segment.P1 = endPoint1;
        segment.P2 = endPoint2;

        _segments.Add(segment);
        _endpoints.Add(endPoint1);
        _endpoints.Add(endPoint2);
    }

    private void AddTriangle(List<Vector2> triangles, float angle1, float angle2, Segment segment)
    {
        var p1 = Origin;
        var p2 = new Vector2(Origin.X + MathF.Cos(angle1), Origin.Y + MathF.Sin(angle1));
        var p3 = Vector2.Zero;
        var p4 = Vector2.Zero;

        if (segment != null)
        {
            // Stop the triangle at the intersecting segment
            p3.X = segment.P1.Position.X;
            p3.Y = segment.P1.Position.Y;
            p4.X = segment.P2.Position.X;
            p4.Y = segment.P2.Position.Y;
        }
        else
        {
            // Stop the triangle at a fixed distance; this probably is
            // not what we want, but it never gets used in the demo
            p3.X = Origin.X + MathF.Cos(angle1) * Radius * 2;
            p3.Y = Origin.Y + MathF.Sin(angle1) * Radius * 2;
            p4.X = Origin.X + MathF.Cos(angle2) * Radius * 2;
            p4.Y = Origin.Y + MathF.Sin(angle2) * Radius * 2;
        }

        var pBegin = VectorMath.LineLineIntersection(p3, p4, p1, p2);

        p2.X = Origin.X + MathF.Cos(angle2);
        p2.Y = Origin.Y + MathF.Sin(angle2);

        var pEnd = VectorMath.LineLineIntersection(p3, p4, p1, p2);

        triangles.Add(pBegin);
        triangles.Add(pEnd);
    }

    /// <summary>
    /// Helper function to construct segments along the outside perimiter
    /// in order to limit the radius of the light
    /// </summary>
    private void LoadBoundaries()
    {
        var circle = new CircleF(Origin, Radius);
        var last = circle.BoundaryPointAt(0);
        for (int i = 10; i <= 360; i += 10)
        {
            var current = circle.BoundaryPointAt(MathHelper.ToRadians(i));
            AddSegment(last, current);
            last = current;
        }
    }

    // Processess segments so that we can sort them later
    private void UpdateSegments()
    {
        //Parallel.ForEach(_segments, segment =>
        foreach (Segment segment in _segments)
        {
            // NOTE: future optimization: we could record the quadrant
            // and the y/x or x/y ratio, and sort by (quadrant,
            // ratio), instead of calling atan2. See
            // <https://github.com/mikolalysenko/compare-slope> for a
            // library that does this.

            segment.P1.Angle = MathF.Atan2(segment.P1.Position.Y - Origin.Y, segment.P1.Position.X - Origin.X);
            segment.P2.Angle = MathF.Atan2(segment.P2.Position.Y - Origin.Y, segment.P2.Position.X - Origin.X);

            // Map angle between -Pi and Pi
            var dAngle = segment.P2.Angle - segment.P1.Angle;
            if (dAngle <= -MathHelper.Pi)
            {
                dAngle += MathHelper.TwoPi;
            }
            if (dAngle > MathHelper.Pi)
            {
                dAngle -= MathHelper.TwoPi;
            }

            segment.P1.Begin = dAngle > 0.0f;
            segment.P2.Begin = !segment.P1.Begin;
        };
    }
}
using Roguecraft.Engine.Actors;

namespace Roguecraft.Engine.Geometry;

/// <summary>
/// Represents an occluding line segment in the visibility mesh
/// </summary>
internal class Segment
{
    internal Segment(Actor actor)
    {
        Actor = actor;
        P1 = null;
        P2 = null;
    }

    public Actor Actor { get; }

    /// <summary>
    /// First end-point of the segment
    /// </summary>
    internal EndPoint P1 { get; set; }

    /// <summary>
    /// Second end-point of the segment
    /// </summary>
    internal EndPoint P2 { get; set; }

    public override bool Equals(object obj)
    {
        return obj is Segment other && P1.Equals(other.P1) && P2.Equals(other.P2);
    }

    public override int GetHashCode()
    {
        return P1.GetHashCode() + P2.GetHashCode();
    }

    public override string ToString()
    {
        return "{" + P1.Position.ToString() + ", " + P2.Position.ToString() + "}";
    }
}
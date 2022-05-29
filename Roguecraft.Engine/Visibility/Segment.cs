namespace Roguecraft.Engine.Visibility;

/// <summary>
/// Represents an occluding line segment in the visibility mesh
/// </summary>
internal class Segment
{
    internal Segment()
    {
        P1 = null;
        P2 = null;
    }

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
        if (obj is Segment other)
        {
            return P1.Equals(other.P1) &&
                    P2.Equals(other.P2);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return P1.GetHashCode() +
                P2.GetHashCode();
    }

    public override string ToString()
    {
        return "{" + P1.Position.ToString() + ", " + P2.Position.ToString() + "}";
    }
}
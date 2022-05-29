namespace Roguecraft.Engine.Procedural.Dungeons;

public class WayPointConnection
{
    public (int X, int Y) PointA { get; set; }
    public (int X, int Y) PointB { get; set; }
    public float Weight { get; internal set; } = 1;
}
using Microsoft.Xna.Framework;

namespace Roguecraft.Engine.Procedural.Dungeons;

public class Room
{
    private readonly List<(int X, int Y)> _waypoints = new();

    public Room(int x, int y, int width, int height)
    {
        Bounds = new Rectangle(x, y, width, height);
    }

    public int Bottom => Bounds.Bottom;
    public Rectangle Bounds { get; private set; }
    public bool CanHaveConnections { get; internal set; } = true;
    public Vector2 Center => Bounds.Center.ToVector2();

    public Vector2 FloatCenter => new(Left + (Width + 1) * 0.5f, Top + (Height + 1) * 0.5f);

    public int Height => Bounds.Height;
    public int Left => Bounds.Left;
    public int Right => Bounds.Right;
    public int Top => Bounds.Top;
    public int Width => Bounds.Width;
    public int X => Bounds.X;
    public int Y => Bounds.Y;

    public ((int X, int Y), (int X, int Y)) FindWayPoints(Room other)
    {
        int x1 = 0;
        int y1 = 0;

        int x2 = 0;
        int y2 = 0;
        if (Left == other.Right)
        {
            x1 = Left + 1;
            y1 = (Math.Max(Top, other.Top) + Math.Min(Bottom, other.Bottom)) / 2;
            x2 = Left - 1;
            y2 = y1;
        }
        else if (Right == other.Left)
        {
            x1 = Right - 1;
            y1 = (Math.Max(Top, other.Top) + Math.Min(Bottom, other.Bottom)) / 2;
            x2 = Right + 1;
            y2 = y1;
        }

        if (Top == other.Bottom)
        {
            y1 = Top + 1;
            x1 = (Math.Max(Left, other.Left) + Math.Min(Right, other.Right)) / 2;
            y2 = Top - 1;
            x2 = x1;
        }
        else if (Bottom == other.Top)
        {
            y1 = Bottom - 1;
            x1 = (Math.Max(Left, other.Left) + Math.Min(Right, other.Right)) / 2;
            y2 = Bottom + 1;
            x2 = x1;
        }
        return ((x1, y1), (x2, y2));
    }

    public bool IsCorridor(int minRoomSize = 2)
    {
        return Height == minRoomSize || Width == minRoomSize;
    }

    internal void AddWayPoint((int X, int Y) waypoint)
    {
        _waypoints.Add(waypoint);
    }

    internal (int X, int Y) FindDoor(Room other)
    {
        int x = 0;
        int y = 0;
        if (Left == other.Right)
        {
            x = Left;
            y = (Math.Max(Top, other.Top) + Math.Min(Bottom, other.Bottom)) / 2;
        }
        else if (Right == other.Left)
        {
            x = Right;
            y = (Math.Max(Top, other.Top) + Math.Min(Bottom, other.Bottom)) / 2;
        }

        if (Top == other.Bottom)
        {
            y = Top;
            x = (Math.Max(Left, other.Left) + Math.Min(Right, other.Right)) / 2;
        }
        else if (Bottom == other.Top)
        {
            y = Bottom;
            x = (Math.Max(Left, other.Left) + Math.Min(Right, other.Right)) / 2;
        }
        return (x, y);
    }

    internal List<WayPointConnection> GetWayPointConnections()
    {
        var connections = new List<WayPointConnection>();
        for (int i = 0; i < _waypoints.Count; i++)
        {
            var wp1 = _waypoints[i];
            for (int j = i + 1; j < _waypoints.Count; j++)
            {
                var wp2 = _waypoints[j];
                connections.Add(new WayPointConnection
                {
                    PointA = wp1,
                    PointB = wp2
                });
            }
            connections.Add(new WayPointConnection
            {
                PointA = wp1,
                PointB = ((int)Center.X, (int)Center.Y),
                Weight = 0.75f
            });
        }
        return connections;
    }

    internal bool Intersects(Room other)
    {
        return other.Bounds.Intersects(Bounds);
    }

    internal void Move(int x, int y)
    {
        Bounds = new Rectangle(Bounds.X + x, Bounds.Y + y, Bounds.Width, Bounds.Height);
    }

    internal float TaxiDistance(Room room)
    {
        return MathF.Abs(Center.X - room.Center.X) + MathF.Abs(Center.Y - room.Center.Y);
    }
}
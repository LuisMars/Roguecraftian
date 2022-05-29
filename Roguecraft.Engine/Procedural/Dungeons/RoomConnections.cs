namespace Roguecraft.Engine.Procedural.Dungeons;

public class RoomConnections
{
    private readonly List<RoomConnection> _connections = new();

    public List<(int X, int Y)> Positions => _connections.Select(c => c.Position).ToList();

    internal void Add(Room a, Room b)
    {
        var connection = new RoomConnection
        {
            RoomA = a,
            RoomB = b,
            Position = a.FindDoor(b)
        };
        _connections.Add(connection);
    }

    internal int Count(Room r)
    {
        return _connections.Count(connection => connection.Contains(r));
    }

    internal bool HasDoorAt((int x, int y) position)
    {
        return _connections.Any(c => c.Position == position);
    }

    internal bool HasRoomDoorAtX(Room room, int x, out List<int> ys)
    {
        ys = new List<int>();
        var connections = _connections.Where(c => (c.RoomA == room || c.RoomB == room) && c.Position.X == x);
        if (!connections.Any())
        {
            return false;
        }

        ys = connections.Select(c => c.Position.Y).OrderBy(x => x).ToList();
        return true;
    }

    internal bool HasRoomDoorAtY(Room room, int y, out List<int> xs)
    {
        xs = new List<int>();
        var connections = _connections.Where(c => (c.RoomA == room || c.RoomB == room) && c.Position.Y == y);
        if (!connections.Any())
        {
            return false;
        }

        xs = connections.Select(c => c.Position.X).OrderBy(y => y).ToList();
        return true;
    }

    internal List<Room> NeighboursOf(Room current)
    {
        return _connections.Where(c => c.Contains(current)).Select(c =>
        {
            if (c.RoomA == current)
            {
                return c.RoomB;
            }
            return c.RoomA;
        }).ToList();
    }
}
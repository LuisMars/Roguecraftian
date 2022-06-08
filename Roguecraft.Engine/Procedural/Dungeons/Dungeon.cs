using MonoGame.Extended;
using System.Diagnostics;

namespace Roguecraft.Engine.Procedural.Dungeons;

public class Dungeon
{
    private const int AverageRoomSize = 10;
    private const int MaxDungeonSize = 100;
    private const int MaxRoomSize = 20;
    private const int MinRoomSize = 2;
    private readonly FastRandom _random;
    private readonly List<WayPointConnection> _waypointsConnections = new();

    public Dungeon()
    {
        _random = new FastRandom((int)(DateTime.Now.Ticks % int.MaxValue));
    }

    public RoomConnections Connections { get; set; } = new();
    public List<Room> Rooms { get; set; } = new();

    public void AddRoom()
    {
        if (!Rooms.Any())
        {
            var room = RandomRoom();
            Rooms.Add(room);
            return;
        }
        var tries = 0;

        var rooms = Rooms.Where(r => r.IsCorridor(MinRoomSize) && Connections.Count(r) == 1 && r.CanHaveConnections).ToList();
        if (!rooms.Any())
        {
            rooms = Rooms.Where(r => r.CanHaveConnections).ToList();
        }
        var roomIndex = _random.Next(rooms.Count - 1);
        var other = rooms[roomIndex];
        while (tries < 1000)
        {
            tries++;

            var newRoom = GenerateBottomRoom(other);
            if (CanAdd(newRoom))
            {
                SetRoom(other, newRoom);
                return;
            }
            newRoom = GenerateTopRoom(other);
            if (CanAdd(newRoom))
            {
                SetRoom(other, newRoom);
                return;
            }
            newRoom = GenerateRightRoom(other);
            if (CanAdd(newRoom))
            {
                SetRoom(other, newRoom);
                return;
            }
            newRoom = GenerateLeftRoom(other);
            if (CanAdd(newRoom))
            {
                SetRoom(other, newRoom);
                return;
            }
        }
        other.CanHaveConnections = false;
    }

    public (int x, int y, int width, int height) Bounds()
    {
        var left = Rooms.Select(x => x.Left).Min();
        var top = Rooms.Select(x => x.Top).Min();
        var right = Rooms.Select(x => x.Right).Max();
        var bottom = Rooms.Select(x => x.Bottom).Max();

        return (left, top, right - left, bottom - top);
    }

    public Room? FindSpecialRoom()
    {
        return FindSpecialRoom(new());
    }

    public Room? FindSpecialRoom(List<Room> toAvoid)
    {
        var longestPath = GetLongestPath();
        longestPath.AddRange(toAvoid);
        Room? farthest = null;
        int maxLength = 0;
        foreach (var room in Rooms.Where(r => !r.IsCorridor() && !longestPath.Contains(r)))
        {
            var minLength = int.MaxValue;
            foreach (var path in longestPath)
            {
                var current = ShortestPathFrom(room, path).Count;
                minLength = Math.Min(current, minLength);
            }
            if (minLength > maxLength)
            {
                maxLength = minLength;
                farthest = room;
            }
        }
        return farthest;
    }

    public List<Room> GetLongestPath()
    {
        var path = new List<Room>();
        var maxDistance = 0f;
        if (Rooms.Count <= 1)
        {
            return path;
        }
        foreach (var room in Rooms)
        {
            var goal = FarthestRoomFrom(room);
            var current = ShortestPathFrom(room, goal);
            var distance = TotalDistance(current);
            if (maxDistance < distance)
            {
                path = current;
                maxDistance = distance;
            }
        }

        return path;
    }

    public List<WayPointConnection> GetWayPointConnections()
    {
        List<WayPointConnection> waypointConnections = new();
        waypointConnections.AddRange(_waypointsConnections);
        foreach (var room in Rooms)
        {
            waypointConnections.AddRange(room.GetWayPointConnections());
        }
        return waypointConnections;
    }

    public bool HasDoorAt((int x, int y) position)
    {
        return Connections.HasDoorAt(position);
    }

    public bool HasRoomDoorAtX(Room room, int x, out List<int> ys)
    {
        return Connections.HasRoomDoorAtX(room, x, out ys);
    }

    public bool HasRoomDoorAtY(Room room, int y, out List<int> xs)
    {
        return Connections.HasRoomDoorAtY(room, y, out xs);
    }

    private static List<Room> ReconstructPath(Dictionary<Room, Room> cameFrom, Room current)
    {
        var path = new List<Room> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        return path;
    }

    private bool CanAdd(Room newRoom)
    {
        foreach (var room in Rooms)
        {
            if (room.Intersects(newRoom))
            {
                return false;
            }
        }

        return true;
    }

    private Room FarthestRoomFrom(Room room)
    {
        Room farthest = room;
        var maxDist = 0f;
        foreach (var other in Rooms)
        {
            var distance = other.TaxiDistance(room);
            if (distance > maxDist)
            {
                farthest = other;
                maxDist = distance;
            }
        }
        return farthest;
    }

    private Room GenerateBottomRoom(Room other)
    {
        var room = RandomRoom(other);
        var xOffset = _random.Next(-room.Width + MinRoomSize, other.Width - MinRoomSize);
        room.Move(xOffset, other.Height);
        return room;
    }

    private Room GenerateLeftRoom(Room other)
    {
        var room = RandomRoom(other);
        var yOffset = _random.Next(-room.Height + MinRoomSize, other.Height - MinRoomSize);
        room.Move(-room.Width, yOffset);
        return room;
    }

    private Room GenerateRightRoom(Room other)
    {
        var room = RandomRoom(other);
        var yOffset = _random.Next(-room.Height + MinRoomSize, other.Height - MinRoomSize);
        room.Move(other.Width, yOffset);
        return room;
    }

    private Room GenerateTopRoom(Room other)
    {
        var room = RandomRoom(other);
        var xOffset = _random.Next(-room.Width + MinRoomSize, other.Width - MinRoomSize);
        room.Move(xOffset, -room.Height);
        return room;
    }

    private (int width, int height) GetRandomRoomSize(bool connectedToCorridor)
    {
        var width = RandomRoomSide();
        var height = RandomRoomSide();
        var corridorChange = 0.85f;
        if (connectedToCorridor)
        {
            corridorChange += 0.149f;
        }
        var isCorridor = _random.NextSingle() > corridorChange;
        if (isCorridor)
        {
            var isHorizontal = _random.NextSingle() > 0.5f;
            if (isHorizontal)
            {
                height = MinRoomSize;
            }
            else
            {
                width = MinRoomSize;
            }
            return (width, height);
        }

        if (width * height > MaxDungeonSize)
        {
            if (_random.NextSingle() > 0.5f)
            {
                width = MaxDungeonSize / height;
            }
            else
            {
                height = MaxDungeonSize / width;
            }
        }
        Debug.Assert(width >= MinRoomSize);
        Debug.Assert(height >= MinRoomSize);
        return (width, height);
    }

    private Room RandomRoom(Room other)
    {
        (var width, var height) = GetRandomRoomSize(!other.IsCorridor(MinRoomSize));
        var room = new Room(other.X, other.Y, width, height);
        return room;
    }

    private Room RandomRoom()
    {
        (var width, var height) = GetRandomRoomSize(false);
        var room = new Room(0, 0, width, height);
        return room;
    }

    private int RandomRoomSide()
    {
        return _random.Next(AverageRoomSize, MaxRoomSize);
    }

    private void SetRoom(Room other, Room newRoom)
    {
        Connections.Add(other, newRoom);
        var (pointA, pointB) = other.FindWayPoints(newRoom);
        _waypointsConnections.Add(new WayPointConnection
        {
            PointA = pointA,
            PointB = pointB,
            Weight = 0.5f
        });

        other.AddWayPoint(pointA);
        newRoom.AddWayPoint(pointB);

        Rooms.Add(newRoom);
    }

    private List<Room> ShortestPathFrom(Room start, Room goal)
    {
        var closedSet = new List<Room>();
        var openSet = new List<Room> { start };
        var cameFrom = new Dictionary<Room, Room>();

        var gScore = new Dictionary<Room, float>
            {
                { start, 0 },
            };

        var fScore = new Dictionary<Room, float>
            {
                { start, start.TaxiDistance(goal) },
            };

        while (openSet.Any())
        {
            var current = openSet.MinBy(r => fScore[r]);
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }
            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in Connections.NeighboursOf(current).Where(r => !closedSet.Contains(r)))
            {
                var tentativeGScore = gScore[current] + neighbor.TaxiDistance(current);
                if (!closedSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + neighbor.TaxiDistance(goal);
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }

    private float TotalDistance(List<Room> path)
    {
        if (path == null)
        {
            return 0;
        }
        var distance = 0f;
        var current = path[0];
        foreach (var room in path)
        {
            distance += room.TaxiDistance(current);
            current = room;
        }
        return distance;
    }
}
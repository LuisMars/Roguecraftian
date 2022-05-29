namespace Roguecraft.Engine.Procedural.Dungeons;

public class RoomConnection
{
    public (int X, int Y) Position { get; set; }
    public Room RoomA { get; set; }
    public Room RoomB { get; set; }

    public bool Contains(Room room)
    {
        return RoomA == room || RoomB == room;
    }
}
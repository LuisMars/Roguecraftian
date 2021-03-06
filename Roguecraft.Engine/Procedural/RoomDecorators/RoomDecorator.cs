using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Procedural.RoomDecorators.RoomRules;
using Roguecraft.Engine.Random;

namespace Roguecraft.Engine.Procedural.RoomDecorators;

public class RoomDecorator
{
    private readonly RandomGenerator _random;

    public RoomDecorator(RandomGenerator random)
    {
        _random = random;
    }

    private bool HasChurch { get; set; }

    public char[,] Decorate(Dungeon dungeon, Room room, bool isInMainPath, bool isStart, bool isEnd, bool isSpecial)
    {
        RoomRulesBase roomRules = new EmptyRoomRules(dungeon, room, _random);
        if (isStart)
        {
            roomRules = new StartRoomRules(dungeon, room, _random);
        }
        else if (isEnd)
        {
        }
        else if (isSpecial)
        {
            roomRules = new SpecialRoomRule(dungeon, room, _random);
        }
        else if (isInMainPath)
        {
            roomRules = new PathRoomRules(dungeon, room, _random);
        }
        else
        {
            if (!HasChurch)
            {
                roomRules = new ChurchRoomRules(dungeon, room, _random);

                if (roomRules.Apply())
                {
                    HasChurch = true;
                    return roomRules.Map;
                }
            }

            if (_random.Next(1) == 0)
            {
                roomRules = new LibraryRoomRules(dungeon, room, _random);
            }
            else
            {
                roomRules = new BedroomRoomRules(dungeon, room, _random);
            }
        }

        roomRules.Apply();
        return roomRules.Map;
    }
}
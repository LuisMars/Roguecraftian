using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Random;

namespace Roguecraft.Engine.Procedural.RoomDecorators.RoomRules;

public class EmptyRoomRules : RoomRulesBase
{
    public EmptyRoomRules(Dungeon dungeon, Room room, RandomGenerator random) : base(dungeon, room, random)
    {
    }
}
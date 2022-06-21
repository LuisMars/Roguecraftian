using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Procedural.RoomDecorators.Rules;
using Roguecraft.Engine.Random;

namespace Roguecraft.Engine.Procedural.RoomDecorators.RoomRules;

internal class LibraryRoomRules : RoomRulesBase
{
    public LibraryRoomRules(Dungeon dungeon, Room room, RandomGenerator random) : base(dungeon, room, random)
    {
        Rules = new()
        {
            new()
            {
                new RoomShapeRule(),
                new TorchRule(),
                new BookshelfRule(),
                new BookshelfDoubleRule(),
                new TableRule(),
                new ChairRule()
            },
            new()
            {
                new EnemyRule
                {
                    MaxOccurences = 2
                }
            },
        };
    }
}
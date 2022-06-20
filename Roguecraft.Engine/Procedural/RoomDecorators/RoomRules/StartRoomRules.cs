using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Procedural.RoomDecorators.Rules;
using Roguecraft.Engine.Random;

namespace Roguecraft.Engine.Procedural.RoomDecorators.RoomRules;

public class StartRoomRules : RoomRulesBase
{
    public StartRoomRules(Dungeon dungeon, Room room, RandomGenerator random) : base(dungeon, room, random)
    {
        Rules = new()
        {
            new()
            {
                new StartRule()
            },
            new()
            {
                new TorchRule(),
                new WheelchairRule(),
                new PlantRule(),
                new BarrelRule(),
                new BarrelGrowRule(),
                new BarrelReductionRule(),
                new BarrelRowReductionRule(),
                new BookshelfRule()
                {
                    MaxOccurences = 1
                },
                new CouchRule()
                {
                    MaxOccurences = 1
                },
                new RoomShapeRule(),
                new BedRule()
                {
                    MaxOccurences = 1
                },
                new DeskRule(),
                new ArmorStandRule()
                {
                    MaxOccurences = 1
                }
            }
        };
    }
}
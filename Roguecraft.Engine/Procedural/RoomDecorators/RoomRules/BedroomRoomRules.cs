using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Procedural.RoomDecorators.Rules;
using Roguecraft.Engine.Random;

namespace Roguecraft.Engine.Procedural.RoomDecorators.RoomRules;

internal class BedroomRoomRules : RoomRulesBase
{
    public BedroomRoomRules(Dungeon dungeon, Room room, RandomGenerator random) : base(dungeon, room, random)
    {
        Rules = new()
        {
            new()
            {
                new RoomShapeRule(),
                new ArmorStandRule
                {
                    MaxOccurences = 1
                },
                new BookshelfRule
                {
                    MaxOccurences = 2
                },
                new PlantRule(),
                new BedRule(),
                new WheelchairRule(),
                new DeskRule(),
                new TorchRule()
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
using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Procedural.RoomDecorators.Rules;
using Roguecraft.Engine.Random;

namespace Roguecraft.Engine.Procedural.RoomDecorators.RoomRules;

public class PathRoomRules : RoomRulesBase
{
    public PathRoomRules(Dungeon dungeon, Room room, RandomGenerator random) : base(dungeon, room, random)
    {
        Rules = new()
        {
            new()
            {
                new TorchRule(),
                new StatueRule(),
                new PlantRule(),
                new WheelchairRule(),
                new EnemyRule()
                {
                    MaxOccurences = 2
                }
            }
        };
    }
}
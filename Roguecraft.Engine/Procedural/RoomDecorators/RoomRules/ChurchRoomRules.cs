using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Procedural.RoomDecorators.Rules;
using Roguecraft.Engine.Random;

namespace Roguecraft.Engine.Procedural.RoomDecorators.RoomRules;

internal class ChurchRoomRules : RoomRulesBase
{
    public ChurchRoomRules(Dungeon dungeon, Room room, RandomGenerator random) : base(dungeon, room, random)
    {
        Rules = new()
        {
            new()
            {
                new PodiumRule
                {
                    MaxOccurences = 1
                },
                new PodiumExtraRule(),
                new PewRule()
            },
            new()
            {
                new EnemyPewRule(),
                new EnemyRule()
            }
        };
    }
}
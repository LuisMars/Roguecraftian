using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Procedural.RoomDecorators.Rules;
using Roguecraft.Engine.Random;

namespace Roguecraft.Engine.Procedural.RoomDecorators.RoomRules;

public class SpecialRoomRule : RoomRulesBase
{
    public SpecialRoomRule(Dungeon dungeon, Room room, RandomGenerator random) : base(dungeon, room, random)
    {
        Rules = new()
        {
            new()
            {
                new RitualRule()
                {
                    MaxOccurences = 1
                }
            },
            new()
            {
                new TorchRule(),
                new RoomShapeRule()
            }
        };
    }
}
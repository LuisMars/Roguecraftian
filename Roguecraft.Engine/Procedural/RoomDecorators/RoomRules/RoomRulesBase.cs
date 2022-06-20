using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Random;

namespace Roguecraft.Engine.Procedural.RoomDecorators.RoomRules;

public abstract class RoomRulesBase
{
    private readonly Dungeon _dungeon;
    private readonly RandomGenerator _random;

    private readonly Room _room;

    public RoomRulesBase(Dungeon dungeon, Room room, RandomGenerator random)
    {
        _dungeon = dungeon;
        _room = room;
        _random = random;
    }

    public char[,] Map { get; set; }
    protected List<List<ReplacementRuleBase>> Rules { get; set; } = new();

    public void Apply()
    {
        Map = GenerateInitialMap();
        var start = 0;
        while (start < Rules.Count)
        {
            var currentRules = Rules[start];
            if (currentRules.All(x => !x.CanApply) || !TryApplyRules(currentRules))
            {
                start++;
            }
        }
    }

    private char[,] GenerateInitialMap()
    {
        var map = new char[_room.Width + 1, _room.Height + 1];

        for (var x = 0; x <= _room.Width; x++)
        {
            for (var y = 0; y <= _room.Height; y++)
            {
                map[x, y] = 'F';
            }
        }
        for (var x = 0; x <= _room.Width; x++)
        {
            if (!_dungeon.HasDoorAt((_room.X + x, _room.Y)))
            {
                map[x, 0] = 'W';
            }
            else
            {
                map[x, 0] = 'D';
            }
            if (!_dungeon.HasDoorAt((_room.X + x, _room.Y + _room.Height)))
            {
                map[x, _room.Height] = 'W';
            }
            else
            {
                map[x, _room.Height] = 'D';
            }
        }
        for (var y = 0; y <= _room.Height; y++)
        {
            if (!_dungeon.HasDoorAt((_room.X, _room.Y + y)))
            {
                map[0, y] = 'W';
            }
            else
            {
                map[0, y] = 'D';
            }
            if (!_dungeon.HasDoorAt((_room.X + _room.Width, _room.Y + y)))
            {
                map[_room.Width, y] = 'W';
            }
            else
            {
                map[_room.Width, y] = 'D';
            }
        }

        return map;
    }

    private bool TryApplyRules(List<ReplacementRuleBase> Rules)
    {
        var applied = true;
        var appliedRules = 0;
        while (applied && appliedRules < 100)
        {
            applied = false;
            var rules = Rules.Shuffle(_random).Where(r => r.CanApply);
            foreach (var rule in rules)
            {
                if (rule.TryApply(_random, Map))
                {
                    appliedRules++;
                    applied = true;
                    break;
                }
            }
        }
        return applied;
    }
}
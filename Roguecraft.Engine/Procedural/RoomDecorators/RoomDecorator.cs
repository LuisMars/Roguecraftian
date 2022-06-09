using MonoGame.Extended.Collections;
using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Procedural.RoomDecorators.Rules;

namespace Roguecraft.Engine.Procedural.RoomDecorators;

public class RoomDecorator
{
    private readonly Random _random = new Random();
    //public List<ReplacementRule> Rules { get; set; } = new();

    public char[,] Decorate(Dungeon dungeon, Room room, bool isInMainPath, bool isStart, bool isEnd)
    {
        var map = GenerateInitialMap(dungeon, room);

        if (isStart/* || isEnd*/)
        {
            var stairsRule = new StairsRule();
            stairsRule.TryApply(_random, map);
        }

        var rules = new List<ReplacementRule>
        {
            new BarrelRule(),
            new BarrelToWallRule(),
            new BarrelReductionRule(),
            new RoomShapeRule(),
            new ArmorStandRule(),
            new TableRule(),
            //new LongTableRule(),
            new ChairRule(),
            new ChestRule(),
            new BookshelfRule(),
            new LargeBookshelfRule()
        };
        if (!isInMainPath)
        {
            rules.Add(new RitualRule());
            rules.Add(new BedRule());
            rules.Add(new DeskRule());
            rules.Add(new ChestRule());
            rules.Add(new ReduceChestRule());
        }
        ApplyRules(map, rules);
        if (!isStart && !isEnd)
        {
            var enemyRules = new List<ReplacementRule>
            {
                new EnemyRule(),
                new ReduceEnemiesRule(),
            };
            ApplyRules(map, enemyRules);
        }
        return map;
    }

    private static char[,] GenerateInitialMap(Dungeon dungeon, Room room)
    {
        var map = new char[room.Width + 1, room.Height + 1];

        for (var x = 0; x <= room.Width; x++)
        {
            for (var y = 0; y <= room.Height; y++)
            {
                map[x, y] = 'F';
            }
        }
        for (var x = 0; x <= room.Width; x++)
        {
            if (!dungeon.HasDoorAt((room.X + x, room.Y)))
            {
                map[x, 0] = 'W';
            }
            else
            {
                map[x, 0] = 'D';
            }
            if (!dungeon.HasDoorAt((room.X + x, room.Y + room.Height)))
            {
                map[x, room.Height] = 'W';
            }
            else
            {
                map[x, room.Height] = 'D';
            }
        }
        for (var y = 0; y <= room.Height; y++)
        {
            if (!dungeon.HasDoorAt((room.X, room.Y + y)))
            {
                map[0, y] = 'W';
            }
            else
            {
                map[0, y] = 'D';
            }
            if (!dungeon.HasDoorAt((room.X + room.Width, room.Y + y)))
            {
                map[room.Width, y] = 'W';
            }
            else
            {
                map[room.Width, y] = 'D';
            }
        }

        return map;
    }

    private void ApplyRules(char[,] map, List<ReplacementRule> Rules)
    {
        var applied = true;
        var appliedRules = 0;
        while (applied && appliedRules < 100)
        {
            applied = false;
            var rules = Rules.Shuffle(_random);
            foreach (var rule in rules)
            {
                if (rule.TryApply(_random, map))
                {
                    appliedRules++;
                    applied = true;
                    break;
                }
            }
        }
    }
}
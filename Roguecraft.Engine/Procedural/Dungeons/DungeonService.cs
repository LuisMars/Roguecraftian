using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Factories;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Procedural.Dungeons
{
    public class DungeonService
    {
        private readonly CollisionService _collisionService;
        private readonly Configuration _configuration;
        private readonly IActorFactory _doorFactory;
        private readonly Dungeon _dungeon;
        private readonly IActorFactory _enemyFactory;
        private readonly IActorFactory _heroFactory;
        private readonly IActorFactory _wallFactory;

        public DungeonService(Configuration configuration)
        {
            var path = _dungeon.GetLongestPath();
            var playerRoom = path.First();
            var endRoom = path.Last();
            var specialRoom = _dungeon.FindSpecialRoom();
        }

        public DungeonService(Configuration configuration,
                              CollisionService collisionService,
                              IActorFactory heroFactory,
                              IActorFactory enemyFactory,
                              IActorFactory wallFactory,
                              IActorFactory doorFactory)
        {
            _configuration = configuration;

            _collisionService = collisionService;
            _heroFactory = heroFactory;
            _enemyFactory = enemyFactory;
            _wallFactory = wallFactory;
            _doorFactory = doorFactory;

            _dungeon = new Dungeon();
            for (int i = 0; i < _configuration.RoomsPerDungeon; i++)
            {
                _dungeon.AddRoom();
            }
            var longestPath = _dungeon.GetLongestPath();
            Start = longestPath.First();
        }

        public RectangleF Bounds
        {
            get
            {
                var (x, y, width, height) = _dungeon.Bounds();
                return new RectangleF(x * _configuration.WallSize,
                                      y * _configuration.WallSize,
                                     (width + 1) * _configuration.WallSize,
                                     (height + 1) * _configuration.WallSize);
            }
        }

        private Room End { get; set; }
        private Room Start { get; set; }

        public void Initialize()
        {
            _collisionService.Initialize(Bounds);
            AddWalls();
            AddDoors();
            AddPlayer();
            AddEnemies();
        }

        private void AddDoors()
        {
            foreach (var (x, y) in _dungeon.Connections.Positions)
            {
                _doorFactory.Add(new Vector2(x, y) * _configuration.WallSize);
            }
        }

        private void AddEnemies()
        {
            foreach (var room in _dungeon.Rooms)
            {
                if (room != Start)
                {
                    continue;
                }
                AddEnemy(room.Center + new Vector2(1.5f));
            }
        }

        private void AddEnemy(Vector2 position)
        {
            _enemyFactory.Add(position * _configuration.WallSize);
        }

        private void AddPlayer()
        {
            _heroFactory.Add((Start.Center + new Vector2(0.5f)) * _configuration.WallSize);
        }

        private void AddWalls()
        {
            var walls = new HashSet<Vector2>();

            var offset = new Point(-Math.Min(0, _dungeon.Rooms.Min(r => r.Left)), -Math.Min(0, _dungeon.Rooms.Min(r => r.Top)));
            var sizeBounds = new Point(_dungeon.Rooms.Max(r => r.Right) + offset.X, _dungeon.Rooms.Max(r => r.Bottom) + offset.Y);
            var cells = new bool[sizeBounds.X + offset.X + 1, sizeBounds.Y + offset.Y + 1];

            foreach (var room in _dungeon.Rooms)
            {
                for (int x = room.Left; x <= room.Right; x++)
                {
                    cells[offset.X + x, offset.Y + room.Top] = true;
                    cells[offset.X + x, offset.Y + room.Bottom] = true;
                }
                for (int y = room.Top; y <= room.Bottom; y++)
                {
                    cells[offset.X + room.Left, offset.Y + y] = true;
                    cells[offset.X + room.Right, offset.Y + y] = true;
                }
            }

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    if (!cells[x, y] || _dungeon.HasDoorAt((x - offset.X, y - offset.Y)))
                    {
                        continue;
                    }
                    _wallFactory.Add(new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize));
                }
            }
        }
    }
}
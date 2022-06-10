using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Factories;
using Roguecraft.Engine.Procedural.RoomDecorators;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Procedural.Dungeons
{
    public class DungeonService
    {
        private readonly CollisionService _collisionService;
        private readonly Configuration _configuration;
        private readonly DecorationFactory _decorationFactory;
        private readonly IActorFactory _doorFactory;
        private readonly Dungeon _dungeon;
        private readonly Room _end;
        private readonly IActorFactory _enemyFactory;
        private readonly FloorDecorationFactory _floorDecorationFactory;
        private readonly IActorFactory _heroFactory;
        private readonly List<Room> _longestPath;
        private readonly MoveableDecorationFactory _moveableDecorationFactory;
        private readonly IActorFactory _potionFactory;
        private readonly RoomDecorator _roomDecorator;
        private readonly Room _special;
        private readonly Room _start;
        private readonly IActorFactory _wallFactory;
        private readonly IActorFactory _weaponFactory;

        public DungeonService(Configuration configuration,
                              CollisionService collisionService,
                              IActorFactory heroFactory,
                              IActorFactory enemyFactory,
                              IActorFactory wallFactory,
                              IActorFactory doorFactory,
                              IActorFactory potionFactory,
                              IActorFactory weaponFactory,
                              DecorationFactory decorationFactory,
                              MoveableDecorationFactory moveableDecorationFactory,
                              FloorDecorationFactory floorDecorationFactory)
        {
            _configuration = configuration;
            _roomDecorator = new RoomDecorator();
            _collisionService = collisionService;
            _heroFactory = heroFactory;
            _enemyFactory = enemyFactory;
            _wallFactory = wallFactory;
            _doorFactory = doorFactory;
            _potionFactory = potionFactory;
            _weaponFactory = weaponFactory;
            _decorationFactory = decorationFactory;
            _moveableDecorationFactory = moveableDecorationFactory;
            _floorDecorationFactory = floorDecorationFactory;
            _dungeon = new Dungeon();
            for (int i = 0; i < _configuration.RoomsPerDungeon; i++)
            {
                _dungeon.AddRoom();
            }
            _longestPath = _dungeon.GetLongestPath();
            _start = _longestPath.First();
            _end = _longestPath.Last();
            _special = _dungeon.FindSpecialRoom();
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

        public void Initialize()
        {
            _collisionService.Initialize(Bounds);

            AddCells();
        }

        private void AddCells()
        {
            var walls = new HashSet<Vector2>();

            var offset = new Point(-Math.Min(0, _dungeon.Rooms.Min(r => r.Left)), -Math.Min(0, _dungeon.Rooms.Min(r => r.Top)));
            var sizeBounds = new Point(_dungeon.Rooms.Max(r => r.Right) + offset.X, _dungeon.Rooms.Max(r => r.Bottom) + offset.Y);
            var cells = new char[sizeBounds.X + offset.X + 1, sizeBounds.Y + offset.Y + 1];
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x, y] = '_';
                }
            }
            foreach (var room in _dungeon.Rooms)
            {
                var isInPath = _longestPath.Contains(room);
                var isStart = _start == room;
                var isEnd = _end == room;
                var map = _roomDecorator.Decorate(_dungeon, room, isInPath, isStart, isEnd);
                for (var i = 0; i < map.GetLength(0); i++)
                {
                    for (var j = 0; j < map.GetLength(1); j++)
                    {
                        cells[offset.X + room.Left + i, offset.Y + room.Top + j] = map[i, j];
                    }
                }
            }

            var s = "";
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    var c = cells[x, y];
                    if (c == '_' || c == 'F')
                    {
                        c = ' ';
                    }
                    s += c;
                }
                s += "\n";
            }
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'W')
                    {
                        continue;
                    }
                    //cells[x, y] = '_';
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _wallFactory.Add(position);
                }
            }
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'D')
                    {
                        continue;
                    }
                    //cells[x, y] = 'F';
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _doorFactory.Add(position);
                }
            }
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'S')
                    {
                        continue;
                    }
                    //cells[x, y] = 'F';
                    var position = new Vector2((x - offset.X + 0.5f) * _configuration.WallSize, (y - offset.Y + 0.5f) * _configuration.WallSize);

                    _heroFactory.Add(position);
                }
            }
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'E')
                    {
                        continue;
                    }
                    //cells[x, y] = 'F';
                    var position = new Vector2((x - offset.X + 0.5f) * _configuration.WallSize, (y - offset.Y + 0.5f) * _configuration.WallSize);
                    _enemyFactory.Add(position);
                }
            }
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'T')
                    {
                        continue;
                    }

                    //for (var i = 0; i < 2; i++)
                    //{
                    //    for (var j = 0; j < 2; j++)
                    //    {
                    //        cells[x + i, y + j] = 'F';
                    //    }
                    //}
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _moveableDecorationFactory.AddTable(position, new Vector2(2), "Table");
                    x += 2;
                    y += 2;
                }
            }
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'C')
                    {
                        continue;
                    }
                    //cells[x, y] = 'F';
                    var position = new Vector2((x - offset.X + 0.5f) * _configuration.WallSize, (y - offset.Y + 0.5f) * _configuration.WallSize);

                    _moveableDecorationFactory.AddChair(position);
                }
            }
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'A')
                    {
                        continue;
                    }
                    //cells[x, y] = 'F';
                    var position = new Vector2((x - offset.X + 0.5f) * _configuration.WallSize, (y - offset.Y + 0.5f) * _configuration.WallSize);

                    _weaponFactory.Add(position);
                }
            }
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'B')
                    {
                        continue;
                    }
                    //cells[x, y] = 'F';
                    var width = 1;
                    var height = 1;
                    var spriteEffect = SpriteEffects.None;
                    if (cells[x - 1, y] == 'W')
                    {
                        spriteEffect = SpriteEffects.FlipVertically;
                    }
                    //if (cells[x + 1, y] == 'B')
                    //{
                    //    width = 2;
                    //    cells[x + 1, y] = 'F';
                    //}
                    //if (cells[x, y + 1] == 'B')
                    //{
                    //    height = 2;
                    //    cells[x, y + 1] = 'F';
                    //}
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _decorationFactory.AddBookshelf(position, new Vector2(width, height), spriteEffect);
                }
            }
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'R')
                    {
                        continue;
                    }
                    //for (var i = 0; i < 4; i++)
                    //{
                    //    for (var j = 0; j < 4; j++)
                    //    {
                    //        cells[x + i, y + j] = 'F';
                    //    }
                    //}
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _floorDecorationFactory.Add(position, new Vector2(4), "Ritual");
                    x += 4;
                    y += 4;
                }
            }

            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'J')
                    {
                        continue;
                    }
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _moveableDecorationFactory.AddBarrel(position, "Barrel");
                }
            }

            //for (var x = 0; x < cells.GetLength(0); x++)
            //{
            //    for (var y = 0; y < cells.GetLength(1); y++)
            //    {
            //        if (cells[x, y] == 'F' || cells[x, y] == '_')
            //        {
            //            continue;
            //        }

            //        var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);
            //        _decorationFactory.Add(position);
            //    }
            //}
        }
    }
}
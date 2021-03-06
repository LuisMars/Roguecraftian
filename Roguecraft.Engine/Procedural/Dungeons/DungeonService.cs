using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Roguecraft.Engine.Actors;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.Factories;
using Roguecraft.Engine.Helpers;
using Roguecraft.Engine.Procedural.RoomDecorators;
using Roguecraft.Engine.Random;
using Roguecraft.Engine.Simulation;

namespace Roguecraft.Engine.Procedural.Dungeons
{
    public class DungeonService
    {
        private readonly CollisionService _collisionService;
        private readonly Configuration _configuration;
        private readonly Dungeon _dungeon;
        private readonly Room _end;
        private readonly List<Room> _longestPath;
        private readonly RandomGenerator _random;
        private readonly RoomDecorator _roomDecorator;
        private readonly Spawner _spawner;
        private readonly Room _special;
        private readonly Room _start;

        public DungeonService(RandomGenerator randomGenerator,
                              Configuration configuration,
                              CollisionService collisionService,
                              Spawner spawner)
        {
            _random = randomGenerator;
            _configuration = configuration;
            _roomDecorator = new RoomDecorator(_random);
            _collisionService = collisionService;
            _spawner = spawner;
            _dungeon = new Dungeon(_random);
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

        private static TextureRotation GetRotation(char[,] cells, int x, int y)
        {
            var rotation = TextureRotation.None;
            if (cells[x, y + 1] == 'W')
            {
                rotation = TextureRotation.HalfTurn;
            }
            else if (cells[x - 1, y] == 'W')
            {
                rotation = TextureRotation.AntiClockwise;
            }
            else if (cells[x + 1, y] == 'W')
            {
                rotation = TextureRotation.Clockwise;
            }

            return rotation;
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
                var isSpecial = _special == room;
                var map = _roomDecorator.Decorate(_dungeon, room, isInPath, isStart, isEnd, isSpecial);
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
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _spawner.Add<Wall>(position);
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
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _spawner.Add<Door>(position);
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
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _spawner.Add<Hero>(position);
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
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);
                    _spawner.Add<Enemy>(position);
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

                    for (var i = 0; i < 2; i++)
                    {
                        for (var j = 0; j < 2; j++)
                        {
                            cells[x + i, y + j] = 'F';
                        }
                    }
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _spawner.AddTable(position, new Vector2(2));
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
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _spawner.AddChair(position);
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
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    if (_random.Float() > 0.5f)
                    {
                        _spawner.Add<Potion>(position);
                    }
                    else
                    {
                        _spawner.Add<Weapon>(position);
                    }
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

                    var rotation = GetRotation(cells, x, y);

                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _spawner.AddBookshelf(position, rotation);
                }
            }
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'p')
                    {
                        continue;
                    }

                    var rotation = GetRotation(cells, x, y);

                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _spawner.AddPlant(position, rotation);
                }
            }

            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 't')
                    {
                        continue;
                    }

                    var rotation = GetRotation(cells, x, y);

                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _spawner.AddTorch(position, rotation);
                }
            }
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 's')
                    {
                        continue;
                    }

                    var rotation = GetRotation(cells, x, y);

                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _spawner.AddStatue(position, rotation);
                }
            }
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'w')
                    {
                        continue;
                    }

                    var rotation = GetRotation(cells, x, y);

                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _spawner.AddWheelchair(position, rotation);
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
                    for (var i = 0; i < 4; i++)
                    {
                        for (var j = 0; j < 4; j++)
                        {
                            cells[x + i, y + j] = 'F';
                        }
                    }
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _spawner.AddFloorDecoration(position, new Vector2(4));
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

                    _spawner.AddBarrel(position);
                }
            }

            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != '$')
                    {
                        continue;
                    }
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);

                    _spawner.Add<Potion>(position);
                }
            }

            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'Z')
                    {
                        continue;
                    }
                    var width = 1;
                    var height = 1;

                    cells[x, y] = 'F';
                    if (cells[x, y + 1] == 'Z')
                    {
                        cells[x, y + 1] = 'F';
                        height++;
                    }
                    else if (cells[x + 1, y] == 'Z')
                    {
                        width++;
                        cells[x + 1, y] = 'F';
                    }

                    var rotation = GetRotation(cells, x, y);
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);
                    _spawner.AddBed(position, new Vector2(width, height), rotation);
                }
            }

            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'P')
                    {
                        continue;
                    }
                    var width = 1;
                    var height = 1;

                    var rotation = TextureRotation.HalfTurn;
                    if (cells[x, y + 1] == 'P')
                    {
                        rotation = TextureRotation.Clockwise;
                        if (cells[x + 1, y] == 'ꝓ')
                        {
                            rotation = TextureRotation.AntiClockwise;
                        }
                    }
                    else if (cells[x, y + 1] == 'ꝓ')
                    {
                        rotation = TextureRotation.None;
                    }

                    cells[x, y] = 'F';
                    if (cells[x, y + 1] == 'P')
                    {
                        cells[x, y + 1] = 'F';
                        height++;
                    }
                    else if (cells[x + 1, y] == 'P')
                    {
                        width++;
                        cells[x + 1, y] = 'F';
                    }

                    if (rotation == TextureRotation.HalfTurn || rotation == TextureRotation.None)
                    {
                        var increment = 2;
                        if (rotation == TextureRotation.HalfTurn)
                        {
                            increment = -2;
                        }
                        var i = y + increment;
                        var cell = cells[x, i];

                        do
                        {
                            cells[x, i - increment / 2] = 'F';
                            cells[x, i + 1 - increment / 2] = 'F';
                            cells[x, i] = 'F';
                            cells[x, i + 1] = 'F';

                            var p = new Vector2((x - offset.X) * _configuration.WallSize, (i - offset.Y) * _configuration.WallSize);
                            _spawner.AddPew(p, new Vector2(width, height), rotation);
                            i += increment;
                            cell = cells[x, i];
                        }
                        while (cell == 'ꝓ');
                    }
                    if (rotation == TextureRotation.AntiClockwise || rotation == TextureRotation.Clockwise)
                    {
                        var increment = 2;
                        if (rotation == TextureRotation.Clockwise)
                        {
                            increment = -2;
                        }
                        var i = x + increment;
                        var cell = cells[i, y];

                        do
                        {
                            cells[i - increment / 2, y] = 'F';
                            cells[i - increment / 2, y + 1] = 'F';
                            cells[i, y] = 'F';
                            cells[i, y + 1] = 'F';

                            var p = new Vector2((i - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);
                            _spawner.AddPew(p, new Vector2(width, height), rotation);
                            i += increment;
                            cell = cells[i, y];
                        }
                        while (cell == 'ꝓ');
                    }
                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);
                    _spawner.AddPodium(position, new Vector2(width, height), rotation);
                }
            }

            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y] != 'c')
                    {
                        continue;
                    }
                    var width = 1;
                    var height = 1;

                    cells[x, y] = 'F';
                    if (cells[x, y + 1] == 'c')
                    {
                        cells[x, y + 1] = 'F';
                        height++;
                    }
                    else if (cells[x + 1, y] == 'c')
                    {
                        width++;
                        cells[x + 1, y] = 'F';
                    }

                    var rotation = GetRotation(cells, x, y);

                    var position = new Vector2((x - offset.X) * _configuration.WallSize, (y - offset.Y) * _configuration.WallSize);
                    _spawner.AddCouch(position, new Vector2(width, height), rotation);
                }
            }
        }
    }
}
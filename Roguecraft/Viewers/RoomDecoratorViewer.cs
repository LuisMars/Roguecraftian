using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using Roguecraft.Engine.Procedural.Dungeons;
using Roguecraft.Engine.Procedural.RoomDecorators;
using System.Collections.Generic;

namespace Roguecraft.Viewers;

public class RoomDecoratorViewer : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;

    public RoomDecoratorViewer()
    {
        Dungeon = new Dungeon();
        RoomDecorator = new RoomDecorator();
        _graphics = new GraphicsDeviceManager(this)
        {
            GraphicsProfile = GraphicsProfile.HiDef
        };

        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        for (var _ = 0; _ < 50; _++)
        {
            Dungeon.AddRoom();
        }
        LongestPath = Dungeon.GetLongestPath();
        LoadMap();
    }

    public List<Room> LongestPath { get; private set; } = new();

    public List<Room> SpecialRooms { get; private set; } = new();

    private Room CurrentRoom { get; set; }

    private int CurrentRoomIndex { get; set; } = 0;

    private Dungeon Dungeon { get; set; }

    private char[,] Map { get; set; }

    private RoomDecorator RoomDecorator { get; set; }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();

        for (var x = 0; x <= CurrentRoom.Width; x++)
        {
            for (var y = 0; y <= CurrentRoom.Height; y++)
            {
                DrawTile(x, y);
            }
        }

        _spriteBatch.End();
    }

    protected override void Update(GameTime gameTime)
    {
        var state = KeyboardExtended.GetState();
        if (state.WasKeyJustDown(Keys.Up))
        {
            CurrentRoomIndex = (CurrentRoomIndex + 1) % Dungeon.Rooms.Count;
            LoadMap();
        }
        if (state.WasKeyJustDown(Keys.Down))
        {
            CurrentRoomIndex = (CurrentRoomIndex - 1) % Dungeon.Rooms.Count;
            if (CurrentRoomIndex < 0)
            {
                CurrentRoomIndex = Dungeon.Rooms.Count - 1;
            }
            LoadMap();
        }
    }

    private void DrawTile(int x, int y, int offset = 50, int tileSize = 25)
    {
        var color = Map[x, y] switch
        {
            'F' => new Color(0.1f, 0.1f, 0.1f, 1), // Floor
            'W' => Color.White, // Wall
            'D' => Color.Red, // Door
            'B' => Color.Purple, // Bookshelf, closet...
            'T' => Color.Brown, // Table
            'C' => Color.SaddleBrown, // Chair
            'A' => Color.Silver, // Armor stand
            'Z' => Color.LightSteelBlue, // Bed
            'R' => Color.Crimson, // Ritual, pentagram...
            'E' => Color.Green, // Enemy
            '$' => Color.Gold, // Chest
            'S' => Color.Pink, // Start
            _ => Color.Magenta
        };
        _spriteBatch.FillRectangle((x * tileSize) + offset, (y * tileSize) + offset, tileSize, tileSize, color);
        _spriteBatch.DrawRectangle((x * tileSize) + offset, (y * tileSize) + offset, tileSize, tileSize, Color.Black);
    }

    private void LoadMap()
    {
        CurrentRoom = Dungeon.Rooms[CurrentRoomIndex];
        var isInPath = LongestPath.Contains(CurrentRoom);
        var isStart = LongestPath[0] == CurrentRoom;
        var isEnd = LongestPath[^1] == CurrentRoom;
        Map = RoomDecorator.Decorate(Dungeon, CurrentRoom, isInPath, isStart, isEnd);
    }
}
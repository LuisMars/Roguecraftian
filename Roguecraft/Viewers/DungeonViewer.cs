using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using Roguecraft.Engine.Procedural.Dungeons;
using System.Collections.Generic;

namespace Roguecraft.Viewers;

public class DungeonViewer : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private readonly SpriteBatch _spriteBatch;

    private double elapsed = 0;

    public DungeonViewer()
    {
        Dungeon = new Dungeon();
        _graphics = new GraphicsDeviceManager(this)
        {
            GraphicsProfile = GraphicsProfile.HiDef
        };
        _graphics.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    public List<Room> LongestPath { get; private set; } = new();
    public List<Room> SpecialRooms { get; private set; } = new();
    private Dungeon Dungeon { get; set; }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();

        var scale = 5;
        var offset = 500;
        foreach (var room in Dungeon.Rooms)
        {
            var rect = room.Bounds;
            var color = Color.Blue;
            if (LongestPath.Contains(room))
            {
                color = Color.Red;
            }
            if (room.IsCorridor())
            {
                color = Color.Lerp(color, Color.Black, 0.5f);
            }
            if (SpecialRooms.Contains(room))
            {
                color = Color.Green;
            }
            _spriteBatch.FillRectangle(rect.Location.X * scale + offset,
                                       rect.Location.Y * scale + offset,
                                       rect.Width * scale,
                                       rect.Height * scale,
                                       color);
            _spriteBatch.DrawRectangle(rect.Location.X * scale + offset,
                                       rect.Location.Y * scale + offset,
                                       rect.Width * scale,
                                       rect.Height * scale,
                                       Color.Black);
        }

        foreach (var (X, Y) in Dungeon.Connections.Positions)
        {
            _spriteBatch.DrawCircle(X * scale + offset, Y * scale + offset, 2, 6, Color.Yellow);
        }

        foreach (var wp in Dungeon.GetWayPointConnections())
        {
            _spriteBatch.DrawLine(
                wp.PointA.X * scale + offset,
                wp.PointA.Y * scale + offset,
                wp.PointB.X * scale + offset,
                wp.PointB.Y * scale + offset,
                Color.Gray);
        }
        _spriteBatch.End();
    }

    protected override void Initialize()
    {
        GraphicsDevice.PresentationParameters.MultiSampleCount = 8;
        _graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
        var state = KeyboardExtended.GetState();
        if (state.WasKeyJustDown(Keys.S))
        {
            AddSpecialRoom();
        }
        if (state.WasKeyJustDown(Keys.R))
        {
            Dungeon = new Dungeon();
            SpecialRooms.Clear();
        }
        elapsed += gameTime.ElapsedGameTime.TotalSeconds;
        if (elapsed < 0.25)
        {
            return;
        }
        elapsed = 0;

        Dungeon.AddRoom();
        LongestPath = Dungeon.GetLongestPath();
        var count = SpecialRooms.Count;
        SpecialRooms.Clear();
        for (var i = 0; i < count; i++)
        {
            AddSpecialRoom();
        }
    }

    private void AddSpecialRoom()
    {
        var specialRoom = Dungeon.FindSpecialRoom(SpecialRooms);
        SpecialRooms.Add(specialRoom);
    }

    private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
    {
        _graphics.PreferMultiSampling = true;
    }
}
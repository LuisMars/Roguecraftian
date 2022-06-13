using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguecraft.Engine.Core;
using Roguecraft.Engine.IoC;
using System;

namespace Roguecraft;

public class Main : Game
{
    private readonly GraphicsDeviceManager _graphics;

    private readonly Level _level;
    private readonly Services _services;

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            GraphicsProfile = GraphicsProfile.HiDef
        };

        _graphics.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        _graphics.SynchronizeWithVerticalRetrace = false; //Vsync
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / 60);
        _services = new Services(GraphicsDevice, _graphics, Content);
        _level = _services.Level;
    }

    protected override void Draw(GameTime gameTime)
    {
        _level.Draw((float)gameTime.ElapsedGameTime.TotalSeconds);
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
        _level.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs args)
    {
        args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        _graphics.PreferMultiSampling = true;
    }
}
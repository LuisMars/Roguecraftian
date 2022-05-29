using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Roguecraft.Engine.Core;

namespace Roguecraft;

public class Main : Game
{
    //private readonly ActorPool _actorPool;
    //private readonly CollisionService _collisionService;
    //private readonly ContentRepository _contentRepository;
    //private readonly CreatureFactory<Enemy> _enemyFactory;
    //private readonly GameLoop _gameLoop;
    private readonly GraphicsDeviceManager _graphics;

    //private readonly CreatureFactory<Hero> _heroFactory;
    private readonly Engine.Core.Level _level;

    //private readonly ShapeRenderer _shapeRenderer;
    //private readonly SpriteBatch _spriteBatch;
    //private readonly TextureRenderer _textureRenderer;
    //private readonly WallFactory _wallFactory;

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

        //_contentRepository = new ContentRepository(Content);

        //_actorPool = new ActorPool();
        //_collisionService = new CollisionService(_actorPool, new RectangleF(-10000, -10000, 20000, 20000));
        //_gameLoop = new GameLoop(_actorPool, _collisionService);
        //_textureRenderer = new TextureRenderer(_actorPool);
        //_shapeRenderer = new ShapeRenderer(_actorPool);
        //_heroFactory = new CreatureFactory<Hero>(_actorPool, _collisionService, _contentRepository);
        //_enemyFactory = new CreatureFactory<Enemy>(_actorPool, _collisionService, _contentRepository);
        //_wallFactory = new WallFactory(_actorPool, _collisionService, _contentRepository);
        //_heroFactory.Add(100, 100, "Hero");
        //_enemyFactory.Add(100, 160);
        //_wallFactory.Add(100, 190);
        //_spriteBatch = new SpriteBatch(GraphicsDevice);

        _level = new Level(GraphicsDevice, _graphics, Content);
    }

    protected override void Draw(GameTime gameTime)
    {
        _level.Draw();
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